using System.Runtime.Serialization;

namespace SchinkZeShips.Server
{
	[DataContract]
	public class CellState
	{
		[DataMember]
		public string ShipId { get; set; }

		[DataMember]
		public bool WasShot { get; set; }
	}
}