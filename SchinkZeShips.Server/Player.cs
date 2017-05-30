using System;
using System.Runtime.Serialization;

namespace SchinkZeShips.Server
{
	[DataContract]
	public class Player
	{
		[DataMember]
		public string Id { get; set; } = Guid.NewGuid().ToString();

		[DataMember]
		public string Username { get; set; }

		public override bool Equals(object obj)
		{
			var other = obj as Player;

			return other != null && other.Id == Id && other.Username == Username;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode() ^ Username.GetHashCode();
		}
	}
}