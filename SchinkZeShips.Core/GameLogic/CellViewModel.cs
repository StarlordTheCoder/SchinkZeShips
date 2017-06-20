using System;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.GameLogic
{
	public class CellViewModel : NotifyPropertyChangedBase
	{
		private CellState _model;
		public Coordinate Coordinate { get; }

		public CellState Model
		{
			get { return _model; }
			set
			{
				_model = value;
				OnPropertyChanged();
			}
		}

		private bool _isSelected;
		private Ship _ship;

		public bool IsSelected
		{
			get { return _isSelected; }
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
			get { return _ship; }
			set
			{
				_ship = value;
				OnPropertyChanged();
				_ship?.Update();
			}
		}


		public event EventHandler<EventArgs> SelectedChanged;

		private void RaiseSelectedChanged()
		{
			SelectedChanged?.Invoke(this, EventArgs.Empty);
		}

		public CellViewModel(Coordinate coordinate)
		{
			Coordinate = coordinate;
		}
	}
}
