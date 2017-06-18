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
			_validate.CanAddShip(new List<Coordinate> { new Coordinate(5, 9), new Coordinate(9, 5), new Coordinate(9, 6), new Coordinate(9, 7), new Coordinate(9, 8) });
			
			Assert.That(!_validate.CanAddShip(new List<Coordinate> { new Coordinate(8, 5), new Coordinate(8, 6), new Coordinate(8, 7), new Coordinate(8, 8), new Coordinate(8, 9) }));
		}
	}
}
