using System.Linq;
using SchinkZeShips.Core.GameLogic;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.ExtensionMethods
{
	public static class PlayingFieldStateExtensions
	{
		public static Coordinate GetCoordinateFor(this BoardState board, CellState cell)
		{
			var rowsWithIndex = board.Cells.Select((value, index) => new { value, index });
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
	}
}
