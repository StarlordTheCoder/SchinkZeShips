using Acr.UserDialogs;
using Xamarin.Forms;

// ReSharper disable ExplicitCallerInfoArgument

namespace SchinkZeShips.Core.Infrastructure
{
	public abstract class ViewModelBase : NotifyPropertyChangedBase
	{
		/// <summary>
		///     Constant for the PushView request
		/// </summary>
		public const string NavigationPushView = "Naviagion.PushView";

		protected static readonly IUserDialogs Dialogs = UserDialogs.Instance;

		protected readonly GameLogicService Service = new GameLogicService();

		protected static void ShowLoading(string title)
		{
			Dialogs.ShowLoading(title);
		}

		protected static void HideLoading()
		{
			Dialogs.HideLoading();
		}

		/// <summary>
		///     Tells the current view to navigate to the provided page
		/// </summary>
		/// <typeparam name="TViewModel">The type of the viewmodel</typeparam>
		/// <param name="instance">The instance of the viewmodel</param>
		/// <param name="page">The page to navigate to</param>
		protected static void PushView<TViewModel>(TViewModel instance, ContentPage page) where TViewModel : class
		{
			Dialogs.HideLoading();
			MessagingCenter.Send(instance, NavigationPushView, page);
		}

		/// <summary>
		///     Changes the main page of the application, disabling navigation
		///     Wraps the provided page inside a navigation page
		/// </summary>
		/// <param name="page">The page to display</param>
		protected static void PushViewModal(ContentPage page)
		{
			Device.BeginInvokeOnMainThread(() => { Application.Current.MainPage = new NavigationPage(page); });
		}

		public virtual void OnAppearing()
		{
		}

		public virtual void OnDisappearing()
		{
		}
	}
}