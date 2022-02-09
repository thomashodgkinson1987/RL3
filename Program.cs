class Program
{

	static void Main(string[] args)
	{
		Console.CursorVisible = false;

		Random rng = new Random();

		Game game = new Game("Game", rng);
		RootNode.Instance.AddChild(game);

		RootNode.Instance.Init();
		RootNode.Instance.Ready();

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

