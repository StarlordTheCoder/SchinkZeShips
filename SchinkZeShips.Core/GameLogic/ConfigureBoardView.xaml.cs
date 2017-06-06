using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.GameLogic
{
	public partial class ConfigureBoardView
	{
		public ConfigureBoardView(Game game)
		{
			InitializeComponent();

			((ConfigureBoardViewModel) BindingContext).CurrentGame = game;
		}

		protected override void OnAppearing()
		{
			this.Subscribe<ConfigureBoardViewModel>();
			(BindingContext as ViewModelBase)?.OnAppearing();
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			this.Unsubscribe<ConfigureBoardViewModel>();
			(BindingContext as ViewModelBase)?.OnDisappearing();
			base.OnDisappearing();
		}
	}
}