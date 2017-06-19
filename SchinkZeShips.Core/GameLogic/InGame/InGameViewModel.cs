using System;
using System.Linq;
using SchinkZeShips.Core.ExtensionMethods;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;

namespace SchinkZeShips.Core.GameLogic.InGame
{
	public class InGameViewModel : ViewModelWithCurrentGameBase
	{
		private const int InGameRefreshTimeoutInMs = 3000;
		private Game _currentGame;
		public Command FireShotCommand { get; }

		public InGameViewModel() : base(InGameRefreshTimeoutInMs)
		{
			FireShotCommand = new Command(FireShotAsync, () => _selectedCell != null && CurrentGame.ThisPlayerIsGameCreator() == CurrentGame.RunningGameState.CurrentPlayerIsGameCreator);
		}

		private async void FireShotAsync()
		{
			_selectedCell.WasShot = true;

			if (!_selectedCell.HasShip)
			{
				CurrentGame.RunningGameState.CurrentPlayerIsGameCreator = !CurrentGame.RunningGameState.CurrentPlayerIsGameCreator;
			}

			await Service.UpdateGameState(CurrentGame.Id, CurrentGame.RunningGameState);

			FireShotCommand.ChangeCanExecute();

			UpdateGamestateAsync();
		}

		public override Game CurrentGame
		{
			get { return _currentGame; }
			set
			{
				if (_currentGame != null)
				{
					if (_currentGame.LatestChangeTime == value.LatestChangeTime)
					{
						// Ignore a change to the game if there were no changes
						return;
					}

					foreach (var cell in _currentGame.OtherPlayerBoard().Cells.SelectMany(c => c))
					{
						cell.GotSelected -= CellGotSelected;
					}
				}

				if (_selectedCell != null)
				{
					var selectedFieldCoordinate = _currentGame.OtherPlayerBoard().GetCoordinateFor(_selectedCell);
					if (selectedFieldCoordinate != null)
					{
						_selectedCell = value.OtherPlayerBoard().Cells[selectedFieldCoordinate.Row][selectedFieldCoordinate.Column];
						_selectedCell.IsSelected = true;
					}
					else
					{
						_selectedCell = null;
					}
				}

				foreach (var cell in value.OtherPlayerBoard().Cells.SelectMany(c => c))
				{
					cell.GotSelected += CellGotSelected;
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

		private void CellGotSelected(object sender, EventArgs e)
		{
			if (_selectedCell != null)
			{
				_selectedCell.IsSelected = false;
			}

			_selectedCell = (CellState) sender;

			if (_selectedCell.WasShot)
			{
				_selectedCell.IsSelected = false;
				_selectedCell = null;
			}

			FireShotCommand.ChangeCanExecute();
		}

		private CellState _selectedCell;
	}
}