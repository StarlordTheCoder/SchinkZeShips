using System.Runtime.Serialization;

namespace SchinkZeShips.Server
{
	[DataContract]
	public class CellState
	{
		[DataMember]
		public bool HasShip { get; set; }

		[DataMember]
		public bool WasShot { get; set; }

		public override bool Equals(object obj)
		{
			var other = obj as CellState;

			return other != null &&
				other.HasShip == HasShip &&
				other.WasShot == WasShot;
		}

		public override int GetHashCode()
		{
			return HasShip.GetHashCode() ^ WasShot.GetHashCode();
		}
	}
}