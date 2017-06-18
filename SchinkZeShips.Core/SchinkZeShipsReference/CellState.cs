using System;
using System.Runtime.Serialization;
using Xamarin.Forms;

namespace SchinkZeShips.Core.SchinkZeShipsReference
{
	public partial class CellState
	{
		[IgnoreDataMember]
		private bool _isSelected;

		[IgnoreDataMember]
		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				if (_isSelected == value) return;
				_isSelected = value;
				RaisePropertyChanged(nameof(IsSelected));


				if(_isSelected) RaiseGotSelected();
			}
		}


		public event EventHandler<EventArgs> GotSelected;

		private void RaiseGotSelected()
		{
			GotSelected?.Invoke(this, EventArgs.Empty);
		}
	}
}