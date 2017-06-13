using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.GameLogic
{
	public class InGameViewModel : ViewModelBase
	{
		private Game _currentGame;

		public Game CurrentGame
		{
			get { return _currentGame; }
			set
			{
				_currentGame = value;
				OnPropertyChanged();
			}
		}

		private bool OnViewVisible { get; set; }

		public override void OnAppearing()
		{
			base.OnAppearing();
			OnViewVisible = true;
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			OnViewVisible = false;
		}

		private bool OnTimerElapsed()
		{
			if (!OnViewVisible)
			{
				return false;
			}
			UpdateGamestateAsync();
			return true;
		}

		private async void UpdateGamestateAsync()
		{
			var ownGame = await Service.GetCurrentGame();

			if (ownGame.RunningGameState.PlayingFieldCreator != null && ownGame.RunningGameState.PlayingFieldParticipant != null)
			{
				//TODO Push InGame View
				return;
			}

			CurrentGame = ownGame;
		}
	}
}