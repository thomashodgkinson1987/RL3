public class Game : Node2D
{

	#region Nodes

	private Map node_map = new Map();

	private Node2D node_floors = new Node2D();
	private Node2D node_walls = new Node2D();

	private Player node_player = new Player();

	private ScreenGroup node_screens = new ScreenGroup();

	private Screen node_screen = new Screen();

	private ScreenGroup node_gameScreens = new ScreenGroup();

	private Screen node_floorsScreen = new Screen();
	private Screen node_wallsScreen = new Screen();
	private Screen node_playerScreen = new Screen();

	private ScreenGroup node_uiScreens = new ScreenGroup();

	private Screen node_uiScreen = new Screen();

	private Camera node_camera = new Camera();

	#endregion // Nodes



	#region Fields

	private Random m_rng;

	#endregion // Fields



	#region Constructors

	public Game(string name, int x, int y, Random rng) : base(name, x, y)
	{
		m_rng = rng;
	}

	public Game(string name, Random rng) : this(name, 0, 0, rng) { }

	public Game(int x, int y, Random rng) : this("Game", x, y, rng) { }

	public Game(Random rng) : this("Game", 0, 0, rng) { }

	#endregion // Constructors



	#region Node2D methods

	public override void Init()
	{
		base.Init();

		node_map = RootNode.GetNode<Map>("Map");

		node_floors = node_map.GetNode<Node2D>("Floors");
		node_walls = node_map.GetNode<Node2D>("Walls");

		node_player = RootNode.GetNode<Player>("Player");

		node_screens = RootNode.GetNode<ScreenGroup>("Screens");

		node_screen = node_screens.GetNode<Screen>("Screen");

		node_gameScreens = node_screens.GetNode<ScreenGroup>("GameScreens");

		node_floorsScreen = node_gameScreens.GetNode<Screen>("FloorsScreen");
		node_wallsScreen = node_gameScreens.GetNode<Screen>("WallsScreen");
		node_playerScreen = node_gameScreens.GetNode<Screen>("PlayerScreen");

		node_uiScreens = node_screens.GetNode<ScreenGroup>("UIScreens");

		node_uiScreen = node_uiScreens.GetNode<Screen>("UIScreen");

		node_camera = RootNode.GetNode<Camera>("Camera");

		node_map.GameObjectAdded += delegate (object? sender, GameObjectAddedEventArgs args)
		{
		};

		node_map.GameObjectRemoved += delegate (object? sender, GameObjectRemovedEventArgs args)
		{
		};

		node_player.HealthChanged += delegate (object? sender, IntChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_uiScreen.IsDirty = true;
		};

		node_player.MaxHealthChanged += delegate (object? sender, IntChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_uiScreen.IsDirty = true;
		};

		node_player.PositionChanged += delegate (object? sender, PointChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_playerScreen.IsDirty = true;
			node_uiScreen.IsDirty = true;
		};

		node_floors.PositionChanged += delegate (object? sender, PointChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
		};

		node_floors.NodeAdded += delegate (object? sender, NodeAddedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;

			int x = args.Node.Position.X;
			int y = args.Node.Position.Y;

			Bounds bounds = node_camera.Bounds;

			bounds.Left = x < bounds.Left ? x : bounds.Left;
			bounds.Right = x > bounds.Right ? x : bounds.Right;
			bounds.Up = y < bounds.Up ? y : bounds.Up;
			bounds.Down = y > bounds.Down ? y : bounds.Down;

			node_camera.Bounds = bounds;
		};

		node_floors.NodeRemoved += delegate (object? sender, NodeRemovedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
		};

		node_walls.PositionChanged += delegate (object? sender, PointChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
		};

		node_walls.NodeAdded += delegate (object? sender, NodeAddedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
		};

		node_walls.NodeRemoved += delegate (object? sender, NodeRemovedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
		};

		node_camera.PositionChanged += delegate (object? sender, PointChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
			node_playerScreen.IsDirty = true;
			node_uiScreen.IsDirty = true;
		};

		for (int i = 0; i < node_map.Height; i++)
		{
			for (int j = 0; j < node_map.Width; j++)
			{
				if (m_rng.Next(0, 100) < 95)
				{
					node_map.AddFloor(j, i);
				}
			}
		}

		for (int i = 0; i < node_map.Height; i++)
		{
			for (int j = 0; j < node_map.Width; j++)
			{
				if (!node_map.Floors.Any(_ => _.Position.X == j && _.Position.Y == i)) continue;

				if (m_rng.Next(0, 100) < 10)
				{
					node_map.AddWall(j, i);
				}
			}
		}

		Console.Clear();
		TickCamera();
		Draw();
	}

	#endregion // Node2D methods



	#region Public methods

	public void Tick(ConsoleKeyInfo consoleKeyInfo)
	{
		node_player.Tick(consoleKeyInfo, this);
		TickCamera();
	}

	public void Draw()
	{
		if (node_screen.IsDirty)
		{
			node_screen.IsDirty = false;
			node_screen.Clear();
		}

		if (node_floorsScreen.IsDirty)
		{
			node_floorsScreen.IsDirty = false;
			node_floorsScreen.Clear();
			Point floorsGlobalPosition = node_floors.GlobalPosition - node_camera.GlobalPosition;
			foreach (GameObject floor in node_floors.Children)
			{
				if (!floor.IsVisible) continue;

				Point floorGlobalPosition = floorsGlobalPosition + floor.Position;
				if (node_floorsScreen.IsPositionOnScreen(floorGlobalPosition))
				{
					node_floorsScreen.SetSymbol(floorGlobalPosition, floor.Symbol);
				}
			}
		}

		if (node_wallsScreen.IsDirty)
		{
			node_wallsScreen.IsDirty = false;
			node_wallsScreen.Clear();
			Point wallsGlobalPosition = node_walls.GlobalPosition - node_camera.GlobalPosition;
			foreach (GameObject wall in node_walls.Children)
			{
				if (!wall.IsVisible) continue;

				Point wallGlobalPosition = wallsGlobalPosition + wall.Position;
				if (node_wallsScreen.IsPositionOnScreen(wallGlobalPosition))
				{
					node_wallsScreen.SetSymbol(wallGlobalPosition, wall.Symbol);
				}
			}
		}

		if (node_playerScreen.IsDirty)
		{
			node_playerScreen.IsDirty = false;
			node_playerScreen.Clear();
			if (node_player.IsVisible)
			{
				Point playerGlobalPosition = node_player.GlobalPosition - node_camera.GlobalPosition;
				if (node_playerScreen.IsPositionOnScreen(playerGlobalPosition))
				{
					node_playerScreen.SetSymbol(playerGlobalPosition, node_player.Symbol);
				}
			}
		}

		if (node_uiScreen.IsDirty)
		{
			node_uiScreen.IsDirty = false;
			UpdateUI();
		}

		if (node_screens.IsVisible && node_screen.IsVisible)
		{
			DrawScreenGroup(node_gameScreens, node_screen);
			DrawScreenGroup(node_uiScreens, node_screen);
		}

		Console.SetCursorPosition(0, 0);
		Console.Write(node_screen.ToString());
	}

	public bool IsClear(Point position)
	{
		Point floorsGlobalPosition = node_floors.GlobalPosition;
		List<Node2D> floors = new List<Node2D>();
		for (int i = 0; i < node_floors.Children.Count; i++)
		{
			Node2D floor = node_floors.Children[i];

			Point floorGlobalPosition = floor.Position + floorsGlobalPosition;

			if (position == floorGlobalPosition)
			{
				floors.Add(floor);
			}
		}
		bool isFloor = floors.Count > 0;

		Point wallsGlobalPosition = node_walls.GlobalPosition;
		List<Node2D> walls = new List<Node2D>();
		for (int i = 0; i < node_walls.Children.Count; i++)
		{
			Node2D wall = node_walls.Children[i];

			Point wallGlobalPosition = wall.Position + wallsGlobalPosition;

			if (position == wallGlobalPosition)
			{
				walls.Add(wall);
			}
		}
		bool isWall = walls.Count > 0;

		return isFloor && !isWall;
	}

	public bool IsClear(int x, int y)
	{
		return IsClear(new Point(x, y));
	}

	#endregion // Public methods



	#region Private methods

	private void DrawScreen(Screen screen, Screen targetScreen)
	{
		if (!screen.IsVisible) return;

		Point screenGlobalPosition = screen.GlobalPosition;
		for (int i = 0; i < screen.Height; i++)
		{
			for (int j = 0; j < screen.Width; j++)
			{
				int x = j + screenGlobalPosition.X;
				int y = i + screenGlobalPosition.Y;

				if (x < 0 || x >= targetScreen.Width) continue;
				if (y < 0 || y >= targetScreen.Height) continue;

				char symbol = screen.GetSymbol(j, i);

				if (symbol != ' ' || !screen.IsSpaceTransparent)
				{
					targetScreen.SetSymbol(x, y, symbol);
				}
			}
		}
	}

	private void DrawScreenGroup(ScreenGroup screenGroup, Screen targetScreen)
	{
		if (screenGroup.IsVisible)
		{
			foreach (Screen screen in screenGroup.Screens)
			{
				DrawScreen(screen, targetScreen);
			}
		}
	}

	private void TickCamera()
	{
		node_camera.CenterOnPosition(node_player.GlobalPosition);

		Bounds bounds = new Bounds(0, node_map.Width, 0, node_map.Height);

		node_camera.Bounds = bounds;

		if (node_camera.Width < node_map.Width)
		{
			if (node_camera.Position.X < node_camera.Bounds.Left)
			{
				node_camera.SetPositionX(node_camera.Bounds.Left);
			}
			else if (node_camera.Position.X + node_camera.Width > node_camera.Bounds.Right)
			{
				node_camera.SetPositionX(node_camera.Bounds.Right - node_camera.Width);
			}
		}
		else
		{
			node_camera.CenterOnPositionX((int)(node_map.Width / 2f));
		}

		if (node_camera.Height < node_map.Height)
		{
			if (node_camera.Position.Y < node_camera.Bounds.Up)
			{
				node_camera.SetPositionY(node_camera.Bounds.Up);
			}
			else if (node_camera.Position.Y + node_camera.Height > node_camera.Bounds.Down)
			{
				node_camera.SetPositionY(node_camera.Bounds.Down - node_camera.Height);
			}
		}
		else
		{
			node_camera.CenterOnPositionY((int)(node_map.Height / 2f));
		}
	}

	private void UpdateUI()
	{
		node_uiScreen.Clear();

		node_uiScreen.FillRow(0, '─');
		node_uiScreen.FillRow(node_uiScreen.Height - 1, '─');
		node_uiScreen.FillColumn(0, '│');
		node_uiScreen.FillColumn(node_uiScreen.Width - 1, '│');
		node_uiScreen.SetSymbol(0, 0, '┌');
		node_uiScreen.SetSymbol(node_uiScreen.Width - 1, 0, '┐');
		node_uiScreen.SetSymbol(0, node_uiScreen.Height - 1, '└');
		node_uiScreen.SetSymbol(node_uiScreen.Width - 1, node_uiScreen.Height - 1, '┘');

		node_uiScreen.DrawText(1, 1, Screen.EDirection.Right, $"Player position: x={node_player.Position.X} y={node_player.Position.Y} gx={node_player.GlobalPosition.X} gy={node_player.GlobalPosition.Y}");
		node_uiScreen.DrawText(1, 2, Screen.EDirection.Right, $"Camera position: x={node_camera.Position.X} y={node_camera.Position.Y} w={node_camera.Width} h={node_camera.Height}");
		node_uiScreen.DrawText(1, 3, Screen.EDirection.Right, $"Map: width={node_map.Width} height={node_map.Height}");
	}

	#endregion // Private methods

}

