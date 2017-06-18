using System.Diagnostics;

namespace SchinkZeShips.Core.GameLogic
{
	[DebuggerDisplay("{" + nameof(Row) + "}, {" + nameof(Column) + "}")]
	public class Coordinate
	{
		public Coordinate(int row, int column)
		{
			Row = row;
			Column = column;
		}

		public int Row { get; }
		public int Column { get; }
	}
}
