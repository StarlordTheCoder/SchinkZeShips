using System;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;

namespace SchinkZeShips.Core.Infrastructure
{
	public abstract class ViewModelWithCurrentGameBase : ViewModelBase
	{
		private readonly int _timeoutInMs;
		private bool _onViewVisible;
		private bool _timerRunning;

		protected ViewModelWithCurrentGameBase(int timeoutInMs)
		{
			_timeoutInMs = timeoutInMs;
		}

		public abstract Game CurrentGame { get; set; }

		private bool OnViewVisible
		{
			get { return _onViewVisible; }
			set
			{
				_onViewVisible = value;
				if (_onViewVisible && !_timerRunning)
					Device.StartTimer(TimeSpan.FromMilliseconds(_timeoutInMs), OnTimerElapsed);
			}
		}

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
			_timerRunning = true;
			if (!OnViewVisible)
			{
				_timerRunning = false;
				return false;
			}
			UpdateGamestateAsync();
			return true;
		}

		protected async void UpdateGamestateAsync()
		{
			CurrentGame = await Service.GetCurrentGame();
		}
	}
}
