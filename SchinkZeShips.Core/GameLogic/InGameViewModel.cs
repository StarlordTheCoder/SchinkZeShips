using System;
using Acr.UserDialogs;
using SchinkZeShips.Core.ExtensionMethods;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;

namespace SchinkZeShips.Core.GameLogic
{
	public class InGameViewModel : ViewModelBase
	{
		private const int InGameRefreshTimeoutInMs = 2000;
		private Game _currentGame;
		private bool _onViewVisible;
		private readonly IProgressDialog _waitingForOpponentDialog = CreateLoadingDialog(string.Empty);

		public Game CurrentGame
		{
			get { return _currentGame; }
			set
			{
				_currentGame = value;

				if (_currentGame.IsConfiguringBoard())
				{
					_waitingForOpponentDialog.Title = "Warte, bis der andere Spieler sein Feld konfiguriert";
					if (!_waitingForOpponentDialog.IsShowing) _waitingForOpponentDialog.Show();
				}
				else if (_currentGame.RunningGameState.CurrentPlayerIsGameCreator != _currentGame.CurrentPlayerIsLobbyCreator())
				{
					_waitingForOpponentDialog.Title = "Warte, bis der andere Spieler seinen Zug beendet";
					if(!_waitingForOpponentDialog.IsShowing) _waitingForOpponentDialog.Show();
				}
				else if (_waitingForOpponentDialog.IsShowing)
				{
					_waitingForOpponentDialog.Hide();
				}

				OnPropertyChanged();
			}
		}

		private bool OnViewVisible
		{
			get { return _onViewVisible; }
			set
			{
				_onViewVisible = value;
				if (_onViewVisible)
					Device.StartTimer(TimeSpan.FromMilliseconds(InGameRefreshTimeoutInMs), OnTimerElapsed);
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
			if (!OnViewVisible)
			{
				return false;
			}
			UpdateGamestateAsync();
			return true;
		}

		private async void UpdateGamestateAsync()
		{
			CurrentGame = await Service.GetCurrentGame();
		}
	}
}