using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SchinkZeShips.Core.Infrastructure;
using Xamarin.UITest;
using Xamarin.UITest.Configuration;

namespace SchinkZeShips.UITests
{
	[Explicit]
	[TestFixture(Platform.Android)]
	public class Tests
	{
		[SetUp]
		public async Task BeforeEachTest()
		{
			//Leave any game if we're part of one
			_server = new GameLogicService();

			var currentGame = await _server.GetCurrentGame();

			if (currentGame != null)
			{
				await _server.RemoveFromGame(currentGame.Id, Settings.UiTestsGuid);
			}

			_app = AppInitializer.StartApp(_platform);
		}

		private IApp _app;
		private readonly Platform _platform;
		private GameLogicService _server;

		public Tests(Platform platform)
		{
			_platform = platform;
		}

		[Test]
		public async Task UsernameSavedAfterRestart()
		{
			_app.WaitForElement("UsernameEntry", "Failed to start app", TimeSpan.FromMinutes(1));

			_app.ClearText("UsernameEntry");

			var randomUsername = Guid.NewGuid().ToString();

			_app.EnterText(randomUsername);

			_app.DismissKeyboard();

			// Restart app
			_app = AppInitializer.StartApp(_platform, AppDataMode.DoNotClear);

			_app.WaitForElement("UsernameEntry", "Failed to start app second time", TimeSpan.FromMinutes(1));

			_app.WaitForElement("UsernameEntry", "Failed to start app second time", TimeSpan.FromMinutes(1));

			_app.WaitForElement(e => e.Marked("UsernameEntry").Text(randomUsername), "Didn't find correct username", TimeSpan.FromMinutes(1));

			_app.Back();
		}
	}
}