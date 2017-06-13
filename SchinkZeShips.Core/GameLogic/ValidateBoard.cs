using SchinkZeShips.Core.Infrastructure;
using SchinkZeShips.Core.SchinkZeShipsReference;
using System.Collections.Generic;
using System.Linq;

namespace SchinkZeShips.Core.GameLogic
{
	public class ValidateBoard : NotifyPropertyChangedBase
	{
		public Dictionary<int, int> Ships = new Dictionary<int, int>();
		private PlayingFieldState _field;

		public int AllowedShipsOfLength2 => Ships[2];
		public int AllowedShipsOfLength3 => Ships[3];
		public int AllowedShipsOfLength4 => Ships[4];
		public int AllowedShipsOfLength5 => Ships[5];

		public ValidateBoard(PlayingFieldState field)
		{
			_field = field;


			Ships.Add(2, 4);
			Ships.Add(3, 3);
			Ships.Add(4, 2);
			Ships.Add(5, 1);
		}

		public bool CanAddShip(List<KeyValuePair<int, int>> addedCells)
		{
			var first = addedCells.First();

			var topLeft = new KeyValuePair<int, int>(first.Key == 0 ? first.Key : first.Key - 1, first.Value == 0 ? first.Value : first.Value - 1);

			var last = addedCells.Last();

			var bottomRight = new KeyValuePair<int, int>(last.Key == 9 ? last.Key : last.Key + 1, last.Value == 9 ? last.Value : last.Value + 1);

			for (int i = topLeft.Key; i <= bottomRight.Key; i++)
			{
				for (int j = topLeft.Value; j <= bottomRight.Value; j++)
				{
					if (_field.Cells[i][j].HasShip)
					{
						return false;
					}
				}
			}

			int allowedShips;
			var shipLength = addedCells.Count;
			var foundValue= Ships.TryGetValue(shipLength, out allowedShips);
			if (foundValue)
			{
				if (allowedShips > 0)
				{
					Ships[shipLength]--;
					OnPropertyChanged(nameof(AllowedShipsOfLength2));
					OnPropertyChanged(nameof(AllowedShipsOfLength3));
					OnPropertyChanged(nameof(AllowedShipsOfLength4));
					OnPropertyChanged(nameof(AllowedShipsOfLength5));
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}

			return true;
		}
	}
}
