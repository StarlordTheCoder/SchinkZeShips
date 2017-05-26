using System.Runtime.Serialization;

namespace SchinkZeShips.Server
{
	[DataContract]
	public class PlayingFieldState
	{
		[DataMember]
		public CellState[] Cells { get; set; }
	}
}