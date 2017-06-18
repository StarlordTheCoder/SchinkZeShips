using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SchinkZeShips.Server
{
	[DataContract]
	public class BoardState
	{
		[DataMember]
		public List<List<CellState>> Cells { get; set; } = new List<List<CellState>>();

		public BoardState()
		{
			for (var i = 0; i < 10; i++)
			{
				var list = new List<CellState>();

				for (var j = 0; j < 10; j++)
				{
					list.Add(new CellState());
				}

				Cells.Add(list);
			}
		}

		public override bool Equals(object obj)
		{
			var other = obj as BoardState;

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