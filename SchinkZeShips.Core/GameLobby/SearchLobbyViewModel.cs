using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
		private string _gameFilter = string.Empty;
		private bool _isLoadingGames;

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
				ApplyFilter();
			}
		}

		public bool IsLoadingGames
		{
			get { return _isLoadingGames; }
			set
			{
				if (Equals(_isLoadingGames, value)) return;
				_isLoadingGames = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<Game> Games { get; } = new ObservableCollection<Game>();

		public Command LoadGamesCommand { get; }

		private IList<Game> _allGames = new List<Game>();

		private async void LoadGames()
		{
			IsLoadingGames = true;
			var dialog = CreateLoadingDialog("Lade Spiele");
			dialog.Show();

			try
			{
				var games = await Service.GetAllGames();

				_allGames = games;

				ApplyFilter();
			}
			catch (HttpRequestException)
			{
				UserDialogs.Instance.AlertNoConnection();
			}
			finally
			{
				dialog.Hide();
				IsLoadingGames = false;
			}
		}

		private void ApplyFilter()
		{
			Games.Clear();

			var filteredGames = string.IsNullOrEmpty(GameFilter)
				? _allGames
				: _allGames.Where(g => g.Name.ContainsIgnoreCase(GameFilter) ||
				                       g.GameCreator.Username.ContainsIgnoreCase(GameFilter));

			foreach (var game in filteredGames)
				Games.Add(game);
		}

		public async Task JoinGame(Game game)
		{
			var dialog = CreateLoadingDialog("Trete Spiel bei");
			dialog.Show();

			try
			{
				await Service.JoinGame(game.Id);

				game.GameParticipant = Settings.Instance.Player;

				PushViewModal(new GameLobbyView(game));
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