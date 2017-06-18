using System.Runtime.Serialization;

namespace SchinkZeShips.Server
{
	[DataContract]
	public class GameState
	{
		[DataMember]
		public bool CurrentPlayerIsGameCreator { get; set; }

		[DataMember]
		public BoardState BoardCreator { get; set; }

		[DataMember]
		public BoardState BoardParticipant { get; set; }
	}
}