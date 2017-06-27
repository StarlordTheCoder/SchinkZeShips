using System;

namespace SchinkZeShips.Core.GameLogic.Board
{
	public partial class CellView
	{
		public CellView()
		{
			InitializeComponent();
		}

		private void Cell_Tapped(object sender, EventArgs e)
		{
			var cell = (CellViewModel) BindingContext;
			cell.IsSelected = !cell.IsSelected;
		}
	}
}