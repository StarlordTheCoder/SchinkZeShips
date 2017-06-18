using System;
using SchinkZeShips.Core.SchinkZeShipsReference;

namespace SchinkZeShips.Core.GameLogic
{
	public partial class CellView
	{
		public CellView()
		{
			InitializeComponent();
		}

		private void Cell_Tapped(object sender, EventArgs e)
		{
			var cell = (CellState)BindingContext;
			cell.IsSelected = !cell.IsSelected;
		}
	}
}