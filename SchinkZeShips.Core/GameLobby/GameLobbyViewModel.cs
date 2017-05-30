using System;
using System.Net.Http;
using Acr.UserDialogs;
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
			StartGameCommand = new Command(StartGame, () => IsLobbyLeader && HasParticipant);
			//LeaveGameCommand = new Command(LeaveGame);
			//KickParticipantCommand = new Command(KickParticipant, () => IsLobbyLeader && HasParticipant);
		}

		public Game CurrentGame
		{
			get => _currentGame;
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
			get => _onViewVisible;
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
		                              Equals(CurrentGame.GameCreator.Id, Settings.Instance.Guid.ToString());

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
			OnTimerElapsedAsync();
			return true;
		}

		private async void OnTimerElapsedAsync()
		{
			var ownGame = await Service.GetCurrentGame();

			if (ownGame == null)
			{
				await UserDialogs.Instance.AlertAsync("Sie sind nicht mehr Teil eines Spieles!");
				//TODO Push start View
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

		public async void StartGame()
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
				UserDialogs.Instance.Alert("Fehler beim Verbinden mit dem Server!");
			}
			finally
			{
				dialog.Hide();
			}
		}

		private bool CanStartGame()
		{
			if (CurrentGame == null) return false;

			var isLobbyCreator = Equals(CurrentGame.GameCreator.Id, Settings.Instance.Guid.ToString());

			var lobbyFull = CurrentGame.GameParticipant != null;

			return isLobbyCreator && lobbyFull;
		}
	}
}