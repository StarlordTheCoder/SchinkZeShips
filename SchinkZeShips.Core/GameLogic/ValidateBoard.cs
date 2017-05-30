using SchinkZeShips.Core.SchinkZeShipsReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchinkZeShips.Core.GameLogic
{
	class ValidateBoard
	{

		public PlayingFieldState lastGameState;

		public PlayingFieldState validateBoard(PlayingFieldState board)
		{
			return new PlayingFieldState();
		}

		public ValidateBoard(PlayingFieldState board)
		{
			this.lastGameState = board;
		}
	}
}
