using Acr.UserDialogs;

namespace SchinkZeShips.Core.ExtensionMethods
{
	public static class UserDialogsExtensions
	{
		public static void AlertNoConnection(this IUserDialogs dialogs)
		{
			UserDialogs.Instance.HideLoading();
			dialogs.Alert("Fehler beim Verbinden mit dem Server!", "Verbindung fehlgeschlagen");
		}
	}
}
