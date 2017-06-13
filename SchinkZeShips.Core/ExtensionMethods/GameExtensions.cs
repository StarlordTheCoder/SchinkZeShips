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

		public static bool CurrentPlayerIsLobbyCreator(this Game game)
		{
			return Equals(game.GameCreator.Id, Settings.Instance.UserId);
		}
	}
}