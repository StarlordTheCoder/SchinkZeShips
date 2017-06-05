using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace SchinkZeShips.Core.Infrastructure
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public class Settings : NotifyPropertyChangedBase
	{
		public static Settings Instance => _instance ?? (_instance = new Settings());

		private static ISettings AppSettings => CrossSettings.Current;

		#region Setting Constants

		public const string UiTestsGuid = "a070d89f-28c0-442a-989c-66aa7c24d206";

		private const string UsernameKey = "username";
		private static readonly string UsernameDefault = string.Empty;
		private static Settings _instance;

		private const string GuidKey = "guid";

		#endregion

		public string Username
		{
			get { return AppSettings.GetValueOrDefault(UsernameKey, UsernameDefault); }
			set
			{
				AppSettings.AddOrUpdateValue(UsernameKey, value);
				OnPropertyChanged();
			}
		}

		// To ensure same behaviour every run UITESTS use a static UserId
#if UITESTS
		public Guid Guid => Guid.Parse(UiTestsGuid);

		public string UserId => UiTestsGuid;
#else
		public Guid Guid
		{
			get { return AppSettings.GetValueOrDefault<Guid>(GuidKey); }
			set
			{
				AppSettings.AddOrUpdateValue(GuidKey, value);
				OnPropertyChanged();
				// ReSharper disable once ExplicitCallerInfoArgument
				OnPropertyChanged(nameof(UserId));
			}
		}

		public string UserId => Guid.ToString();

		private Settings()
		{
			if (!AppSettings.Contains(GuidKey))
			{
				Guid = Guid.NewGuid();
			}
		}
#endif

	}
}
