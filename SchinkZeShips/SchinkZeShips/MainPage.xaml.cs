using SchinkZeShips.Core;

namespace SchinkZeShips
{
	public partial class MainPage
	{
		public MainPage()
		{
			InitializeComponent();
			TestServerAccess();
		}

		public async void TestServerAccess()
		{
			var service = new GameLogicService();
			var games = await service.GetAllGames();
			MainLabel.Text = $"Game count: {games.Count}";
		}
	}
}