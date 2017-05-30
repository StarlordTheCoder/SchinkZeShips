using System.ComponentModel;
using System.Runtime.CompilerServices;
using Acr.UserDialogs;

// ReSharper disable ExplicitCallerInfoArgument

namespace SchinkZeShips.Core
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		protected readonly GameLogicService Service = new GameLogicService();
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected static IProgressDialog CreateLoadingDialog(string title)
		{
			return UserDialogs.Instance.Loading(title);
		}
	}
}