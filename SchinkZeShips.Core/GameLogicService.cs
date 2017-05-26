﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SchinkZeShips.Core.SchinkZeShipsReference;

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

			void LoadAllGamesHandler(object sender, GetAllGamesCompletedEventArgs args)
			{
				if (args.Error != null)
					loadAllGames.SetException(args.Error);
				else
					loadAllGames.SetResult(args.Result);
			}

			_client.GetAllGamesCompleted += LoadAllGamesHandler;
			_client.GetAllGamesAsync();

			return await loadAllGames.Task;
		}
	}
}