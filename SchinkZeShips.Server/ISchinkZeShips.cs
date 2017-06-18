using System.Collections.Generic;
using System.ServiceModel;

namespace SchinkZeShips.Server
{
	[ServiceContract]
	public interface ISchinkZeShips
	{
		/// <summary>
		///     Returns all games which are search for a participator
		/// </summary>
		/// <returns>Games without a participator</returns>
		[OperationContract]
		List<Game> GetAllOpenGames();

		/// <summary>
		///     Creates a new game
		/// </summary>
		/// <param name="creator">Player which created the game</param>
		/// <param name="gameName">Name of the game which is shown to the other players</param>
		/// <returns>The created game</returns>
		[OperationContract]
		Game CreateGame(Player creator, string gameName);

		/// <summary>
		///     Returns the game of which the player is part of
		/// </summary>
		/// <param name="playerId">The player to look for</param>
		/// <returns>The current game or null if no game was found</returns>
		[OperationContract]
		Game GetCurrentGame(string playerId);

		/// <summary>
		///     Joins a game if there is no participator. Throws an exception if the game is already filled or the provided player
		///     is already part of a game
		/// </summary>
		/// <param name="gameIdToJoin">Game to join</param>
		/// <param name="player">Player which wants to join</param>
		[OperationContract]
		void JoinGame(string gameIdToJoin, Player player);

		/// <summary>
		///     Removes someone from the game, either the creator or the participant.
		///     Of the creator is removed and there is a participant, the participant gets promoted to the creator
		///     If the creator is removed and there is no participant, the game gets destroyed
		///     Throws an exception if either game or player couldn't be found
		/// </summary>
		/// <param name="gameId"></param>
		/// <param name="playerId">The player to remove</param>
		[OperationContract]
		void RemoveFromGame(string gameId, string playerId);

		/// <summary>
		///     Updates a game state. Is used for configuring the board and later playing the game
		/// </summary>
		/// <param name="gameId">Game to update</param>
		/// <param name="gameState">New game state</param>
		[OperationContract]
		void UpdateGameState(string gameId, GameState gameState);
	}
}