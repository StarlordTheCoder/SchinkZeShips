using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using SchinkZeShips.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace SchinkZeShips.Android
{
	[Activity(Label = "SchinkZeShips", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			Forms.Init(this, bundle);
			UserDialogs.Init(() => (Activity) Forms.Context);
			LoadApplication(new App());
		}
	}
}