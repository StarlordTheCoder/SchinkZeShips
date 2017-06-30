using System;
using System.Collections.Generic;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.GameLogic.Board
{
	public partial class ShipStatusDisplay
	{
		public ShipStatusDisplay()
		{
			InitializeComponent();

			Battleships.BindingContext = DummyShip(5);
			Cruisers.BindingContext = DummyShip(4);
			Destroyer.BindingContext = DummyShip(3);
			Submarines.BindingContext = DummyShip(2);
		}

		private static Ship DummyShip(int length)
		{
			var parts = new List<CellViewModel>();
			for (var i = 0; i < length; i++)
			{
				parts.Add(new CellViewModel(new Coordinate(0, i), false));
			}

			var ship = new Ship(parts, true, Guid.NewGuid().ToString());

			foreach (var part in parts)
			{
				part.Model = new CellState();
				part.Ship = ship;
			}

			return ship;
		}
	}
}