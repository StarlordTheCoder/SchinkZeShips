using System.Net.Http;
using Acr.UserDialogs;
using SchinkZeShips.Core.Infrastructure;
using Xamarin.Forms;

namespace SchinkZeShips.Core
{
	public class StartViewModel : ViewModelBase
	{
		public StartViewModel()
		{
			CreateGameCommand = new Command(CreateGame);
			SearchGameCommand = new Command(SearchGame);
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

					//TODO CreateGameView
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
		}

		public void SearchGame()
		{
			//TODO SearchGameView
			PushView(this, new StartView());
		}
	}
}