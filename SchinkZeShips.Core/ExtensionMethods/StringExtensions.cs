using System;

namespace SchinkZeShips.Core.ExtensionMethods
{
	public static class StringExtensions
	{
		public static bool ContainsIgnoreCase(this string source, string stringToContain)
		{
			return source.IndexOf(stringToContain, StringComparison.OrdinalIgnoreCase) >= 0;
		}
	}
}
