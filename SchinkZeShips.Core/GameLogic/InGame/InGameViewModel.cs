using System;
using System.Linq;
using System.ServiceModel;
using SchinkZeShips.Core.ExtensionMethods;
using SchinkZeShips.Core.GameLobby;
using SchinkZeShips.Core.GameLogic.Board;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;

namespace SchinkZeShips.Core.GameLogic.InGame
{
	public class InGameViewModel : ViewModelWithCurrentGameBase
	{
		private const int InGameRefreshTimeoutInMs = 2500;
		private Game _currentGame;

		private CellViewModel _lastClickedCell;
		private string _statusText;
		private bool _displayActivityIndicator;

		public InGameViewModel() : base(InGameRefreshTimeoutInMs)
		{
			FireShotCommand = new Command(FireShotAsync, CanFireShot);
			SurrenderGameCommand = new Command(SurrenderGameAsync);
		}

		public Command FireShotCommand { get; }
		public Command SurrenderGameCommand { get; }

		public override Game CurrentGame
		{
			get => _currentGame;
			set
			{
				if (_currentGame != null)
				{
					if (_currentGame.LatestChangeTime == value?.LatestChangeTime)
						return;

					foreach (var cell in _currentGame.OtherPlayerBoard().Cells.SelectMany(c => c))
						cell.SelectedChanged -= CellSelectedChanged;
				}

				if (value == null)
				{
					Dialogs.Alert("Das Spiel existiert nicht mehr");
					PushViewModal(new StartView());
					return;
				}
				if (value.RunningGameState == null)
				{
					if (value.GameParticipant == null)
					{
						Dialogs.Alert("Der andere Spieler hat aufgegeben", "Gewonnen");
					}
					else
					{
						Dialogs.Alert("Sie haben das Spiel verloren", "Verloren");
					}
					PushViewModal(new GameLobbyView(value));
					return;
				}

				foreach (var cell in value.OtherPlayerBoard().Cells.SelectMany(c => c))
					cell.SelectedChanged += CellSelectedChanged;

				_currentGame = value;

				UpdateStatusText();

				OnPropertyChanged();
				FireShotCommand.ChangeCanExecute();
			}
		}

		private void UpdateStatusText()
		{
			if (_currentGame.IsConfiguringBoard())
			{
				StatusText = "Der andere Spieler konfiguriert sein Feld…";
				DisplayActivityIndicator = true;
			}
			else if (_currentGame.RunningGameState.CurrentPlayerIsGameCreator != _currentGame.ThisPlayerIsGameCreator())
			{
				StatusText = "Der andere Spieler ist am Zug…";
				DisplayActivityIndicator = true;
			}
			else
			{
				StatusText = "Sie sind am Zug!";
				DisplayActivityIndicator = false;
			}
		}

		public string StatusText
		{
			get => _statusText;
			private set
			{
				if (_statusText == value) return;
				_statusText = value;
				OnPropertyChanged();
			}
		}

		private CellViewModel LastClickedCell
		{
			get => _lastClickedCell;
			set
			{
				if (!Equals(_lastClickedCell, value))
				{
					if (_lastClickedCell != null)
						_lastClickedCell.IsSelected = false;

					_lastClickedCell = value;
				}

				FireShotCommand.ChangeCanExecute();
			}
		}

		private CellViewModel SelectedCell => LastClickedCell?.IsSelected == true ? LastClickedCell : null;

		public bool DisplayActivityIndicator
		{
			get => _displayActivityIndicator;
			private set
			{
				if (_displayActivityIndicator == value) return;
				_displayActivityIndicator = value;
				OnPropertyChanged();
			}
		}

		private async void FireShotAsync()
		{
			ShowLoading("Richte Kanonen aus");

			if (!LastClickedCell.Model.HasShip)
				CurrentGame.RunningGameState.CurrentPlayerIsGameCreator = !CurrentGame.RunningGameState.CurrentPlayerIsGameCreator;

			LastClickedCell.Model.WasShot = true;

			await Service.UpdateGameState(CurrentGame.Id, CurrentGame.RunningGameState);

			HideLoading();

			LastClickedCell = null;

			UpdateStatusText();

			var shotShips = CurrentGame
				.OtherPlayerBoard()
				.Cells
				.SelectMany(c => c)
				.Count(c => c.Model.WasShot && c.Model.HasShip);

			if (shotShips == BoardStateViewModel.AmountOfCellsWithShips)
			{
				await Dialogs.AlertAsync("Sie haben das Spiel gewonnen!", "Gewonnen!", "Zurück zur Lobby");
				ShowLoading("Beende Spiel");

				try
				{
					await Service.UpdateGameState(CurrentGame.Id, null);

					CurrentGame.RunningGameState = null;
					PushViewModal(new GameLobbyView(CurrentGame));
				}
				catch (FaultException)
				{
					throw;
				}
				catch (Exception e) when (e is TimeoutException || e is CommunicationException)
				{
					Dialogs.AlertNoConnection();
				}
				finally
				{
					HideLoading();
				}
			}
		}

		private bool CanFireShot()
		{
			return SelectedCell?.Model?.WasShot == false &&
				   CurrentGame.RunningGameState != null &&
				   CurrentGame.ThisPlayerIsGameCreator() ==
			       CurrentGame.RunningGameState.CurrentPlayerIsGameCreator;
		}

		private async void SurrenderGameAsync()
		{
			var reallySurrender = await Dialogs.ConfirmAsync(
				"Möchten Sie wirklich aufgeben?", "Wirklich aufgeben?", "Aufgeben","Weiterspielen");

			if (!reallySurrender)
			{
				return;
			}

			ShowLoading("Verlasse Spiel");

			try
			{
				await Service.UpdateGameState(CurrentGame.Id, null);
				await Service.RemoveFromGame(CurrentGame.Id, Settings.Instance.UserId);

				CurrentGame = null;
			}
			catch (FaultException)
			{
				throw;
			}
			catch (Exception e) when (e is TimeoutException || e is CommunicationException)
			{
				Dialogs.AlertNoConnection();
			}
			finally
			{
				HideLoading();
			}
		}

		private void CellSelectedChanged(object sender, EventArgs e)
		{
			LastClickedCell = (CellViewModel) sender;
		}
	}
}