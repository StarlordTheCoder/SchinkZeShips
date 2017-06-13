using System;
using System.Net.Http;
using Acr.UserDialogs;
using SchinkZeShips.Core.ExtensionMethods;
using SchinkZeShips.Core.GameLogic;
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
		private bool _leftLobbyManually;

		public GameLobbyViewModel()
		{
			StartGameCommand = new Command(StartGameAsync, () => IsLobbyLeader && HasParticipant);
			LeaveGameCommand = new Command(LeaveGameAsync);
			KickParticipantCommand = new Command(KickParticipantAsync, () => CanKickParticipant);
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
			_leftLobbyManually = true;
			var dialog = CreateLoadingDialog("Verlasse Spiel");
			dialog.Show();

			try
			{
				await Service.RemoveFromGame(CurrentGame.Id, Settings.Instance.UserId);

				dialog.Hide();
				PushViewModal(new StartView());
			}
			catch (HttpRequestException)
			{
				UserDialogs.Instance.AlertNoConnection();
				_leftLobbyManually = false;
			}
			finally
			{
				if (dialog.IsShowing) dialog.Hide();
			}
		}

		public Game CurrentGame
		{
			get { return _currentGame; }
			set
			{
				_currentGame = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsLobbyLeader));
				OnPropertyChanged(nameof(HasParticipant));
				OnPropertyChanged(nameof(CanKickParticipant));
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

		public bool IsLobbyLeader => CurrentGame != null &&
		                              Equals(CurrentGame.GameCreator.Id, Settings.Instance.UserId);

		public bool HasParticipant => CurrentGame?.GameParticipant != null;

		public bool CanKickParticipant => HasParticipant && IsLobbyLeader;

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
			if (_leftLobbyManually)
			{
				return;
			}

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
				PushViewModal(new ConfigureBoardView(ownGame));
				return;
			}

			CurrentGame = ownGame;
		}

		private async void StartGameAsync()
		{
			_leftLobbyManually = true;
			var dialog = CreateLoadingDialog("Starte Spiel");
			dialog.Show();

			try
			{
				await Service.UpdateGame(CurrentGame.Id, new GameState());

				dialog.Hide();

				PushViewModal(new ConfigureBoardView(CurrentGame));
			}
			catch (HttpRequestException)
			{
				UserDialogs.Instance.AlertNoConnection();
				_leftLobbyManually = false;
			}
			finally
			{
				if (dialog.IsShowing) dialog.Hide();
			}
		}
	}
}