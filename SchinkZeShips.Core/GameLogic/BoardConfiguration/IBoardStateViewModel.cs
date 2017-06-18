using System.Collections.Generic;

namespace SchinkZeShips.Core.GameLogic.BoardConfiguration
{
	public interface IBoardStateViewModel
	{
		int AllowedBattleships { get; }
		int AllowedCruisers { get; }
		int AllowedDestroyer { get; }
		int AllowedSubmarines { get; }

		bool TryAddShip(List<Coordinate> shipToAdd);
	}
}