using System;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.GameLogic
{
	public class CellViewModel : NotifyPropertyChangedBase
	{
		private bool _isSelected;
		private CellState _model;
		private Ship _ship;

		public CellViewModel(Coordinate coordinate)
		{
			Coordinate = coordinate;
		}

		public Coordinate Coordinate { get; }

		public CellState Model
		{
			get => _model;
			set
			{
				_model = value;
				OnPropertyChanged();
			}
		}

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				if (_isSelected == value) return;
				_isSelected = value;
				OnPropertyChanged();

				RaiseSelectedChanged();
			}
		}

		public Ship Ship
		{
			get => _ship;
			set
			{
				_ship = value;
				Model.ShipId = _ship?.ShipId;
				_ship?.Update();
				OnPropertyChanged();
			}
		}


		public event EventHandler<EventArgs> SelectedChanged;

		private void RaiseSelectedChanged()
		{
			SelectedChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}