using System.Runtime.Serialization;

namespace SchinkZeShips.Server
{
	[DataContract]
	public class GameState
	{
		[DataMember]
		public bool CurrentPlayerIsGameCreator { get; set; }

		[DataMember]
		public PlayingFieldState PlayingFieldCreator { get; set; }

		[DataMember]
		public PlayingFieldState PlayingFieldParticipant { get; set; }

		public override bool Equals(object obj)
		{
			var other = obj as GameState;

			return other != null &&
				other.CurrentPlayerIsGameCreator == CurrentPlayerIsGameCreator &&
				Equals(other.PlayingFieldCreator, PlayingFieldCreator) &&
				Equals(other.PlayingFieldParticipant, PlayingFieldParticipant);
		}

		public override int GetHashCode()
		{
			return CurrentPlayerIsGameCreator.GetHashCode() ^ PlayingFieldCreator.GetHashCode() ^ PlayingFieldCreator.GetHashCode();
		}
	}
}