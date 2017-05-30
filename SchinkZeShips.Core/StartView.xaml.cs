using SchinkZeShips.Core.Infrastructure;

namespace SchinkZeShips.Core
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
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			this.Unsubscribe<StartViewModel, StartView>();
			base.OnDisappearing();
		}
	}
}