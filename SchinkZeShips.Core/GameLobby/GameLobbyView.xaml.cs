using SchinkZeShips.Core.Infrastructure;

namespace SchinkZeShips.Core.GameLobby
{
	public partial class GameLobbyView
	{
		public GameLobbyView()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			this.Subscribe<GameLobbyViewModel, GameLobbyView>();
			(BindingContext as ViewModelBase)?.OnAppearing();
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			this.Unsubscribe<GameLobbyViewModel, GameLobbyView>();
			(BindingContext as ViewModelBase)?.OnDisappearing();
			base.OnDisappearing();
		}
	}
}