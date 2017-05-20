using System.Collections.Generic;
using System.ServiceModel;
using SchinkZeShips.Core;

namespace SchinkZeShips.Server
{
	[ServiceContract]
	public interface ISchinkZeShip
	{
		[OperationContract]
		List<Game> GetAllGames();

		[OperationContract]
		Game CreateGame(Player creator);

		[OperationContract]
		void JoinGame(Game gameToJoin, Player participant);
	}
}
