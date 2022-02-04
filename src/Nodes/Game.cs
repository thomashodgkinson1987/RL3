public class Game : Node2D
{

	#region Nodes

	private Node2D node_world = new Node2D();

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

	private int m_boundsLeft;
	private int m_boundsRight;
	private int m_boundsUp;
	private int m_boundsDown;

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

		node_world = RootNode.GetNode<Node2D>("World");

		node_floors = node_world.GetNode<Node2D>("Floors");
		node_walls = node_world.GetNode<Node2D>("Walls");
		node_player = node_world.GetNode<Player>("Player");

		node_screens = RootNode.GetNode<ScreenGroup>("Screens");

		node_screen = node_screens.GetNode<Screen>("Screen");

		node_gameScreens = node_screens.GetNode<ScreenGroup>("GameScreens");

		node_floorsScreen = node_gameScreens.GetNode<Screen>("FloorsScreen");
		node_wallsScreen = node_gameScreens.GetNode<Screen>("WallsScreen");
		node_playerScreen = node_gameScreens.GetNode<Screen>("PlayerScreen");

		node_uiScreens = node_screens.GetNode<ScreenGroup>("UIScreens");

		node_uiScreen = node_uiScreens.GetNode<Screen>("UIScreen");

		node_camera = RootNode.GetNode<Camera>("Camera");

		m_boundsLeft = node_playerScreen.Position.X;
		m_boundsRight = node_playerScreen.Position.X + node_playerScreen.Width - 1;
		m_boundsUp = node_playerScreen.Position.Y;
		m_boundsDown = node_playerScreen.Position.Y + node_playerScreen.Height - 1;

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

			m_boundsLeft = args.Node.Position.X < m_boundsLeft ? args.Node.Position.X : m_boundsLeft;
			m_boundsRight = args.Node.Position.X > m_boundsRight ? args.Node.Position.X : m_boundsRight;
			m_boundsUp = args.Node.Position.Y < m_boundsUp ? args.Node.Position.Y : m_boundsUp;
			m_boundsDown = args.Node.Position.Y > m_boundsDown ? args.Node.Position.Y : m_boundsDown;
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

		for (int i = -node_floorsScreen.Height; i < node_floorsScreen.Height * 2; i++)
		{
			for (int j = -node_floorsScreen.Width; j < node_floorsScreen.Width * 2; j++)
			{
				if (m_rng.Next(0, 100) < 95)
				{
					AddFloor(j, i);
				}
			}
		}

		for (int i = -node_wallsScreen.Height; i < node_wallsScreen.Height * 2; i++)
		{
			for (int j = -node_wallsScreen.Width; j < node_wallsScreen.Width * 2; j++)
			{
				if (m_rng.Next(0, 100) < 10) AddWall(j, i);
			}
		}

		Console.Clear();
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

	public void AddFloor(int x, int y)
	{
		GameObject floor = new GameObject("Floor", x, y, '.');
		node_floors.AddChild(floor);
	}

	public void AddWall(int x, int y)
	{
		GameObject wall = new GameObject("Wall", x, y, '#');
		node_walls.AddChild(wall);
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

		int x = node_camera.Position.X;
		int y = node_camera.Position.Y;
		int w = node_camera.Width - 1;
		int h = node_camera.Height - 1;

		if (x < m_boundsLeft)
		{
			x = m_boundsLeft;
		}
		else if (x + w > m_boundsRight)
		{
			x = m_boundsRight - w;
		}

		if (y < m_boundsUp)
		{
			y = m_boundsUp;
		}
		else if (y + h > m_boundsDown)
		{
			y = m_boundsDown - h;
		}

		node_camera.SetPosition(x, y);
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
	}

	#endregion // Private methods

}

