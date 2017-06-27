using SchinkZeShips.Core.GameLogic.Board;
using SchinkZeShips.Core.GameLogic.BoardConfiguration;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.ExtensionMethods
{
	public static class GameExtensions
	{
		private static BoardStateViewModel _otherPlayerBoardStateViewModel;
		private static BoardStateViewModel _thisPlayerBoardStateViewModel;

		public static bool IsInLobby(this Game game)
		{
			return game.RunningGameState == null;
		}

		public static bool IsConfiguringBoard(this Game game)
		{
			return game.RunningGameState != null &&
			       (game.RunningGameState.BoardCreator == null || game.RunningGameState.BoardParticipant == null);
		}

		public static bool IsPlaying(this Game game)
		{
			return game.RunningGameState?.BoardCreator != null && game.RunningGameState.BoardParticipant != null;
		}

		public static bool ThisPlayerIsGameCreator(this Game game)
		{
			return Equals(game.GameCreator.Id, Settings.Instance.UserId);
		}

		public static BoardStateViewModel ThisPlayerBoard(this Game game)
		{
			var board = game.ThisPlayerIsGameCreator()
				? game.RunningGameState.BoardCreator
				: game.RunningGameState.BoardParticipant;

			if (_thisPlayerBoardStateViewModel == null)
				_thisPlayerBoardStateViewModel = new BoardStateViewModel(board, true);
			else
				_thisPlayerBoardStateViewModel.Model = board;

			return _thisPlayerBoardStateViewModel;
		}

		public static BoardStateViewModel OtherPlayerBoard(this Game game)
		{
			var board = game.ThisPlayerIsGameCreator()
				? game.RunningGameState.BoardParticipant
				: game.RunningGameState.BoardCreator;

			if (_otherPlayerBoardStateViewModel == null)
				_otherPlayerBoardStateViewModel = new BoardStateViewModel(board, false);
			else
				_otherPlayerBoardStateViewModel.Model = board;

			return _otherPlayerBoardStateViewModel;
		}

		public static void ResetBoards()
		{
			_thisPlayerBoardStateViewModel = null;
			_otherPlayerBoardStateViewModel = null;
		}
	}
}