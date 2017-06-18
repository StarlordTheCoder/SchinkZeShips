using System;
using System.Collections.Generic;
using System.Linq;
using SchinkZeShips.Core.ExtensionMethods;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;

namespace SchinkZeShips.Core.GameLogic.BoardConfiguration
{
	public class ConfigureBoardViewModel : ViewModelBase
	{
		private Game _currentGame;
		public IBoardStateViewModel BoardStateViewModel { get; }

		public ConfigureBoardViewModel()
		{
			LockInLayoutCommand = new Command(LockInLayoutAsync);
			BoardStateViewModel = new BoardStateViewModel(ConfiguringBoard);
		}

		private CellState _firstClickedCell;

		private void CellGotSelected(object sender, EventArgs eventArgs)
		{
			var clickedCell = (CellState) sender;

			if (_firstClickedCell == null)
			{
				_firstClickedCell = clickedCell;
			}
			else
			{
				var firstCoordinate = GetCoordinateFor(_firstClickedCell);
				var secondCoordinate = GetCoordinateFor(clickedCell);

				_firstClickedCell.IsSelected = false;
				clickedCell.IsSelected = false;

				_firstClickedCell = null;

				if (firstCoordinate == null || secondCoordinate == null)
				{
					throw new Exception("You clicked cells which are outside of the field. This should never happen");
				}

				var shipToBePlaced = new List<Coordinate>();

				if (firstCoordinate.Row == secondCoordinate.Row)
				{
					int biggerColumn;
					int smallerColumn;
					if (firstCoordinate.Column > secondCoordinate.Column)
					{
						biggerColumn = firstCoordinate.Column;
						smallerColumn = secondCoordinate.Column;
					}
					else
					{
						smallerColumn = firstCoordinate.Column;
						biggerColumn = secondCoordinate.Column;
					}

					for (var column = smallerColumn; column <= biggerColumn; column++)
					{
						shipToBePlaced.Add(new Coordinate(firstCoordinate.Row, column));
					}
				}
				else if (firstCoordinate.Column == secondCoordinate.Column)
				{
					int biggerRow;
					int smallerRow;
					if (firstCoordinate.Row > secondCoordinate.Row)
					{
						biggerRow = firstCoordinate.Row;
						smallerRow = secondCoordinate.Row;
					}
					else
					{
						smallerRow = firstCoordinate.Row;
						biggerRow = secondCoordinate.Row;
					}

					for (var row = smallerRow; row <= biggerRow; row++)
					{
						shipToBePlaced.Add(new Coordinate(row, firstCoordinate.Column));
					}
				}
				else
				{
					Dialogs.Alert("Das Schiff darf nicht diagonal platziert werden", "Platzieren fehlgeschlagen");
					return;
				}

				var addedShip = BoardStateViewModel.TryAddShip(shipToBePlaced);

				if (!addedShip)
				{
					Dialogs.Alert("Das Schiff kann dort nicht platziert werden", "Platzieren fehlgeschlagen");
				}
			}
		}

		private Coordinate GetCoordinateFor(CellState cell)
		{
			var rowsWithIndex = ConfiguringBoard.Cells.Select((value, index) => new {value, index});
			foreach (var row in rowsWithIndex)
			{
				var column = row.value.Select((value, index) => new { value, index }).FirstOrDefault(c => c.value.Equals(cell));
				if (column != null)
				{
					return new Coordinate(row.index, column.index);
				}
			}

			return null;
		}

		public override void OnAppearing()
		{
			base.OnAppearing();

			foreach (var cell in ConfiguringBoard.Cells.SelectMany(c => c))
			{
				cell.GotSelected += CellGotSelected;
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();

			foreach (var cell in ConfiguringBoard.Cells.SelectMany(c => c))
			{
				cell.GotSelected -= CellGotSelected;
			}
		}

		private async void LockInLayoutAsync()
		{
			var latestGameState = await Service.GetCurrentGame();

			if (latestGameState.CurrentPlayerIsLobbyCreator())
			{
				latestGameState.RunningGameState.PlayingFieldCreator = ConfiguringBoard;
			}
			else
			{
				latestGameState.RunningGameState.PlayingFieldParticipant = ConfiguringBoard;
			}

			await Service.UpdateGameState(latestGameState.Id, latestGameState.RunningGameState);

			PushViewModal(new InGame.InGameView(latestGameState));
		}

		public Command LockInLayoutCommand { get; }

		public Game CurrentGame
		{
			get { return _currentGame; }
			set
			{
				_currentGame = value;
				OnPropertyChanged();
			}
		}

		public PlayingFieldState ConfiguringBoard { get; } = new PlayingFieldState();
	}
}