using System.Runtime.Serialization;

namespace SchinkZeShips.Server
{
	[DataContract]
	public class Player
	{
		[DataMember]
		public string Id { get; set; }

		[DataMember]
		public string Username { get; set; }

		public override bool Equals(object obj)
		{
			var other = obj as Player;

			return other != null && other.Id == Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}