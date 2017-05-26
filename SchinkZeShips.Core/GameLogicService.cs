using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchinkZeShips.Core.Connected_Services.SchinkZeShipsReference;

namespace SchinkZeShips.Core
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
	}
}