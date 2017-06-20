using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SchinkZeShips.Server;

namespace SchinkZeShips.Tests.Server
{
	[TestFixture]
	public class SchinkZeShipsServiceTest
	{
		private ISchinkZeShips _service;

		[SetUp]
		public void SetUp()
		{
			_service = new SchinkZeShips.Server.SchinkZeShips();
		}

		[Test]
		public void NewPlayerHasNoRunningGames()
		{
			//Act & Assert
			Assert.That(_service.GetCurrentGame("RandomUserId"), Is.Null);
		}

		[Test]
		public void CanJoinCreatedGame()
		{
			//Arrange
			var creator = new Player();
			var participant = new Player();

			var game = _service.CreateGame(creator, "Foo");
			_service.JoinGame(game.Id, participant);
			game.GameParticipant = participant;

			// Act & Assert
			Assert.That(_service.GetCurrentGame(creator.Id), Is.EqualTo(game));
			Assert.That(_service.GetCurrentGame(participant.Id), Is.EqualTo(game));
		}

		[Test]
		public void CannotJoinOrCreateGameIfAlreadyInGame()
		{
			//Arrange
			var creator = new Player();
			var participant = new Player();

			var game = _service.CreateGame(creator, "Foo");
			_service.JoinGame(game.Id, participant);
			game.GameParticipant = participant;

			// Act & Assert
			Assert.That(() => _service.CreateGame(creator, "Bar"), Throws.Exception);
			Assert.That(() => _service.CreateGame(participant, "Bar"), Throws.Exception);

			var newGame = _service.CreateGame(new Player(), "Banana");
			Assert.That(() => _service.JoinGame(newGame.Id, creator), Throws.Exception);
			Assert.That(() => _service.JoinGame(newGame.Id, participant), Throws.Exception);
		}

		[Test]
		public void CannotJoinUnexistingGame()
		{
			//Arrange
			var player = new Player();

			// Act & Assert
			Assert.That(() => _service.JoinGame("Random unexisting game", player), Throws.Exception);
		}

		[Test]
		public void CanRemovePlayerFromGame()
		{
			//Arrange
			var originalCreator = new Player();
			var originalParticipant = new Player();

			var game = _service.CreateGame(originalCreator, "Foo");
			_service.JoinGame(game.Id, originalParticipant);
			game.GameParticipant = originalParticipant;

			// Act & Assert
			_service.RemoveFromGame(game.Id, originalCreator.Id);
			Assert.That(_service.GetCurrentGame(originalCreator.Id), Is.Null);

			var gameAfterRemoval = _service.GetCurrentGame(originalParticipant.Id);
			Assert.That(gameAfterRemoval, Is.Not.Null);
			Assert.That(gameAfterRemoval.GameCreator, Is.EqualTo(originalParticipant));
			Assert.That(gameAfterRemoval.GameParticipant, Is.Null);
		}

		[Test]
		public void GetAllOpenGamesIgnoresFullGames()
		{
			//Arrange
			var creator = new Player();
			var participant = new Player();

			var game = _service.CreateGame(creator, "Foo");

			// Act & Assert
			var openGamesBeforeJoin = _service.GetAllOpenGames();
			Assert.That(openGamesBeforeJoin, Has.Count.EqualTo(1));
			Assert.That(openGamesBeforeJoin, Contains.Item(game));

			_service.JoinGame(game.Id, participant);

			var openGamesAfterJoin = _service.GetAllOpenGames();
			Assert.That(openGamesAfterJoin, Is.Empty);
		}

		[Test]
		public void UpdateCurrentGame()
		{
			//Arrange
			var creator = new Player();

			_service.CreateGame(creator, "Foo");
			var game2 = new Game();

			// Act & Assert
			var currentGame = _service.GetCurrentGame(creator.Id);
			currentGame.RunningGameState = new GameState
			{
				BoardCreator = new BoardState()
			};

			var placedShip = Guid.NewGuid().ToString();
			currentGame.RunningGameState.BoardCreator.Cells[0][0].ShipId = placedShip;

			_service.UpdateGameState(currentGame.Id, currentGame.RunningGameState);
			Assert.That(_service.GetCurrentGame(creator.Id).RunningGameState.BoardCreator.Cells[0][0].ShipId, Is.EqualTo(placedShip));

			Assert.That(() => _service.UpdateGameState(game2.Id, game2.RunningGameState), Throws.Exception);
		}

		[Test]
		public async Task UpdateGameAdjustesLatestChangeDate()
		{
			//Arrange
			var creator = new Player();

			var game = _service.CreateGame(creator, "Foo");

			var previousDateTime = game.LatestChangeTime;

			Action validateLatestChangeTime = () =>
			{
				var newlyLoadedGame = _service.GetCurrentGame(creator.Id);

				Assert.That(previousDateTime, Is.LessThan(newlyLoadedGame.LatestChangeTime));
				previousDateTime = newlyLoadedGame.LatestChangeTime;
			};

			var newGameState = new GameState
			{
				BoardCreator = new BoardState()
			};
			newGameState.BoardCreator.Cells[0][0].ShipId = "Super uniqe ship ID";

			// Act & Assert

			// Wait one millisecond to ensore compatibility with fast processors

			await Task.Delay(1);

			// Add game participant
			var participant = new Player();
			_service.JoinGame(game.Id, participant);
			validateLatestChangeTime();

			await Task.Delay(1);

			// Remove game participant
			_service.RemoveFromGame(game.Id, participant.Id);
			validateLatestChangeTime();

			await Task.Delay(1);

			// Update Game state
			_service.UpdateGameState(game.Id, newGameState);
			validateLatestChangeTime();
		}

	}
}
