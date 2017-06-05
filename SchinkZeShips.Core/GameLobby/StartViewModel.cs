using System.Net.Http;
using Acr.UserDialogs;
using SchinkZeShips.Core.ExtensionMethods;
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
			var result = await UserDialogs.Instance.PromptAsync($"Spielname eingeben", okText: "Spiel erstellen", cancelText: "Abbrechen", placeholder: "Spielname");

			if (result.Ok)
			{
				var dialog = CreateLoadingDialog("Erstelle Spiel");
				dialog.Show();

				try
				{
					//TODO Remove game return value? Use it?
					var game = await Service.CreateGame(result.Text);

					PushViewModal(new GameLobbyView());
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

		public void SearchGame()
		{
			PushView(this, new SearchLobbyView());
		}
	}
}