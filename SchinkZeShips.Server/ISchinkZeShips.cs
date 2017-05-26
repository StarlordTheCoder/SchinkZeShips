using System.Collections.Generic;
using System.ServiceModel;

namespace SchinkZeShips.Server
{
	[ServiceContract]
	public interface ISchinkZeShips
	{
		[OperationContract]
		List<Game> GetAllGames();

		[OperationContract]
		Game CreateGame(Player creator);

		[OperationContract]
		Game GetCurrentGame(Player player);

		[OperationContract]
		void JoinGame(Game gameToJoin, Player participant);

		[OperationContract]
		void RemoveFromGame(Game game, Player player);
	}
}