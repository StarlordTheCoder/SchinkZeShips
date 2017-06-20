using System;
using System.Collections.Generic;
using System.Linq;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.GameLogic.BoardConfiguration
{
	public class BoardStateViewModel : NotifyPropertyChangedBase
	{
		private readonly Dictionary<ShipType, int> _ships = new Dictionary<ShipType, int>();
		private BoardState _model;

		public BoardState Model
		{
			get { return _model; }
			set
			{
				_model = value;

				for (var row = 0; row < 10; row++)
				{
					for (var col = 0; col < 10; col++)
					{
						Cells[row][col].Model = _model.Cells[row][col];
					}
				}

				var cellsWithShips = Cells.SelectMany(c => c).Where(c => c.Model.HasShip).ToList();

				foreach (var row in cellsWithShips.GroupBy(c => c.Coordinate.Row))
				{
					var shipParts = new List<CellViewModel>();
					Action commitShip = () =>
					{
						var ship = new Ship(shipParts.ToList(), IsCreatorBoard);

						if (!ship.ShipType.HasValue)
						{
							shipParts.Clear();
							return;
						}
						foreach (var shipPart in shipParts)
						{
							shipPart.Ship = ship;
						}

						shipParts.Clear();
					};
					foreach (var rowCell in row)
					{
						if (shipParts.Count == 0)
						{
							shipParts.Add(rowCell);
						}
						else if (shipParts.Any(s => s.Coordinate.Column == rowCell.Coordinate.Column + 1 || s.Coordinate.Column == rowCell.Coordinate.Column - 1))
						{
							shipParts.Add(rowCell);
						}
						else
						{
							commitShip();
							shipParts.Add(rowCell);
						}
					}
					commitShip();
				}

				foreach (var col in cellsWithShips.GroupBy(c => c.Coordinate.Column))
				{
					var shipParts = new List<CellViewModel>();
					Action commitShip = () =>
					{
						var ship = new Ship(shipParts.ToList(), IsCreatorBoard);

						if (!ship.ShipType.HasValue)
						{
							shipParts.Clear();
							return;
						}
						foreach (var shipPart in shipParts)
						{
							shipPart.Ship = ship;
						}

						shipParts.Clear();
					};

					foreach (var colCell in col)
					{
						if (shipParts.Count == 0)
						{
							shipParts.Add(colCell);
						}
						else if (shipParts.Any(s => s.Coordinate.Row == colCell.Coordinate.Row + 1 || s.Coordinate.Row == colCell.Coordinate.Row - 1))
						{
							shipParts.Add(colCell);
						}
						else
						{
							commitShip();
							shipParts.Add(colCell);
						}
					}
					commitShip();
				}

				OnPropertyChanged();
			}
		}

		public int AllowedSubmarines => _ships[ShipType.Submarine];
		public int AllowedDestroyer => _ships[ShipType.Destroyer];
		public int AllowedCruisers => _ships[ShipType.Cruiser];
		public int AllowedBattleships => _ships[ShipType.Battleship];

		public BoardStateViewModel(BoardState board, bool isCreatorBoard)
		{
			for (var row = 0; row < 10; row++)
			{
				var list = new List<CellViewModel>();

				for (var col = 0; col < 10; col++)
				{
					var coordinate = new Coordinate(row, col);
					list.Add(new CellViewModel(coordinate));
				}

				Cells.Add(list);
			}

			IsCreatorBoard = isCreatorBoard;
			Model = board;

			_ships.Add(ShipType.Submarine, 4);
			_ships.Add(ShipType.Destroyer, 3);
			_ships.Add(ShipType.Cruiser, 2);
			_ships.Add(ShipType.Battleship, 1);
		}

		public List<List<CellViewModel>> Cells { get; } = new List<List<CellViewModel>>();

		public bool IsCreatorBoard { get; }

		public bool TryAddShip(Ship shipToAdd)
		{
			var addShipAllowed = CanAddShip(shipToAdd);

			if (!addShipAllowed) return false;

			foreach (var ship in shipToAdd.ShipParts)
			{
				ship.Model.HasShip = true;
				ship.Ship = shipToAdd;
			}


			_ships[shipToAdd.ShipType.Value]--;
			OnPropertyChanged(nameof(AllowedSubmarines));
			OnPropertyChanged(nameof(AllowedDestroyer));
			OnPropertyChanged(nameof(AllowedCruisers));
			OnPropertyChanged(nameof(AllowedBattleships));

			return true;
		}

		public bool CanAddShip(Ship ship)
		{
			if (!ship.ShipType.HasValue) return false;

			// The area around the to be placed ship has to be empty
			var first = ship.ShipParts.First().Coordinate;

			var topLeft = new Coordinate(first.Row == 0 ? first.Row : first.Row - 1, first.Column == 0 ? first.Column : first.Column - 1);

			var last = ship.ShipParts.Last().Coordinate;

			var bottomRight = new Coordinate(last.Row == 9 ? last.Row : last.Row + 1, last.Column == 9 ? last.Column : last.Column + 1);

			for (var row = topLeft.Row; row <= bottomRight.Row; row++)
			{
				for (var column = topLeft.Column; column <= bottomRight.Column; column++)
				{
					if (Model.Cells[row][column].HasShip)
					{
						return false;
					}
				}
			}

			// The Ship also has to be available
			var allowedAmountOfShips = _ships[ship.ShipType.Value];

			return allowedAmountOfShips > 0;
		}
	}
}

