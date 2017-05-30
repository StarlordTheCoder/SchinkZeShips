using System;
using System.Runtime.Serialization;

namespace SchinkZeShips.Server
{
	[DataContract]
	public class Game
	{
		[DataMember]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public Player GameCreator { get; set; }

		[DataMember]
		public Player GameParticipant { get; set; }

		[DataMember]
		public GameState RunningGameState { get; set; }

		public override bool Equals(object obj)
		{
			var other = obj as Game;

			return other != null &&
				other.Id == Id &&
				Equals(other.GameCreator, GameCreator) &&
				Equals(other.GameParticipant, GameParticipant) &&
				Equals(other.RunningGameState, RunningGameState);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode() ^ GameCreator.GetHashCode() ^ GameParticipant.GetHashCode() ^ RunningGameState.GetHashCode();
		}
	}
}