class Program
{

	static void Main(string[] args)
	{
		Console.CursorVisible = false;

		int screenX = 0;
		int screenY = 0;
		int screenW = 64;
		int screenH = 32;

		int playerX = 1;
		int playerY = 1;

		if (args.Length == 4)
		{
			screenW = int.Parse(args[0]);
			screenH = int.Parse(args[1]);
			playerX = int.Parse(args[2]);
			playerY = int.Parse(args[3]);
		}
		else if (args.Length == 6)
		{
			screenX = int.Parse(args[0]);
			screenY = int.Parse(args[1]);
			screenW = int.Parse(args[2]);
			screenH = int.Parse(args[3]);
			playerX = int.Parse(args[4]);
			playerY = int.Parse(args[5]);
		}

		Random rng = new Random();

		//

		Node2D rootNode = new Node2D("RootNode");

		rootNode.RootNode.Init();

		//

		MapGroup mapGroup = new MapGroup("Maps");

		rootNode.AddChild(mapGroup);

		//

		Player player = new Player(playerX, playerY, 10);

		rootNode.AddChild(player);

		//

		ScreenGroup screens = new ScreenGroup("Screens");

		rootNode.AddChild(screens);

		//

		Screen screen = new Screen("Screen", screenX, screenY, screenW, screenH);

		screens.AddChild(screen);

		//

		ScreenGroup gameScreens = new ScreenGroup("GameScreens");

		int gameWindowX = screenX;
		int gameWindowY = screenY;
		int gameWindowW = screenW;
		int gameWindowH = (int)MathF.Floor((screenH / 4f) * 3);

		Screen floorsScreen = new Screen("FloorsScreen", gameWindowX, gameWindowY, gameWindowW, gameWindowH);
		Screen wallsScreen = new Screen("WallsScreen", gameWindowX, gameWindowY, gameWindowW, gameWindowH);
		Screen playerScreen = new Screen("PlayerScreen", gameWindowX, gameWindowY, gameWindowW, gameWindowH);

		gameScreens.AddChild(floorsScreen);
		gameScreens.AddChild(wallsScreen);
		gameScreens.AddChild(playerScreen);

		screens.AddChild(gameScreens);

		//

		ScreenGroup uiScreens = new ScreenGroup("UIScreens");

		int uiWindowX = screenX;
		int uiWindowY = screenY + (int)MathF.Ceiling((screenH / 4f) * 3);
		int uiWindowW = screenW;
		int uiWindowH = (int)MathF.Floor(screenH / 4f);

		Screen uiScreen = new Screen("UIScreen", uiWindowX, uiWindowY, uiWindowW, uiWindowH);
		uiScreen.IsSpaceTransparent = false;

		uiScreens.AddChild(uiScreen);

		screens.AddChild(uiScreens);

		//

		Game game = new Game("Game", rng);

		rootNode.AddChild(game);

		//

		Camera camera = new Camera("Camera", gameWindowX, gameWindowY, gameWindowW, gameWindowH);

		rootNode.AddChild(camera);

		//

		rootNode.Init();

		//

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

