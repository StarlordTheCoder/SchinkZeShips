using System;
using System.Linq;
using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;

namespace SchinkZeShips.Core.GameLogic
{
	public class CellViewModel : NotifyPropertyChangedBase
	{
		private bool _isSelected;
		private CellState _model;
		private Ship _ship;
		private ImageSource _shipImage;
		private int _rotation;

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

		public int Rotation
		{
			get => _rotation;
			set
			{
				_rotation = value;
				OnPropertyChanged();
			}
		}

		public ImageSource ShipImage
		{
			get => _shipImage;
			set
			{
				_shipImage = value;
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
				CheckShipImage();
			}
		}

		private void CheckShipImage()
		{
			if (Ship == null)
			{
				Rotation = 0;
				ShipImage = null;
				return;
			}

			var ownIndex = Ship.ShipParts.IndexOf(this);

			var previous = Ship.ShipParts.ElementAtOrDefault(ownIndex - 1);
			var next = Ship.ShipParts.ElementAtOrDefault(ownIndex + 1);

			if (previous != null && next != null)
			{
				ShipImage = ImageSource.FromResource("SchinkZeShips.Core.Resources.ShipCenter.png");
			}
			else
			{
				ShipImage = ImageSource.FromResource("SchinkZeShips.Core.Resources.ShipStartEnd.png");
			}

			var neighbour = (previous ?? next).Coordinate;
			if (neighbour.Row == Coordinate.Row)
			{
				Rotation = neighbour.Column > Coordinate.Column ? 180 : 0;
			}
			else
			{
				Rotation = neighbour.Row > Coordinate.Row ? 270 : 90;
			}
		}


		public event EventHandler<EventArgs> SelectedChanged;

		private void RaiseSelectedChanged()
		{
			SelectedChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}