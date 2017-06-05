using Acr.UserDialogs;

namespace SchinkZeShips.Core.ExtensionMethods
{
	public static class UserDialogsExtensions
	{
		public static void AlertNoConnection(this IUserDialogs dialogs)
		{
			dialogs.Alert("Fehler beim Verbinden mit dem Server!", "Verbindung fehlgeschlagen");
		}
	}
}
