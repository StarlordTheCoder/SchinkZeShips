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

		private const string UsernameKey = "username";
		private static readonly string UsernameDefault = string.Empty;
		private static Settings _instance;

		private const string GuidKey = "guid";

		#endregion

		private Settings()
		{
			if (!AppSettings.Contains(GuidKey))
			{
				Guid = Guid.NewGuid();
			}
		}


		public string Username
		{
			get { return AppSettings.GetValueOrDefault(UsernameKey, UsernameDefault); }
			set
			{
				AppSettings.AddOrUpdateValue(UsernameKey, value);
				OnPropertyChanged();
			}
		}

		public Guid Guid
		{
			get { return AppSettings.GetValueOrDefault<Guid>(GuidKey); }
			set
			{
				AppSettings.AddOrUpdateValue(GuidKey, value);
				OnPropertyChanged();
				OnPropertyChanged(nameof(UserId));
			}
		}

		public string UserId => Guid.ToString();
	}
}
