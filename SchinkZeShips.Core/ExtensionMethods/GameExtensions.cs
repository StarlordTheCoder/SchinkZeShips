using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.ExtensionMethods
{
	public static class GameExtensions
	{
		public static bool IsInLobby(this Game game)
		{
			return game.RunningGameState == null;
		}

		public static bool IsConfiguringBoard(this Game game)
		{
			return game.RunningGameState != null &&
			       (game.RunningGameState.PlayingFieldCreator == null || game.RunningGameState.PlayingFieldParticipant == null);
		}

		public static bool IsPlaying(this Game game)
		{
			return game.RunningGameState?.PlayingFieldCreator != null && game.RunningGameState.PlayingFieldParticipant != null;
		}

		public static bool ThisPlayerIsGameCreator(this Game game)
		{
			return Equals(game.GameCreator.Id, Settings.Instance.UserId);
		}

		public static PlayingFieldState ThisPlayerBoard(this Game game)
		{
			return game.ThisPlayerIsGameCreator() ? game.RunningGameState.PlayingFieldCreator : game.RunningGameState.PlayingFieldParticipant;
		}

		public static PlayingFieldState OtherPlayerBoard(this Game game)
		{
			return game.ThisPlayerIsGameCreator() ? game.RunningGameState.PlayingFieldParticipant : game.RunningGameState.PlayingFieldCreator;
		}
	}
}