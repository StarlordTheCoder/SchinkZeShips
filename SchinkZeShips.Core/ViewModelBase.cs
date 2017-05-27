using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// ReSharper disable ExplicitCallerInfoArgument

namespace SchinkZeShips.Core
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		protected GameLogicService Service = new GameLogicService();
		private int _workers;
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public bool IsBusy => Workers > 0;
		public bool IsNotBusy => !IsBusy;

		private int Workers
		{
			get { return _workers; }
			set
			{
				_workers = value;
				OnPropertyChanged(nameof(IsBusy));
				OnPropertyChanged(nameof(IsNotBusy));
			}
		}

		protected async Task RunAsyncOperation(Func<Task> asyncOperation)
		{
			try
			{
				Workers++;
				await asyncOperation();
			}
			finally
			{
				Workers--;
			}
		}

		protected async Task<T> RunAsyncOperation<T>(Func<Task<T>> asyncOperation)
		{
			try
			{
				Workers++;
				return await asyncOperation();
			}
			finally
			{
				Workers--;
			}
		}
	}
}