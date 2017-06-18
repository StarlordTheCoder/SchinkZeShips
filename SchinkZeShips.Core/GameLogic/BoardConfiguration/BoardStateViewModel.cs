using System;
using System.Collections.Generic;
using System.Linq;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.GameLogic.BoardConfiguration
{
	public class BoardStateViewModel : NotifyPropertyChangedBase, IBoardStateViewModel
	{
		public Dictionary<Ship, int> Ships = new Dictionary<Ship, int>();
		private readonly BoardState _board;

		public int AllowedSubmarines => Ships[Ship.Submarine];
		public int AllowedDestroyer => Ships[Ship.Destroyer];
		public int AllowedCruisers => Ships[Ship.Cruiser];
		public int AllowedBattleships => Ships[Ship.Battleship];

		public BoardStateViewModel(BoardState board)
		{
			_board = board;


			Ships.Add(Ship.Submarine, 4);
			Ships.Add(Ship.Destroyer, 3);
			Ships.Add(Ship.Cruiser, 2);
			Ships.Add(Ship.Battleship, 1);
		}

		public bool TryAddShip(List<Coordinate> shipToAdd)
		{
			var addShipAllowed = CanAddShip(shipToAdd);

			if (!addShipAllowed) return false;

			foreach (var coordinate in shipToAdd)
			{
				_board.Cells[coordinate.Row][coordinate.Column].HasShip = true;
			}


			var shipType = (Ship)shipToAdd.Count;

			Ships[shipType]--;
			OnPropertyChanged(nameof(AllowedSubmarines));
			OnPropertyChanged(nameof(AllowedDestroyer));
			OnPropertyChanged(nameof(AllowedCruisers));
			OnPropertyChanged(nameof(AllowedBattleships));

			return true;
		}

		public bool CanAddShip(List<Coordinate> ship)
		{
			// Only ships of length 2, 3, 4, 5 are valid
			var validShipType = Enum.IsDefined(typeof(Ship), ship.Count);

			if (!validShipType) return false;

			// The area around the to be placed ship has to be empty
			var first = ship.First();

			var topLeft = new Coordinate(first.Row == 0 ? first.Row : first.Row - 1, first.Column == 0 ? first.Column : first.Column - 1);

			var last = ship.Last();

			var bottomRight = new Coordinate(last.Row == 9 ? last.Row : last.Row + 1, last.Column == 9 ? last.Column : last.Column + 1);

			for (var row = topLeft.Row; row <= bottomRight.Row; row++)
			{
				for (var column = topLeft.Column; column <= bottomRight.Column; column++)
				{
					if (_board.Cells[row][column].HasShip)
					{
						return false;
					}
				}
			}

			// The Ship also has to be available
			var shipType = (Ship) ship.Count;
			var allowedAmountOfShips = Ships[shipType];

			return allowedAmountOfShips > 0;
		}
	}
}
