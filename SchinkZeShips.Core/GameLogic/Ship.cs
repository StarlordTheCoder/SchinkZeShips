using System;
using System.Collections.Generic;
using System.Linq;
using SchinkZeShips.Core.Infrastructure;

namespace SchinkZeShips.Core.GameLogic
{
	public class Ship : NotifyPropertyChangedBase
	{
		public bool IsOwnerShip { get; }

		public Ship(IReadOnlyCollection<CellViewModel> shipParts, bool isOwnerShip)
		{
			ShipParts = shipParts;
			IsOwnerShip = isOwnerShip;
		}

		public void Update()
		{
			OnPropertyChanged(nameof(ShipVisible));
		}

		public IReadOnlyCollection<CellViewModel> ShipParts { get; }

		public ShipType? ShipType
		{
			get
			{
				var shipLength = ShipParts.Count;

				var validShipType = Enum.IsDefined(typeof(ShipType), shipLength);

				return validShipType ? (ShipType?)shipLength : null;
			}
		}

		public bool ShipVisible => ShipParts.Count > 0 && (IsOwnerShip || ShipParts.All(s => s.Model.WasShot));
	}
}
