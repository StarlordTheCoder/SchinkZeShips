namespace SchinkZeShips
{
	public partial class SharedApp
	{
		public SharedApp()
		{
			InitializeComponent();

			MainPage = new StartView();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}