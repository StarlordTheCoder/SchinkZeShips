using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.GameLogic
{
	public partial class InGameView
	{
		public InGameView(Game game)
		{
			InitializeComponent();

			((InGameViewModel) BindingContext).CurrentGame = game;
		}

		protected override void OnAppearing()
		{
			this.Subscribe<InGameViewModel>();
			(BindingContext as ViewModelBase)?.OnAppearing();
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			this.Unsubscribe<InGameViewModel>();
			(BindingContext as ViewModelBase)?.OnDisappearing();
			base.OnDisappearing();
		}
	}
}