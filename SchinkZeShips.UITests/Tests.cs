using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SchinkZeShips.Core;
using Xamarin.UITest;

namespace SchinkZeShips.UITests
{
	[TestFixture(Platform.Android)]
	public class Tests
	{
		private IApp _app;
		private readonly Platform _platform;

		public Tests(Platform platform)
		{
			_platform = platform;
		}

		[SetUp]
		public void BeforeEachTest()
		{
			_app = AppInitializer.StartApp(_platform);
		}

		[Test]
		public async Task AppLaunches()
		{
			var server = new GameLogicService();

			_app.WaitForElement("GameCountLabel", "Failed to start app", TimeSpan.FromMinutes(1));

			var games = await server.GetAllGames();

			_app.WaitForElement(e => e.Marked("GameCountLabel").Text($"Game count: {games.Count}"));

			_app.Back();
		}
	}
}

