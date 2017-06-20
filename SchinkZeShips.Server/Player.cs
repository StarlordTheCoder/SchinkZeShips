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
	}
}