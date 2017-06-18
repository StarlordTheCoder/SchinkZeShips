using System;
using SchinkZeShips.Core.ExtensionMethods;
using SchinkZeShips.Core.SchinkZeShipsReference;
using Xamarin.Forms;

namespace SchinkZeShips.Core.Converter
{
	public class GameStateBoardConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var game = (Game)value;

			if (game == null)
			{
				return null;
			}

			return ReturnOwnerBoard ?
				game.ThisPlayerBoard() :
				game.OtherPlayerBoard();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		public bool ReturnOwnerBoard { get; set; }
	}
}
