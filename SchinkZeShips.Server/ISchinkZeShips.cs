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
		void JoinGame(Game gameToJoin, Player participant);
	}
}
