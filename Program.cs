class Program
{

	static void Main(string[] args)
	{
		Console.CursorVisible = false;

		Random rng = new Random();

		Node2D rootNode = new Node2D("RootNode");
		rootNode.RootNode.Init();

		Game game = new Game("Game", rng);
		rootNode.AddChild(game);

		rootNode.Init();
		rootNode.Ready();

		while (true)
		{
			ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);

			if (consoleKeyInfo.KeyChar == 'q')
			{
				break;
			}
			else
			{
				game.Tick(consoleKeyInfo);
				game.Draw();
			}
		}
	}

}

