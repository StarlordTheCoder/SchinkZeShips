﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace SchinkZeShips.Server
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
	public class SchinkZeShips : ISchinkZeShips
	{
		private List<Game> Games { get; } = new List<Game>();

		public List<Game> GetAllOpenGames() => Games.Where(g => g.GameParticipant == null).ToList();

		public Game CreateGame(Player creator)
		{
			if (GetCurrentGame(creator.Id) != null)
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

		public void JoinGame(string gameIdToJoin, Player player)
		{
			var game = Games.FirstOrDefault(g => Equals(g.Id, gameIdToJoin));

			if (game == null)
			{
				throw new FaultException("Runde wurde nicht gefunden");
			}

			if (game.RunningGameState != null)
			{
				throw new FaultException("Runde wurde bereits gestartet");
			}

			if (game.GameParticipant != null)
			{
				throw new FaultException("Runde bereits voll");
			}

			if (GetCurrentGame(player.Id) != null)
			{
				throw new FaultException("Sie sind bereits teil einer Runde");
			}

			game.GameParticipant = player;
		}

		public Game GetCurrentGame(string playerId) => Games.FirstOrDefault(g =>
			Equals(g.GameCreator.Id, playerId) ||
			(g.GameParticipant != null && Equals(g.GameParticipant.Id, playerId)));

		public void RemoveFromGame(string gameId, string playerId)
		{
			var game = Games.FirstOrDefault(g => Equals(g.Id, gameId));

			if (game == null)
			{
				throw new FaultException("Runde wurde nicht gefunden");
			}

			if (game.RunningGameState != null)
			{
				throw new FaultException("Runde wurde bereits gestartet");
			}

			if (Equals(game.GameCreator.Id, playerId))
			{
				if (game.GameParticipant != null)
				{
					game.GameCreator = game.GameParticipant;
					game.GameParticipant = null;
				}
				else
				{
					Games.Remove(game);
				}
			}
			else if (Equals(game.GameCreator.Id, playerId))
			{
				game.GameParticipant = null;
			}
			else
			{
				throw new FaultException("Spieler nicht Teil der Runde");
			}
		}

		public void UpdateCurrentGame(Game currentGame)
		{
			if (Games.FindIndex(g => Equals(g.Id, currentGame.Id)) == -1)
			{
				throw new FaultException("Spiel existiert nicht");
			}
			else
			{
				Games[Games.FindIndex(g => Equals(g.Id, currentGame.Id))] = currentGame;
			}
		}
	}
}