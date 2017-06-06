using Xamarin.Forms;

namespace SchinkZeShips.Core.Infrastructure
{
	/// <summary>
	///     A helper class for ViewModel-events
	/// </summary>
	public static class EventsHelper
	{
		/// <summary>
		///     Subscribes all events
		/// </summary>
		/// <typeparam name="TViewModel">The type of the viewmodel</typeparam>
		/// <param name="instance">The instance of the viewmodel</param>
		public static void Subscribe<TViewModel>(this ContentPage instance) where TViewModel : ViewModelBase
		{
			MessagingCenter.Subscribe<TViewModel, NavigationPage>(instance, ViewModelBase.NavigationPushView,
				(sender, page) =>
				{
					Device.BeginInvokeOnMainThread(async () =>
					{
						await instance.Navigation.PushAsync(page);
					});
				});
		}

		/// <summary>
		///     Unsubscribes all events
		/// </summary>
		/// <typeparam name="TViewModel">The type of the viewmodel</typeparam>
		/// <param name="instance">The instance of the viewmodel</param>
		public static void Unsubscribe<TViewModel>(this ContentPage instance) where TViewModel : ViewModelBase
		{
			MessagingCenter.Unsubscribe<TViewModel, NavigationPage>(instance, ViewModelBase.NavigationPushView);
		}
	}
}