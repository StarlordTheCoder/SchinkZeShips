using System.Linq;
using System.Runtime.Serialization;

namespace SchinkZeShips.Server
{
	[DataContract]
	public class PlayingFieldState
	{
		[DataMember]
		public CellState[][] Cells { get; set; } = Enumerable.Repeat(Enumerable.Repeat(new CellState(), 10).ToArray(), 10).ToArray();

		public override bool Equals(object obj)
		{
			var other = obj as PlayingFieldState;

			return other != null &&
				other.Cells.SequenceEqual(Cells);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = 0;

				foreach (var cell in Cells)
				{
					result += cell.GetHashCode();
					result ^= cell.GetHashCode();
				}

				return result;
			}
		}
	}
}