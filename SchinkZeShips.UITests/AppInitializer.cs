using Xamarin.UITest;
using Xamarin.UITest.Configuration;

namespace SchinkZeShips.UITests
{
	public static class AppInitializer
	{
		public static IApp StartApp(Platform platform, AppDataMode dataMode = AppDataMode.Auto)
		{
			if (platform == Platform.Android)
				return ConfigureApp
					.Android
					.InstalledApp("ch.tbz.schinkzeships")
					.StartApp(dataMode);

			return ConfigureApp
				.iOS
				.StartApp();
		}
	}
}