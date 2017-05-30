using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Xamarin.Forms;

namespace SchinkZeShips.Core
{
	public partial class App
	{
		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new GameLobby.StartView());
		}

		/// <inheritdoc />
		protected override void OnStart()
		{
			// Handle when your app starts
			MobileCenter.Start("android=8fc0dc45-73d1-42d4-89c7-e24cac248309;" +
							   "uwp=5a68fff6-b282-4395-b40d-8a9f239b8544;" +
							   "ios=cb9ff152-34e9-4f8b-9c2d-196c3ff38c2c",
				typeof(Analytics), typeof(Crashes));
		}

		/// <inheritdoc />
		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		/// <inheritdoc />
		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}