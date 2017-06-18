using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace SchinkZeShips.Server
{
	[DataContract]
	public class Game
	{
		private Player _gameCreator;
		private Player _gameParticipant;
		private GameState _runningGameState;

		[DataMember]
		[ReadOnly(true)]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		[DataMember]
		[ReadOnly(true)]
		public string Name { get; set; }

		[DataMember]
		public Player GameCreator
		{
			get { return _gameCreator; }
			set
			{
				if (Equals(_gameCreator, value)) return;
				_gameCreator = value;
				OnDataChange();
			}
		}

		[DataMember]
		public Player GameParticipant
		{
			get { return _gameParticipant; }
			set
			{
				if (Equals(_gameParticipant, value)) return;
				_gameParticipant = value;
				OnDataChange();
			}
		}

		[DataMember]
		public GameState RunningGameState
		{
			get { return _runningGameState; }
			set
			{
				if (Equals(_runningGameState, value)) return;
				_runningGameState = value;
				OnDataChange();
			}
		}

		[DataMember]
		[ReadOnly(true)]
		public DateTime LatestChangeTime { get; set; } = DateTime.UtcNow;

		private void OnDataChange()
		{
			LatestChangeTime = DateTime.UtcNow;
		}

		public override bool Equals(object obj)
		{
			var other = obj as Game;

			return other != null &&
				Equals(other.Id, Id) &&
				Equals(other.LatestChangeTime, LatestChangeTime);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode() ^ LatestChangeTime.GetHashCode();
		}
	}
}