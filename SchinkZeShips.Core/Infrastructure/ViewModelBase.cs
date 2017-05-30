using System.ComponentModel;
using System.Runtime.CompilerServices;
using Acr.UserDialogs;
using Xamarin.Forms;

// ReSharper disable ExplicitCallerInfoArgument

namespace SchinkZeShips.Core.Infrastructure
{
	public class ViewModelBase : NotifyPropertyChangedBase
	{
		/// <summary>
		///     Constant for the PushView request
		/// </summary>
		public const string NavigationPushView = "Naviagion.PushView";

		protected readonly GameLogicService Service = new GameLogicService();

		protected static IProgressDialog CreateLoadingDialog(string title)
		{
			return UserDialogs.Instance.Loading(title);
		}

		/// <summary>
		///     Tells the current view to navigate to the provided page
		/// </summary>
		/// <typeparam name="TViewModel">The type of the viewmodel</typeparam>
		/// <param name="instance">The instance of the viewmodel</param>
		/// <param name="page">The page to navigate to</param>
		protected static void PushView<TViewModel>(TViewModel instance, Page page) where TViewModel : class
		{
			MessagingCenter.Send(instance, NavigationPushView, page);
		}

	}
}