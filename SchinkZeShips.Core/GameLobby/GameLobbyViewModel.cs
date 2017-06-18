using System;
using System.ServiceModel;
using SchinkZeShips.Core.ExtensionMethods;
using SchinkZeShips.Core.GameLogic.BoardConfiguration;
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

			ShowLoading($"Entferne {CurrentGame.GameParticipant.Username} vom Spiel");

			try
			{
				await Service.RemoveFromGame(CurrentGame.Id, CurrentGame.GameParticipant.Id);
			}
			catch (FaultException)
			{
				throw;
			}
			catch (CommunicationException)
			{
				Dialogs.AlertNoConnection();
			}
			finally
			{
				HideLoading();
			}
		}

		private async void LeaveGameAsync()
		{
			_leftLobbyManually = true;

			ShowLoading("Verlasse Spiel");

			try
			{
				await Service.RemoveFromGame(CurrentGame.Id, Settings.Instance.UserId);

				PushViewModal(new StartView());
			}
			catch (FaultException)
			{
				throw;
			}
			catch (CommunicationException)
			{
				Dialogs.AlertNoConnection();
				_leftLobbyManually = false;
			}
			finally
			{
				HideLoading();
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
		                              CurrentGame.ThisPlayerIsGameCreator();

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
				await Dialogs.AlertAsync("Sie sind nicht mehr Teil eines Spiels!");
				return;
			}

			if (ownGame.IsConfiguringBoard())
			{
				PushViewModal(new ConfigureBoardView(ownGame));
				await Dialogs.AlertAsync("Das Spiel wurde gestartet!");
				return;
			}

			CurrentGame = ownGame;
		}

		private async void StartGameAsync()
		{
			_leftLobbyManually = true;
			ShowLoading("Starte Spiel");

			try
			{
				await Service.UpdateGameState(CurrentGame.Id, new GameState());

				PushViewModal(new ConfigureBoardView(CurrentGame));
			}
			catch (FaultException)
			{
				throw;
			}
			catch (CommunicationException)
			{
				Dialogs.AlertNoConnection();
				_leftLobbyManually = false;
			}
			finally
			{
				HideLoading();
			}
		}
	}
}