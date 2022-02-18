class Program
{

	static void Main(string[] args)
	{
		Console.CursorVisible = false;

		Time time = new Time(1 / 60f);
		Input input = new Input();
		Random rng = new Random();
		Game game = new Game(rng);

		RootNode.Instance.AddChild(game);
		RootNode.Instance.Init();
		RootNode.Instance.Ready();

		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

		float timeStepRemaining = 0;

		while (!RootNode.Instance.IsQuit)
		{
			time.SetDelta((float)sw.Elapsed.TotalMilliseconds / 1000f);
			time.SetElapsed(Time.Elapsed + Time.Delta);
			timeStepRemaining += Time.Delta;

			sw.Reset();
			sw.Start();

			if (Console.KeyAvailable)
			{
				ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
				input.SetLastConsoleKeyInfo(consoleKeyInfo);
				game.ProcessInput(consoleKeyInfo);
				game.InputTick(Time.Delta);
			}

			while (timeStepRemaining > Time.FixedTimeStep)
			{
				timeStepRemaining -= Time.FixedTimeStep;
				game.FixedTick(Time.FixedTimeStep);
			}

			game.Tick(Time.Delta);
			game.Draw();
		}
	}

}

