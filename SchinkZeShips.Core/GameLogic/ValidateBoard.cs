using SchinkZeShips.Core.SchinkZeShipsReference;

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
