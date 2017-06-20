using System;
using System.Linq;
using SchinkZeShips.Core.ExtensionMethods;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;
using SchinkZeShips.Core.GameLobby;
using System.ServiceModel;

namespace SchinkZeShips.Core.GameLogic.InGame
{
	public class InGameViewModel : ViewModelWithCurrentGameBase
	{
		private const int InGameRefreshTimeoutInMs = 3000;
		private Game _currentGame;
		public Command FireShotCommand { get; }
		public Command SurrenderGameCommand { get; }

		public InGameViewModel() : base(InGameRefreshTimeoutInMs)
		{
			FireShotCommand = new Command(FireShotAsync, () => _selectedCell != null && CurrentGame.ThisPlayerIsGameCreator() == CurrentGame.RunningGameState.CurrentPlayerIsGameCreator);
			SurrenderGameCommand = new Command(SurrenderGame);
		}

		private async void FireShotAsync()
		{
			_selectedCell.Model.WasShot = true;

			if (!_selectedCell.Model.HasShip)
			{
				CurrentGame.RunningGameState.CurrentPlayerIsGameCreator = !CurrentGame.RunningGameState.CurrentPlayerIsGameCreator;
			}

			await Service.UpdateGameState(CurrentGame.Id, CurrentGame.RunningGameState);

			FireShotCommand.ChangeCanExecute();

			UpdateGamestateAsync();

			var shotShips = CurrentGame.OtherPlayerBoard().Cells.SelectMany(c => c).Count(c => c.Model.WasShot && c.Model.HasShip);

			if(shotShips == 30)
			{
				Dialogs.Alert("Speil Gewonnen");
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

		public override Game CurrentGame
		{
			get { return _currentGame; }
			set
			{
				if (_currentGame != null)
				{
					if (_currentGame.LatestChangeTime == value?.LatestChangeTime)
					{
						// Ignore a change to the game if there were no changes
						return;
					}

					foreach (var cell in _currentGame.OtherPlayerBoard().Cells.SelectMany(c => c))
					{
						cell.SelectedChanged -= CellSelectedChanged;
					}
				}

				if (value?.RunningGameState == null)
				{
					_currentGame = null;

					PushViewModal(new StartView());
					return;
				}

				foreach (var cell in value.OtherPlayerBoard().Cells.SelectMany(c => c))
				{
					cell.SelectedChanged += CellSelectedChanged;
				}

				_currentGame = value;


				if (_currentGame.IsConfiguringBoard())
				{
					ShowLoading("Warte, bis der andere Spieler sein Feld konfiguriert");
				}
				else if (_currentGame.RunningGameState.CurrentPlayerIsGameCreator != _currentGame.ThisPlayerIsGameCreator())
				{
					ShowLoading("Warte, bis der andere Spieler seinen Zug beendet");
				}
				else
				{
					HideLoading();
				}

				OnPropertyChanged();
				FireShotCommand.ChangeCanExecute();
			}
		}

		private void CellSelectedChanged(object sender, EventArgs e)
		{
			if (_selectedCell != null)
			{
				_selectedCell.IsSelected = false;
			}

			_selectedCell = (CellViewModel) sender;

			if (_selectedCell.Model.WasShot)
			{
				_selectedCell.IsSelected = false;
				_selectedCell = null;
			}

			FireShotCommand.ChangeCanExecute();
		}

		private CellViewModel _selectedCell;
	}
}