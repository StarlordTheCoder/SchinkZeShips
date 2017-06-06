using SchinkZeShips.Core.Infrastructure;

namespace SchinkZeShips.Core.GameLobby
{
	public partial class StartView
	{
		public StartView()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			this.Subscribe<StartViewModel>();
			(BindingContext as ViewModelBase)?.OnAppearing();
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			this.Unsubscribe<StartViewModel>();
			(BindingContext as ViewModelBase)?.OnDisappearing();
			base.OnDisappearing();
		}
	}
}