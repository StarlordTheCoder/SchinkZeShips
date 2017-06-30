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
		private const int InGameRefreshTimeoutInMs = 3000;
		private Game _currentGame;

		private CellViewModel _lastClickedCell;

		public InGameViewModel() : base(InGameRefreshTimeoutInMs)
		{
			FireShotCommand = new Command(FireShotAsync, CanFireShot);
			SurrenderGameCommand = new Command(SurrenderGame);
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

				if (value?.RunningGameState == null)
				{
					_currentGame = null;

					PushViewModal(new StartView());
					return;
				}

				foreach (var cell in value.OtherPlayerBoard().Cells.SelectMany(c => c))
					cell.SelectedChanged += CellSelectedChanged;

				_currentGame = value;


				if (_currentGame.IsConfiguringBoard())
					ShowLoading("Warte, bis der andere Spieler sein Feld konfiguriert");
				else if (_currentGame.RunningGameState.CurrentPlayerIsGameCreator != _currentGame.ThisPlayerIsGameCreator())
					ShowLoading("Warte, bis der andere Spieler seinen Zug beendet");
				else
					HideLoading();

				OnPropertyChanged();
				FireShotCommand.ChangeCanExecute();
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

		private async void FireShotAsync()
		{
			LastClickedCell.Model.WasShot = true;

			if (!LastClickedCell.Model.HasShip)
				CurrentGame.RunningGameState.CurrentPlayerIsGameCreator = !CurrentGame.RunningGameState.CurrentPlayerIsGameCreator;

			await Service.UpdateGameState(CurrentGame.Id, CurrentGame.RunningGameState);

			LastClickedCell = null;

			UpdateGamestateAsync();

			var shotShips = CurrentGame.OtherPlayerBoard().Cells.SelectMany(c => c)
				.Count(c => c.Model.WasShot && c.Model.HasShip);

			if (shotShips == BoardStateViewModel.AmountOfCellsWithShips)
			{
				Dialogs.Alert("Spiel Gewonnen");
				ShowLoading("Verlasse Spiel");

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
				catch (CommunicationException)
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
			return SelectedCell?.Model.WasShot == false &&
			       CurrentGame.ThisPlayerIsGameCreator() ==
			       CurrentGame.RunningGameState.CurrentPlayerIsGameCreator;
		}

		public async void SurrenderGame()
		{
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
			catch (CommunicationException)
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