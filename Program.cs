class Program
{

	static void Main(string[] args)
	{
		Console.CursorVisible = false;

		int screenX = 0;
		int screenY = 0;
		int screenW = 63;
		int screenH = 31;

		int playerX = 1;
		int playerY = 1;

		if (args.Length == 4)
		{
			screenW = int.Parse(args[0]);
			screenH = int.Parse(args[1]);
			playerX = int.Parse(args[2]);
			playerY = int.Parse(args[3]);
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
		int gameWindowH = (int)MathF.Ceiling((screenH / 4f) * 3);

		Screen floorsScreen = new Screen("FloorsScreen", gameWindowX, gameWindowY, gameWindowW, gameWindowH);
		Screen wallsScreen = new Screen("WallsScreen", gameWindowX, gameWindowY, gameWindowW, gameWindowH);
		Screen actorsScreen = new Screen("ActorsScreen", gameWindowX, gameWindowY, gameWindowW, gameWindowH);
		Screen playerScreen = new Screen("PlayerScreen", gameWindowX, gameWindowY, gameWindowW, gameWindowH);

		gameScreens.AddChild(floorsScreen);
		gameScreens.AddChild(wallsScreen);
		gameScreens.AddChild(actorsScreen);
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

		Camera camera = new Camera("Camera", gameWindowX, gameWindowY, gameWindowW, gameWindowH);

		rootNode.AddChild(camera);

		//

		Game game = new Game("Game", rng);

		rootNode.AddChild(game);

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

