class Program
{

	static void Main(string[] args)
	{
		Console.CursorVisible = false;

		Input input = new Input();
		Random rng = new Random();
		Game game = new Game(rng);

		RootNode.Instance.AddChild(game);
		RootNode.Instance.Init();
		RootNode.Instance.Ready();

		while (!RootNode.Instance.IsQuit)
		{
			ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
			input.SetLastConsoleKeyInfo(consoleKeyInfo);
			game.Tick();
			game.Draw();
		}
	}

}

