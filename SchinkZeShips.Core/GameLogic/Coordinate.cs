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


		private bool Equals(Coordinate other)
		{
			return Row == other.Row && Column == other.Column;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (!(obj is Coordinate)) return false;
			return Equals((Coordinate) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Row * 397) ^ Column;
			}
		}

		public override string ToString()
		{
			return $"{Row}, {Column}";
		}
	}
}
