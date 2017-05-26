using System.Runtime.Serialization;

namespace SchinkZeShips.Server
{
	[DataContract]
	public class GameState
	{
		[DataMember]
		public Player CurrentPlayer { get; set; }

		[DataMember]
		public PlayingFieldState PlayingFieldCreator { get; set; }

		[DataMember]
		public PlayingFieldState PlayingFieldParticipant { get; set; }
	}
}