using NUnit.Framework;
using SchinkZeShips.Core.GameLogic;
using SchinkZeShips.Core.SchinkZeShipsReference;
using System.Collections.Generic;

namespace SchinkZeShips.Tests.Core
{
	[TestFixture]
	public class ValidateBoardTest
	{
		private ValidateBoard _validate;
		private PlayingFieldState _board;

		[SetUp]
		public void SetUp()
		{
			_board = new PlayingFieldState();

			_validate = new ValidateBoard(_board);
		}

		private static List<List<KeyValuePair<int, int>>> _validShipPositions = new List<List<KeyValuePair<int, int>>>
		{
			new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(0, 0), new KeyValuePair<int, int>(0, 1) },
			new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(0, 9), new KeyValuePair<int, int>(1, 9) },
			new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(9, 0), new KeyValuePair<int, int>(9, 1) },
			new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(8, 9), new KeyValuePair<int, int>(9, 9) },
			new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(5, 9), new KeyValuePair<int, int>(6, 9), new KeyValuePair<int, int>(7, 9), new KeyValuePair<int, int>(8, 9), new KeyValuePair<int, int>(9, 9) },
			new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(5, 9), new KeyValuePair<int, int>(6, 0), new KeyValuePair<int, int>(7, 0), new KeyValuePair<int, int>(8, 0), new KeyValuePair<int, int>(9, 0) },
			new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(0, 0), new KeyValuePair<int, int>(0, 1), new KeyValuePair<int, int>(0, 2), new KeyValuePair<int, int>(0, 3), new KeyValuePair<int, int>(0, 5) },
			new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(9, 4), new KeyValuePair<int, int>(9, 5), new KeyValuePair<int, int>(9, 6), new KeyValuePair<int, int>(9, 7), new KeyValuePair<int, int>(9, 8) }
		};

		[Test, TestCaseSource(nameof(_validShipPositions))]
		public void CanAddShipOnEmptyBoard(List<KeyValuePair<int, int>> ship) => Assert.That(_validate.CanAddShip(ship));

		private static List<List<KeyValuePair<int, int>>> _invalidShipPositions = new List<List<KeyValuePair<int, int>>>
		{
			new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(0, 0), new KeyValuePair<int, int>(0, 1) },
			new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(7, 8), new KeyValuePair<int, int>(7, 9) },
			new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(9, 0) }
		};

		[Test, TestCaseSource(nameof(_invalidShipPositions))]
		public void CantCreateInvalideShip(List<KeyValuePair<int, int>> ship)
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
			_validate.CanAddShip(new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(5, 9), new KeyValuePair<int, int>(9, 5), new KeyValuePair<int, int>(9, 6), new KeyValuePair<int, int>(9, 7), new KeyValuePair<int, int>(9, 8) });
			
			Assert.That(!_validate.CanAddShip(new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(8, 5), new KeyValuePair<int, int>(8, 6), new KeyValuePair<int, int>(8, 7), new KeyValuePair<int, int>(8, 8), new KeyValuePair<int, int>(8, 9) }));
		}
	}
}
