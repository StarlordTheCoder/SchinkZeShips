using System.ServiceModel;
using SchinkZeShips.Core.ExtensionMethods;
using SchinkZeShips.Core.GameLogic.BoardConfiguration;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;

namespace SchinkZeShips.Core.GameLobby
{
	public class GameLobbyViewModel : ViewModelWithCurrentGameBase
	{
		private const int GameLobbyRefreshTimeoutInMs = 2000;
		private Game _currentGame;
		private bool _leftLobbyManually;

		public GameLobbyViewModel() : base(GameLobbyRefreshTimeoutInMs)
		{
			StartGameCommand = new Command(StartGameAsync, () => IsLobbyLeader && HasParticipant);
			LeaveGameCommand = new Command(LeaveGameAsync);
			KickParticipantCommand = new Command(KickParticipantAsync, () => CanKickParticipant);
		}

		public override Game CurrentGame
		{
			get => _currentGame;
			set
			{
				if (_leftLobbyManually)
					return;

				if (value == null)
				{
					PushViewModal(new StartView());
					Dialogs.Alert("Sie sind nicht mehr Teil eines Spiels!");
					return;
				}

				if (value.IsConfiguringBoard())
				{
					PushViewModal(new ConfigureBoardView(value));
					Dialogs.Alert("Das Spiel wurde gestartet!");
					return;
				}

				_currentGame = value;

				OnPropertyChanged();
				OnPropertyChanged(nameof(IsLobbyLeader));
				OnPropertyChanged(nameof(HasParticipant));
				OnPropertyChanged(nameof(CanKickParticipant));
				StartGameCommand.ChangeCanExecute();
				KickParticipantCommand.ChangeCanExecute();
			}
		}

		public Command StartGameCommand { get; }
		public Command LeaveGameCommand { get; }
		public Command KickParticipantCommand { get; }

		public bool IsLobbyLeader => CurrentGame != null &&
		                             CurrentGame.ThisPlayerIsGameCreator();

		public bool HasParticipant => CurrentGame?.GameParticipant != null;

		public bool CanKickParticipant => HasParticipant && IsLobbyLeader;

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