using NUnit.Framework;
using SchinkZeShips.Core.GameLogic;
using SchinkZeShips.Core.SchinkZeShipsReference;
using System.Collections.Generic;
using SchinkZeShips.Core.GameLogic.BoardConfiguration;

namespace SchinkZeShips.Tests.Core
{
	[TestFixture]
	public class BoardStateViewModelTest
	{
		private BoardStateViewModel _validate;
		private PlayingFieldState _board;

		[SetUp]
		public void SetUp()
		{
			_board = new PlayingFieldState();

			_validate = new BoardStateViewModel(_board);
		}

		private static List<List<Coordinate>> _validShipPositions = new List<List<Coordinate>>
		{
			new List<Coordinate> { new Coordinate(0, 0), new Coordinate(0, 1) },
			new List<Coordinate> { new Coordinate(0, 9), new Coordinate(1, 9) },
			new List<Coordinate> { new Coordinate(9, 0), new Coordinate(9, 1) },
			new List<Coordinate> { new Coordinate(8, 9), new Coordinate(9, 9) },
			new List<Coordinate> { new Coordinate(5, 9), new Coordinate(6, 9), new Coordinate(7, 9), new Coordinate(8, 9), new Coordinate(9, 9) },
			new List<Coordinate> { new Coordinate(5, 9), new Coordinate(6, 0), new Coordinate(7, 0), new Coordinate(8, 0), new Coordinate(9, 0) },
			new List<Coordinate> { new Coordinate(0, 0), new Coordinate(0, 1), new Coordinate(0, 2), new Coordinate(0, 3), new Coordinate(0, 5) },
			new List<Coordinate> { new Coordinate(9, 4), new Coordinate(9, 5), new Coordinate(9, 6), new Coordinate(9, 7), new Coordinate(9, 8) }
		};

		[TestCaseSource(nameof(_validShipPositions))]
		public void CanAddShipOnEmptyBoard(List<Coordinate> ship) => Assert.That(_validate.CanAddShip(ship));

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
			_board.Cells[0][0] .HasShip = true;
			_board.Cells[7][8].HasShip = true;


			// Act & Assert
			Assert.That(!_validate.CanAddShip(ship));
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
			var addFirstShip = _validate.TryAddShip(battleShip);
			
			Assert.That(addFirstShip, Is.True);
			Assert.That(_validate.AllowedBattleships, Is.EqualTo(0));

			Assert.That(_board.Cells[9][4].HasShip);
			Assert.That(_board.Cells[9][5].HasShip);
			Assert.That(_board.Cells[9][6].HasShip);
			Assert.That(_board.Cells[9][7].HasShip);
			Assert.That(_board.Cells[9][8].HasShip);

			// Assert second ship isn't added
			var canAddSecondBattleship = _validate.TryAddShip(new List<Coordinate>
				{
					new Coordinate(5, 5),
					new Coordinate(5, 6),
					new Coordinate(5, 7),
					new Coordinate(5, 8),
					new Coordinate(5, 9)
				});
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
			var addFirstShip = _validate.TryAddShip(battleShip);

			Assert.That(addFirstShip, Is.True);
			Assert.That(_validate.AllowedBattleships, Is.EqualTo(0));

			Assert.That(_board.Cells[9][4].HasShip);
			Assert.That(_board.Cells[9][5].HasShip);
			Assert.That(_board.Cells[9][6].HasShip);
			Assert.That(_board.Cells[9][7].HasShip);
			Assert.That(_board.Cells[9][8].HasShip);

			// Assert second ship isn't added
			var canAddsubmarine = _validate.TryAddShip(new List<Coordinate>
			{
				new Coordinate(8, 6),
				new Coordinate(8, 7)
			});
			Assert.That(canAddsubmarine, Is.False);
			Assert.That(_validate.AllowedSubmarines, Is.EqualTo(4));
			Assert.That(_board.Cells[8][6].HasShip, Is.False);
			Assert.That(_board.Cells[8][7].HasShip, Is.False);
		}
	}
}
