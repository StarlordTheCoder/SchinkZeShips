using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Acr.UserDialogs;
using SchinkZeShips.Core.Connected_Services.SchinkZeShipsReference;

namespace SchinkZeShips.Core
{
	public class StartViewModel : ViewModelBase
	{
		public StartViewModel()
		{
			LoadGamesFromServer();
		}

		public ObservableCollection<Game> Games { get; } = new ObservableCollection<Game>();
		public string MainLabelText => $"Game count: {Games.Count}";

		public async void LoadGamesFromServer()
		{
			var dialog = CreateLoadingDialog("Lade alle Spiele");
			dialog.Show();
			await Task.Delay(10000);
			try
			{
				var games = await Service.GetAllGames();
				Games.Clear();
				games.AddRange(games);
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
}
