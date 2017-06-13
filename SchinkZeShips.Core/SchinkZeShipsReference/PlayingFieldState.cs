using System.Collections.Generic;

namespace SchinkZeShips.Core.SchinkZeShipsReference
{
	public partial class PlayingFieldState
	{
		public PlayingFieldState()
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
	}
}