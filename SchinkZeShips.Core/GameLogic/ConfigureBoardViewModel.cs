using System.Linq;
using SchinkZeShips.Core.ExtensionMethods;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;

namespace SchinkZeShips.Core.GameLogic
{
	public class ConfigureBoardViewModel : ViewModelBase
	{
		private Game _currentGame;

		public ConfigureBoardViewModel()
		{
			LockInLayoutCommand = new Command(LockInLayoutAsync);
		}

		private async void LockInLayoutAsync()
		{
			var latestGameState = await Service.GetCurrentGame();

			if (latestGameState.CurrentPlayerIsLobbyCreator())
			{
				latestGameState.RunningGameState.PlayingFieldCreator = ConfiguringBoard;
			}
			else
			{
				latestGameState.RunningGameState.PlayingFieldParticipant = ConfiguringBoard;
			}

			await Service.UpdateGameState(latestGameState.Id, latestGameState.RunningGameState);

			PushViewModal(new InGameView(latestGameState));
		}

		public Command LockInLayoutCommand { get; }

		public Game CurrentGame
		{
			get { return _currentGame; }
			set
			{
				_currentGame = value;
				OnPropertyChanged();
			}
		}

		public PlayingFieldState ConfiguringBoard { get; } = new PlayingFieldState
		{
			Cells = Enumerable.Repeat(Enumerable.Repeat(new CellState(), 10).ToList(), 10).ToList()
		};
	}
}