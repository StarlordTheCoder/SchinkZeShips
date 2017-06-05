using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.Infrastructure
{
	public class GameLogicService
	{
		private readonly SchinkZeShipsClient _client;

		public GameLogicService()
		{
			_client = new SchinkZeShipsClient(SchinkZeShipsClient.EndpointConfiguration.BasicHttpsBinding_ISchinkZeShips);
		}

		public async Task<List<Game>> GetAllGames()
		{
			var loadAllGames = new TaskCompletionSource<List<Game>>();

			EventHandler<GetAllOpenGamesCompletedEventArgs> loadAllGamesHandler = null;
			loadAllGamesHandler = (sender, args) =>
			{
				_client.GetAllOpenGamesCompleted -= loadAllGamesHandler;
				if (args.Error != null)
					loadAllGames.SetException(args.Error);
				else
					loadAllGames.SetResult(args.Result);
			};

			_client.GetAllOpenGamesCompleted += loadAllGamesHandler;
			_client.GetAllOpenGamesAsync();

			return await loadAllGames.Task;
		}

		public async Task<Game> GetCurrentGame()
		{
			var getCurrentGame = new TaskCompletionSource<Game>();

			EventHandler<GetCurrentGameCompletedEventArgs> getCurrentGameHandler = null;
			getCurrentGameHandler = (sender, args) =>
			{
				_client.GetCurrentGameCompleted -= getCurrentGameHandler;
				if (args.Error != null)
					getCurrentGame.SetException(args.Error);
				else
					getCurrentGame.SetResult(args.Result);
			};

			_client.GetCurrentGameCompleted += getCurrentGameHandler;
			_client.GetCurrentGameAsync(Settings.Instance.UserId);

			return await getCurrentGame.Task;
		}

		public async Task<Game> CreateGame(string gameName)
		{
			var loadAllGames = new TaskCompletionSource<Game>();

			EventHandler<CreateGameCompletedEventArgs> createGameHandler = null;
			createGameHandler = (sender, args) =>
			{
				_client.CreateGameCompleted -= createGameHandler;
				if (args.Error != null)
					loadAllGames.SetException(args.Error);
				else
					loadAllGames.SetResult(args.Result);
			};

			_client.CreateGameCompleted += createGameHandler;
			_client.CreateGameAsync(new Player
			{
				Id = Settings.Instance.UserId,
				Username = Settings.Instance.Username
			}, gameName);

			return await loadAllGames.Task;
		}

		public async Task JoinGame(string gameId)
		{
			var joinGame = new TaskCompletionSource<object>();

			EventHandler<AsyncCompletedEventArgs> joinGameHandler = null;
			joinGameHandler = (sender, args) =>
			{
				_client.JoinGameCompleted -= joinGameHandler;
				if (args.Error != null)
					joinGame.SetException(args.Error);
				else
					joinGame.SetResult(null);
			};

			_client.JoinGameCompleted += joinGameHandler;
			_client.JoinGameAsync(gameId, new Player
			{
				Id = Settings.Instance.UserId,
				Username = Settings.Instance.Username
			});

			await joinGame.Task;
		}

		public async Task RemoveFromGame(string gameId, string playerId)
		{
			var removeFromGame = new TaskCompletionSource<object>();

			EventHandler<AsyncCompletedEventArgs> removeFromGameHandler = null;
			removeFromGameHandler = (sender, args) =>
			{
				_client.RemoveFromGameCompleted -= removeFromGameHandler;
				if (args.Error != null)
					removeFromGame.SetException(args.Error);
				else
					removeFromGame.SetResult(null);
			};

			_client.RemoveFromGameCompleted += removeFromGameHandler;
			_client.RemoveFromGameAsync(gameId, playerId);

			await removeFromGame.Task;
		}
	}
}