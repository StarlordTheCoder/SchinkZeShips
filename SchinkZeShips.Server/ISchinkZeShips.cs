using System.Collections.Generic;
using System.ServiceModel;

namespace SchinkZeShips.Server
{
	[ServiceContract]
	public interface ISchinkZeShips
	{
		[OperationContract]
		List<Game> GetAllOpenGames();

		[OperationContract]
		Game CreateGame(Player creator);

		[OperationContract]
		Game GetCurrentGame(string playerId);

		[OperationContract]
		void JoinGame(string gameIdToJoin, Player player);

		[OperationContract]
		void RemoveFromGame(string gameId, string playerId);

		[OperationContract]
		void UpdateCurrentGame(Game currentGame);
	}
}