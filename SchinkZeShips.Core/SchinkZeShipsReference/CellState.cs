using System.ComponentModel;

namespace SchinkZeShips.Core.SchinkZeShipsReference
{
	public partial class CellState
	{
		public CellState()
		{
			PropertyChanged += OnPropertyChanged;
		}

		public bool HasShip => !string.IsNullOrEmpty(ShipId);

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			if (propertyChangedEventArgs.PropertyName == nameof(ShipId))
				RaisePropertyChanged(nameof(HasShip));
		}
	}
}