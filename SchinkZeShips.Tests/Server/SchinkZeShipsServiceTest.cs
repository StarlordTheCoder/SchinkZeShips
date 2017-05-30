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

			var game = _service.CreateGame(creator);
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

			var game = _service.CreateGame(creator);
			_service.JoinGame(game.Id, participant);
			game.GameParticipant = participant;

			// Act & Assert
			Assert.That(() => _service.CreateGame(creator), Throws.Exception);
			Assert.That(() => _service.CreateGame(participant), Throws.Exception);

			var newGame = _service.CreateGame(new Player());
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

			var game = _service.CreateGame(originalCreator);
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

			var game = _service.CreateGame(creator);

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

			var game = _service.CreateGame(creator);
			var game2 = new Game();

			// Act & Assert
			var currentGame = _service.GetCurrentGame(creator.Id);
			currentGame.RunningGameState = new GameState();
			currentGame.RunningGameState.PlayingFieldCreator = new PlayingFieldState();
			currentGame.RunningGameState.PlayingFieldCreator.Cells[0] = new CellState();
			currentGame.RunningGameState.PlayingFieldCreator.Cells[0].HasShip = true;

			_service.UpdateCurrentGame(currentGame.Id, currentGame.RunningGameState);
			Assert.That(_service.GetCurrentGame(creator.Id).RunningGameState.PlayingFieldCreator.Cells[0].HasShip, Is.EqualTo(true));

			Assert.That(() => _service.UpdateCurrentGame(game2.Id, game2.RunningGameState), Throws.Exception);
		}

	}
}
