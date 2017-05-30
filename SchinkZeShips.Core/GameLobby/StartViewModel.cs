using System.Net.Http;
using Acr.UserDialogs;
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

		public async void CreateGame()
		{
			var result = await UserDialogs.Instance.PromptAsync($"Spiel von {Settings.Username}", okText: "Spiel erstellen", placeholder: "Spielname");

			if (result.Ok)
			{
				var dialog = CreateLoadingDialog("Erstelle Spiel");
				dialog.Show();

				try
				{
					var game = await Service.CreateGame(result.Text);

					PushView(this, new GameLobbyView());
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
		}

		public void SearchGame()
		{
			//TODO SearchGameView
			PushView(this, new StartView());
		}
	}
}