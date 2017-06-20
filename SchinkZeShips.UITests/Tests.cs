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

			Settings.Instance.Username = Guid.NewGuid().ToString();

			await RemovePlayerFromGameAsync(Settings.UiTestsGuid);
			await RemovePlayerFromGameAsync(Settings.Instance.UserId);
		}

		private async Task RemovePlayerFromGameAsync(string playerId)
		{
			var game = await _server.GetGameForPlayer(playerId);

			if (game != null)
			{
				await _server.UpdateGameState(game.Id, null);
				await _server.RemoveFromGame(game.Id, playerId);
			}
		}

		private async Task RemoveCurrentGameAsync()
		{
			var game = await _server.GetCurrentGame();

			if (game != null)
			{
				await _server.UpdateGameState(game.Id, null);
				if(game.GameParticipant != null) await _server.RemoveFromGame(game.Id, game.GameParticipant.Id);
				await _server.RemoveFromGame(game.Id, game.GameCreator.Id);
			}
		}

		private IApp _app;
		private readonly Platform _platform;
		private GameLogicService _server;

		public Tests(Platform platform)
		{
			_platform = platform;
		}

		[Test]
		public void UsernameSavedAfterRestart()
		{
			_app = AppInitializer.StartApp(_platform);

			_app.WaitForElement("UsernameEntry", "Failed to start app", TimeSpan.FromSeconds(10));

			_app.ClearText("UsernameEntry");

			var randomUsername = Guid.NewGuid().ToString();

			_app.EnterText(randomUsername);

			_app.DismissKeyboard();

			// Restart app
			_app = AppInitializer.StartApp(_platform, AppDataMode.DoNotClear);

			_app.WaitForElement("UsernameEntry", "Failed to start app second time", TimeSpan.FromSeconds(10));

			_app.WaitForElement(e => e.Marked("UsernameEntry").Text(randomUsername), "Didn't find correct username", TimeSpan.FromSeconds(1));
		}

		[Test]
		public async Task CanFindAndJoinExistingLobby()
		{
			var randomGameName = Guid.NewGuid().ToString();
			await _server.CreateGame(randomGameName);

			_app = AppInitializer.StartApp(_platform);

			_app.WaitForElement("UsernameEntry", "Failed to start app", TimeSpan.FromSeconds(10));

			var randomUsername = Guid.NewGuid().ToString();
			_app.ClearText("UsernameEntry");
			_app.EnterText("UsernameEntry", randomUsername);
			_app.DismissKeyboard();

			_app.Tap("SearchGameButton");

			_app.WaitForElement("GameFilterSearchBar", "Failed to find search bar", TimeSpan.FromSeconds(20));

			_app.EnterText("GameFilterSearchBar", randomGameName);
			_app.DismissKeyboard();

			_app.WaitForElement(e => e.Text(randomGameName), "Failed to find previously created game by name", TimeSpan.FromSeconds(10));

			_app.ClearText("GameFilterSearchBar");
			_app.EnterText("GameFilterSearchBar", Settings.Instance.Username);
			_app.DismissKeyboard();

			_app.WaitForElement(e => e.Text(randomGameName), "Failed to find previously created game by creator", TimeSpan.FromSeconds(10));

			_app.Tap(e => e.Text(randomGameName));

			_app.WaitForElement(e => e.Marked("GameCreatorLabel").Text(Settings.Instance.Username));

			_app.WaitForElement(e => e.Marked("GameParticipantLabel").Text(randomUsername));

			await RemoveCurrentGameAsync();
		}
	}
}