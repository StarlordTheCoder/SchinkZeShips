using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Acr.UserDialogs;
using SchinkZeShips.Core.ExtensionMethods;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;

namespace SchinkZeShips.Core.GameLobby
{
	public class SearchLobbyViewModel : ViewModelBase
	{
		private string _gameFilter;

		public SearchLobbyViewModel()
		{
			LoadGamesCommand = new Command(LoadGames);
			LoadGames();
		}

		public string GameFilter
		{
			get { return _gameFilter; }
			set
			{
				if (Equals(_gameFilter, value)) return;
				_gameFilter = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<Game> Games { get; } = new ObservableCollection<Game>();

		public Command LoadGamesCommand { get; }

		private async void LoadGames()
		{
			var dialog = CreateLoadingDialog("Lade Spiele");
			dialog.Show();

			try
			{
				var games = await Service.GetAllGames();

				Games.Clear();
				foreach (var game in games)
					Games.Add(game);
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

		public async Task JoinGame(Game game)
		{
			var dialog = CreateLoadingDialog("Trete Spiel bei");
			dialog.Show();

			try
			{
				await Service.JoinGame(game.Id);

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
}