using System;
using System.Collections.Generic;
using System.Linq;
using SchinkZeShips.Core.Infrastructure;

namespace SchinkZeShips.Core.GameLogic
{
	public class Ship : NotifyPropertyChangedBase
	{
		public Ship(IReadOnlyCollection<CellViewModel> shipParts, bool isCreatorShip, string shipId)
		{
			ShipParts = shipParts;
			IsCreatorShip = isCreatorShip;
			ShipId = shipId;
		}

		public bool IsCreatorShip { get; }

		public string ShipId { get; }

		public IReadOnlyCollection<CellViewModel> ShipParts { get; }

		public ShipType? ShipType
		{
			get
			{
				var shipLength = ShipParts.Count;

				var validShipType = Enum.IsDefined(typeof(ShipType), shipLength);

				return validShipType ? (ShipType?) shipLength : null;
			}
		}

		public bool ShipVisible => ShipParts.Count > 0 && (IsCreatorShip || ShipParts.All(s => s.Model.WasShot));

		public void Update()
		{
			OnPropertyChanged(nameof(ShipVisible));
		}
	}
}