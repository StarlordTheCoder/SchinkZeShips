using Xamarin.UITest;

namespace SchinkZeShips.UITests
{
	public static class AppInitializer
	{
		public static IApp StartApp(Platform platform)
		{
			if (platform == Platform.Android)
				return ConfigureApp
					.Android
					.InstalledApp("ch.tbz.schinkzeships")
					.StartApp();

			return ConfigureApp
				.iOS
				.StartApp();
		}
	}
}