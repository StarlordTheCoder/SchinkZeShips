using System;
using System.ServiceModel;
using SchinkZeShips.Core.ExtensionMethods;
using SchinkZeShips.Core.GameLogic;
using SchinkZeShips.Core.Infrastructure;
using Xamarin.Forms;

namespace SchinkZeShips.Core.GameLobby
{
	public class StartViewModel : ViewModelBase
	{
		public StartViewModel()
		{
			CreateGameCommand = new Command(CreateGame, () => !string.IsNullOrEmpty(Settings.Username));
			SearchGameCommand = new Command(SearchGame, () => !string.IsNullOrEmpty(Settings.Username));
			TestIfAlreadyInGameAsync();
		}

		private async void TestIfAlreadyInGameAsync()
		{
			ShowLoading("Überprüfe Spielzustand");

			try
			{
				var game = await Service.GetCurrentGame();

				if (game != null)
				{
					if (game.IsInLobby())
					{
						PushViewModal(new GameLobbyView(game));
					}
					else if (game.IsConfiguringBoard())
					{
						var isCreator = game.CurrentPlayerIsLobbyCreator();
						if (isCreator && game.RunningGameState.PlayingFieldCreator == null ||
						    !isCreator && game.RunningGameState.PlayingFieldParticipant == null)
						{
							PushViewModal(new ConfigureBoardView(game));
						}
						else
						{
							PushViewModal(new InGameView(game));
						}
					}
					else if (game.IsPlaying())
					{
						PushViewModal(new InGameView(game));
					}
					else
					{
						throw new Exception("Dieser Fall sollte niemals eintreten");
					}
				}
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

		private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			CreateGameCommand.ChangeCanExecute();
			SearchGameCommand.ChangeCanExecute();
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
			Settings.PropertyChanged += Settings_PropertyChanged;
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			Settings.PropertyChanged -= Settings_PropertyChanged;
		}

		public Command CreateGameCommand { get; }
		public Command SearchGameCommand { get; }

		public Settings Settings { get; } = Settings.Instance;

		private async void CreateGame()
		{
			var result = await Dialogs.PromptAsync("Spielname eingeben", okText: "Spiel erstellen", cancelText: "Abbrechen", placeholder: "Spielname");

			if (result.Ok)
			{
				ShowLoading("Erstelle Spiel");

				try
				{
					var game = await Service.CreateGame(result.Text);

					PushViewModal(new GameLobbyView(game));
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
		}

		private void SearchGame()
		{
			PushView(this, new SearchLobbyView());
		}
	}
}