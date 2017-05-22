using System;
using System.Collections.Generic;

namespace SchinkZeShips.Server
{
	public class SchinkZeShips : ISchinkZeShips
	{
		public List<Game> GetAllGames()
		{
			return new List<Game>();
		}

		public Game CreateGame(Player creator)
		{
			return new Game
			{
				GameCreator = creator
			};
		}

		public void JoinGame(Game gameToJoin, Player participant)
		{
			throw new NotImplementedException();
		}
	}
}