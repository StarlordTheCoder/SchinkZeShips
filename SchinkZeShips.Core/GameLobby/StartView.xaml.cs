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
			this.Subscribe<StartViewModel, StartView>();
			(BindingContext as ViewModelBase)?.OnAppearing();
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			this.Unsubscribe<StartViewModel, StartView>();
			(BindingContext as ViewModelBase)?.OnDisappearing();
			base.OnDisappearing();
		}
	}
}