using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.GameLobby
{
	public partial class GameLobbyView
	{
		public GameLobbyView(Game game)
		{
			InitializeComponent();

			((GameLobbyViewModel) BindingContext).CurrentGame = game;
		}

		protected override void OnAppearing()
		{
			this.Subscribe<GameLobbyViewModel>();
			(BindingContext as ViewModelBase)?.OnAppearing();
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			this.Unsubscribe<GameLobbyViewModel>();
			(BindingContext as ViewModelBase)?.OnDisappearing();
			base.OnDisappearing();
		}
	}
}