using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SchinkZeShips.Core.Connected_Services.SchinkZeShipsReference;

namespace SchinkZeShips
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
			await RunAsyncOperation(async () =>
			{
				var games = await Service.GetAllGames();
				Games.Clear();
				games.AddRange(games);
			});
		}
	}
}
