using System;
using NUnit.Framework;
using SchinkZeShips.Core.GameLogic;
using SchinkZeShips.Core.SchinkZeShipsReference;
using System.Collections.Generic;
using System.Linq;
using SchinkZeShips.Core.GameLogic.BoardConfiguration;

namespace SchinkZeShips.Tests.Core
{
	[TestFixture]
	public class BoardStateViewModelTest
	{
		private BoardStateViewModel _validate;
		private BoardState _board;

		[SetUp]
		public void SetUp()
		{
			_board = new BoardState();

			_validate = new BoardStateViewModel(_board, true);
		}

		private static List<Coordinate[]> _validShipPositions = new List<Coordinate[]>
		{
			new [] { new Coordinate(0, 0), new Coordinate(0, 1) },
			new [] { new Coordinate(0, 9), new Coordinate(1, 9) },
			new [] { new Coordinate(9, 0), new Coordinate(9, 1) },
			new [] { new Coordinate(8, 9), new Coordinate(9, 9) },
			new [] { new Coordinate(5, 9), new Coordinate(6, 9), new Coordinate(7, 9), new Coordinate(8, 9), new Coordinate(9, 9) },
			new [] { new Coordinate(5, 9), new Coordinate(6, 0), new Coordinate(7, 0), new Coordinate(8, 0), new Coordinate(9, 0) },
			new [] { new Coordinate(0, 0), new Coordinate(0, 1), new Coordinate(0, 2), new Coordinate(0, 3), new Coordinate(0, 5) },
			new [] { new Coordinate(9, 4), new Coordinate(9, 5), new Coordinate(9, 6), new Coordinate(9, 7), new Coordinate(9, 8) }
		};

		[TestCaseSource(nameof(_validShipPositions))]
		public void CanAddShipOnEmptyBoard(Coordinate[] ship) => Assert.That(_validate.CanAddShip(CoordinatesToShip(ship)));

		private Ship CoordinatesToShip(IEnumerable<Coordinate> ship)
		{
			return new Ship(ship.Select(s => _validate.Cells[s.Row][s.Column]).ToList(), true, Guid.NewGuid().ToString());
		}

		private static List<List<Coordinate>> _invalidShipPositions = new List<List<Coordinate>>
		{
			new List<Coordinate> { new Coordinate(0, 0), new Coordinate(0, 1) },
			new List<Coordinate> { new Coordinate(7, 8), new Coordinate(7, 9) },
			new List<Coordinate> { new Coordinate(9, 0) }
		};

		[TestCaseSource(nameof(_invalidShipPositions))]
		public void CantCreateInvalideShip(List<Coordinate> ship)
		{
			//Arrange
			_board.Cells[0][0].ShipId = "Tolles Schiff";
			_board.Cells[7][8].ShipId = "Tolles Schiff";


			// Act & Assert
			Assert.That(!_validate.CanAddShip(CoordinatesToShip(ship)));
		}

		[Test]
		public void CantCreateTooManyShipsOfTheSameType()
		{
			// Act & Assert
			Assert.That(_validate.AllowedBattleships, Is.EqualTo(1));

			var battleShip = new List<Coordinate>
			{
				new Coordinate(9, 4),
				new Coordinate(9, 5),
				new Coordinate(9, 6),
				new Coordinate(9, 7),
				new Coordinate(9, 8)
			};

			// Assert first ship is properly added
			var addFirstShip = _validate.TryAddShip(CoordinatesToShip(battleShip));
			
			Assert.That(addFirstShip, Is.True);
			Assert.That(_validate.AllowedBattleships, Is.EqualTo(0));

			Assert.That(_board.Cells[9][4].HasShip);
			Assert.That(_board.Cells[9][5].HasShip);
			Assert.That(_board.Cells[9][6].HasShip);
			Assert.That(_board.Cells[9][7].HasShip);
			Assert.That(_board.Cells[9][8].HasShip);

			// Assert second ship isn't added
			var canAddSecondBattleship = _validate.TryAddShip(CoordinatesToShip(new List<Coordinate>
				{
					new Coordinate(5, 5),
					new Coordinate(5, 6),
					new Coordinate(5, 7),
					new Coordinate(5, 8),
					new Coordinate(5, 9)
				}));
			Assert.That(canAddSecondBattleship, Is.False);
			Assert.That(_validate.AllowedBattleships, Is.EqualTo(0));
			Assert.That(_board.Cells[5][5].HasShip, Is.False);
			Assert.That(_board.Cells[5][6].HasShip, Is.False);
			Assert.That(_board.Cells[5][7].HasShip, Is.False);
			Assert.That(_board.Cells[5][8].HasShip, Is.False);
			Assert.That(_board.Cells[5][9].HasShip, Is.False);
		}

		[Test]
		public void CantCreateShipsWhichAreTooClose()
		{
			// Act & Assert
			Assert.That(_validate.AllowedBattleships, Is.EqualTo(1));
			Assert.That(_validate.AllowedSubmarines, Is.EqualTo(4));

			var battleShip = new List<Coordinate>
			{
				new Coordinate(9, 4),
				new Coordinate(9, 5),
				new Coordinate(9, 6),
				new Coordinate(9, 7),
				new Coordinate(9, 8)
			};

			// Assert first ship is properly added
			var addFirstShip = _validate.TryAddShip(CoordinatesToShip(battleShip));

			Assert.That(addFirstShip, Is.True);
			Assert.That(_validate.AllowedBattleships, Is.EqualTo(0));

			Assert.That(_board.Cells[9][4].HasShip);
			Assert.That(_board.Cells[9][5].HasShip);
			Assert.That(_board.Cells[9][6].HasShip);
			Assert.That(_board.Cells[9][7].HasShip);
			Assert.That(_board.Cells[9][8].HasShip);

			// Assert second ship isn't added
			var canAddsubmarine = _validate.TryAddShip(CoordinatesToShip(new List<Coordinate>
			{
				new Coordinate(8, 6),
				new Coordinate(8, 7)
			}));
			Assert.That(canAddsubmarine, Is.False);
			Assert.That(_validate.AllowedSubmarines, Is.EqualTo(4));
			Assert.That(_board.Cells[8][6].HasShip, Is.False);
			Assert.That(_board.Cells[8][7].HasShip, Is.False);
		}
	}
}
