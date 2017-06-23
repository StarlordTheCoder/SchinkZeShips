using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;

namespace SchinkZeShips.Core.GameLobby
{
	public partial class SearchLobbyView
	{
		public SearchLobbyView()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			this.Subscribe<SearchLobbyViewModel>();
			(BindingContext as ViewModelBase)?.OnAppearing();
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			this.Unsubscribe<SearchLobbyViewModel>();
			(BindingContext as ViewModelBase)?.OnDisappearing();
			base.OnDisappearing();
		}

		private async void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
		{
			await ((SearchLobbyViewModel) BindingContext).JoinGame((Game) e.Item);
		}
	}
}