using System;
using System.Linq;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;

namespace SchinkZeShips.Core.GameLogic
{
	public class ConfigureBoardViewModel : ViewModelBase
	{
		private const int ConfigureBoardRefreshTimeoutInMs = 5000;
		private Game _currentGame;

		public ConfigureBoardViewModel()
		{
			LockInLayoutCommand = new Command(LockInLayoutAsync);
		}

		private async void LockInLayoutAsync()
		{
			//TODO Merge layout with Server
			//TODO Send own layout to Server
			//TODO Start update timer if other wasn't
			// Device.StartTimer(TimeSpan.FromMilliseconds(ConfigureBoardRefreshTimeoutInMs), OnTimerElapsed);
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

		private bool OnViewVisible { get; set; }

		public PlayingFieldState ConfiguringBoard { get; } = new PlayingFieldState
		{
			Cells = Enumerable.Repeat(Enumerable.Repeat(new CellState(), 10).ToList(), 10).ToList()
		};

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