using System.Collections.Generic;
using System.Linq;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.GameLogic.BoardConfiguration
{
	public class BoardStateViewModel : NotifyPropertyChangedBase
	{
		public const int AmountOfCellsWithShips = 30;
		private readonly Dictionary<ShipType, int> _ships = new Dictionary<ShipType, int>();
		private BoardState _model;

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

		public BoardState Model
		{
			get => _model;
			set
			{
				_model = value;

				if (_model == null)
				{
					OnPropertyChanged();
					return;
				}

				for (var row = 0; row < 10; row++)
				for (var col = 0; col < 10; col++)
					Cells[row][col].Model = _model.Cells[row][col];

				foreach (var cellsOfShip in Cells.SelectMany(c => c).Where(c => !string.IsNullOrEmpty(c.Model.ShipId))
					.GroupBy(c => c.Model.ShipId))
				{
					var ship = new Ship(cellsOfShip.ToList(), IsCreatorBoard, cellsOfShip.Key);
					foreach (var cell in cellsOfShip)
						cell.Ship = ship;
				}

				OnPropertyChanged();
			}
		}

		public int AllowedSubmarines => _ships[ShipType.Submarine];
		public int AllowedDestroyer => _ships[ShipType.Destroyer];
		public int AllowedCruisers => _ships[ShipType.Cruiser];
		public int AllowedBattleships => _ships[ShipType.Battleship];

		public List<List<CellViewModel>> Cells { get; } = new List<List<CellViewModel>>();

		public bool IsCreatorBoard { get; }

		public bool TryAddShip(Ship shipToAdd)
		{
			var addShipAllowed = CanAddShip(shipToAdd);

			if (!addShipAllowed) return false;

			foreach (var ship in shipToAdd.ShipParts)
				ship.Ship = shipToAdd;


			_ships[shipToAdd.ShipType.Value]--;
			UpdateShipCounts();
			return true;
		}

		private void UpdateShipCounts()
		{
			OnPropertyChanged(nameof(AllowedSubmarines));
			OnPropertyChanged(nameof(AllowedDestroyer));
			OnPropertyChanged(nameof(AllowedCruisers));
			OnPropertyChanged(nameof(AllowedBattleships));
		}

		public bool CanAddShip(Ship ship)
		{
			if (!ship.ShipType.HasValue) return false;

			// The area around the to be placed ship has to be empty
			var first = ship.ShipParts.First().Coordinate;

			var topLeft = new Coordinate(first.Row == 0 ? first.Row : first.Row - 1,
				first.Column == 0 ? first.Column : first.Column - 1);

			var last = ship.ShipParts.Last().Coordinate;

			var bottomRight = new Coordinate(last.Row == 9 ? last.Row : last.Row + 1,
				last.Column == 9 ? last.Column : last.Column + 1);

			for (var row = topLeft.Row; row <= bottomRight.Row; row++)
			for (var column = topLeft.Column; column <= bottomRight.Column; column++)
				if (Model.Cells[row][column].HasShip)
					return false;

			// The Ship also has to be available
			var allowedAmountOfShips = _ships[ship.ShipType.Value];

			return allowedAmountOfShips > 0;
		}

		public void RemoveShip(Ship shipToRemove)
		{
			_ships[shipToRemove.ShipType.Value]++;

			foreach (var shipPart in shipToRemove.ShipParts)
				shipPart.Ship = null;
		}
	}
}