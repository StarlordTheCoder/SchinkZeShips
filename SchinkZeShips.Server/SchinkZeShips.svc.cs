using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace SchinkZeShips.Server
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
	public class SchinkZeShips : ISchinkZeShips
	{
		private static List<Game> Games { get; }

		public List<Game> GetAllGames() => Games;

		public Game CreateGame(Player creator)
		{
			if(GetCurrentGame(creator) != null)
			{
				throw new FaultException("Sie sind bereits teil einer Runde");
			}

			var game = new Game
			{
				GameCreator = creator
			};

			Games.Add(game);

			return game;
		}

		public void JoinGame(Game gameToJoin, Player participant)
		{
			var gameInstance = Games.FirstOrDefault(gi => Equals(gi, gameToJoin));

			if (gameInstance == null)
			{
				throw new FaultException("Runde wurde nicht gefunden");
			}

			if (gameInstance.RunningGameState != null)
			{
				throw new FaultException("Runde wurde bereits gestartet");
			}

			if (gameInstance.GameParticipant != null)
			{
				throw new FaultException("Runde bereits voll");
			}

			if (GetCurrentGame(participant) != null)
			{
				throw new FaultException("Sie sind bereits teil einer Runde");
			}

			gameInstance.GameParticipant = participant;
		}

		public Game GetCurrentGame(Player player) => Games.FirstOrDefault(g => Equals(g.GameCreator, player) || Equals(g.GameParticipant, player));

		public void RemoveFromGame(Game game, Player player)
		{
			var gameInstance = Games.FirstOrDefault(gi => Equals(gi, game));

			if(gameInstance == null)
			{
				throw new FaultException("Runde wurde nicht gefunden");
			}

			if (gameInstance.RunningGameState != null)
			{
				throw new FaultException("Runde wurde bereits gestartet");
			}

			if (Equals(gameInstance.GameCreator, player))
			{
				gameInstance.GameCreator = gameInstance.GameParticipant;
				gameInstance.GameParticipant = null;
			}
			else if(Equals(gameInstance.GameCreator, player))
			{
				gameInstance.GameParticipant = null;
			}
			else
			{
				throw new FaultException("Spieler nicht Teil der Runde");
			}
		}
	}
}