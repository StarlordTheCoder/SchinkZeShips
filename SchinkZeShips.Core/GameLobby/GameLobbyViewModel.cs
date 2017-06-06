using System;
using System.Net.Http;
using Acr.UserDialogs;
using SchinkZeShips.Core.ExtensionMethods;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;

namespace SchinkZeShips.Core.GameLobby
{
	public class GameLobbyViewModel : ViewModelBase
	{
		private const int GameLobbyRefreshTimeoutInMs = 2000;
		private Game _currentGame;
		private bool _onViewVisible;
		private bool _timerRunning;

		public GameLobbyViewModel()
		{
			StartGameCommand = new Command(StartGameAsync, () => IsLobbyLeader && HasParticipant);
			LeaveGameCommand = new Command(LeaveGameAsync);
			KickParticipantCommand = new Command(KickParticipantAsync, () => HasParticipant);
		}

		private async void KickParticipantAsync()
		{
			if (!IsLobbyLeader)
			{
				LeaveGameAsync();
				return;
			}

			var dialog = CreateLoadingDialog($"Entferne {CurrentGame.GameParticipant.Username} vom Spiel");
			dialog.Show();

			try
			{
				await Service.RemoveFromGame(CurrentGame.Id, CurrentGame.GameParticipant.Id);
			}
			catch (HttpRequestException)
			{
				UserDialogs.Instance.AlertNoConnection();
			}
			finally
			{
				dialog.Hide();
			}
		}

		private async void LeaveGameAsync()
		{
			var dialog = CreateLoadingDialog("Verlasse Spiel");
			dialog.Show();

			try
			{
				await Service.RemoveFromGame(CurrentGame.Id, Settings.Instance.UserId);

				//TODO ConfigureLayoutView
				PushView(this, new StartView());
			}
			catch (HttpRequestException)
			{
				UserDialogs.Instance.AlertNoConnection();
			}
			finally
			{
				dialog.Hide();
			}
		}

		public Game CurrentGame
		{
			get { return _currentGame; }
			set
			{
				_currentGame = value;
				OnPropertyChanged();
				StartGameCommand.ChangeCanExecute();
				KickParticipantCommand.ChangeCanExecute();
			}
		}

		private bool OnViewVisible
		{
			get { return _onViewVisible; }
			set
			{
				_onViewVisible = value;
				if (_onViewVisible && !_timerRunning)
					Device.StartTimer(TimeSpan.FromMilliseconds(GameLobbyRefreshTimeoutInMs), OnTimerElapsed);
			}
		}

		public Command StartGameCommand { get; }
		public Command LeaveGameCommand { get; }
		public Command KickParticipantCommand { get; }

		private bool IsLobbyLeader => CurrentGame != null &&
		                              Equals(CurrentGame.GameCreator.Id, Settings.Instance.UserId);

		private bool HasParticipant => CurrentGame?.GameParticipant != null;

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

		private async void UpdateGamestateAsync()
		{
			var ownGame = await Service.GetCurrentGame();

			if (ownGame == null)
			{
				PushViewModal(new StartView());
				await UserDialogs.Instance.AlertAsync("Sie sind nicht mehr Teil eines Spieles!");
				return;
			}

			if (ownGame.RunningGameState != null)
			{
				await UserDialogs.Instance.AlertAsync("Das Spiel wurde gestartet!");
				//TODO Push ConfigureLayout View
				return;
			}

			CurrentGame = ownGame;
		}

		public async void StartGameAsync()
		{
			var dialog = CreateLoadingDialog("Starte Spiel");
			dialog.Show();

			try
			{
				//TODO Update game to have RunningGameState
				//await Service.UpdateGame();

				//TODO ConfigureLayoutView
				PushView(this, new StartView());
			}
			catch (HttpRequestException)
			{
				UserDialogs.Instance.AlertNoConnection();
			}
			finally
			{
				dialog.Hide();
			}
		}
	}
}