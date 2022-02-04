public class Game : Node2D
{

	#region Fields

	private Node2D m_world = new Node2D();

	private Node2D m_floors = new Node2D();
	private Node2D m_walls = new Node2D();
	private Player m_player = new Player();

	private ScreenGroup m_screens = new ScreenGroup();

	private Screen m_screen = new Screen();

	private ScreenGroup m_gameScreens = new ScreenGroup();

	private Screen m_floorsScreen = new Screen();
	private Screen m_wallsScreen = new Screen();
	private Screen m_playerScreen = new Screen();

	private ScreenGroup m_uiScreens = new ScreenGroup();

	private Screen m_uiScreen = new Screen();

	private Camera m_camera = new Camera();

	private int m_boundsLeft;
	private int m_boundsRight;
	private int m_boundsUp;
	private int m_boundsDown;

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

		m_world = RootNode.GetNode<Node2D>("World");

		m_floors = m_world.GetNode<Node2D>("Floors");
		m_walls = m_world.GetNode<Node2D>("Walls");
		m_player = m_world.GetNode<Player>("Player");

		m_screens = RootNode.GetNode<ScreenGroup>("Screens");

		m_screen = m_screens.GetNode<Screen>("Screen");

		m_gameScreens = m_screens.GetNode<ScreenGroup>("GameScreens");

		m_floorsScreen = m_gameScreens.GetNode<Screen>("FloorsScreen");
		m_wallsScreen = m_gameScreens.GetNode<Screen>("WallsScreen");
		m_playerScreen = m_gameScreens.GetNode<Screen>("PlayerScreen");

		m_uiScreens = m_screens.GetNode<ScreenGroup>("UIScreens");

		m_uiScreen = m_uiScreens.GetNode<Screen>("UIScreen");

		m_camera = RootNode.GetNode<Camera>("Camera");

		m_boundsLeft = m_playerScreen.Position.X;
		m_boundsRight = m_playerScreen.Position.X + m_playerScreen.Width - 1;
		m_boundsUp = m_playerScreen.Position.Y;
		m_boundsDown = m_playerScreen.Position.Y + m_playerScreen.Height - 1;

		m_player.HealthChanged += delegate (object? sender, IntChangedEventArgs args)
		{
			m_screen.IsDirty = true;
			m_uiScreen.IsDirty = true;
		};

		m_player.MaxHealthChanged += delegate (object? sender, IntChangedEventArgs args)
		{
			m_screen.IsDirty = true;
			m_uiScreen.IsDirty = true;
		};

		m_player.PositionChanged += delegate (object? sender, PointChangedEventArgs args)
		{
			m_screen.IsDirty = true;
			m_playerScreen.IsDirty = true;
			m_uiScreen.IsDirty = true;
		};

		m_floors.PositionChanged += delegate (object? sender, PointChangedEventArgs args)
		{
			m_screen.IsDirty = true;
			m_floorsScreen.IsDirty = true;
		};

		m_floors.NodeAdded += delegate (object? sender, NodeAddedEventArgs args)
		{
			m_screen.IsDirty = true;
			m_floorsScreen.IsDirty = true;

			if (args.Node.Position.X < m_boundsLeft)
			{
				m_boundsLeft = args.Node.Position.X;
			}
			else if (args.Node.Position.X > m_boundsRight)
			{
				m_boundsRight = args.Node.Position.X;
			}

			if (args.Node.Position.Y < m_boundsUp)
			{
				m_boundsUp = args.Node.Position.Y;
			}
			else if (args.Node.Position.Y > m_boundsDown)
			{
				m_boundsDown = args.Node.Position.Y;
			}
		};

		m_floors.NodeRemoved += delegate (object? sender, NodeRemovedEventArgs args)
		{
			m_screen.IsDirty = true;
			m_floorsScreen.IsDirty = true;
		};

		m_walls.PositionChanged += delegate (object? sender, PointChangedEventArgs args)
		{
			m_screen.IsDirty = true;
			m_wallsScreen.IsDirty = true;
		};

		m_walls.NodeAdded += delegate (object? sender, NodeAddedEventArgs args)
		{
			m_screen.IsDirty = true;
			m_wallsScreen.IsDirty = true;
		};

		m_walls.NodeRemoved += delegate (object? sender, NodeRemovedEventArgs args)
		{
			m_screen.IsDirty = true;
			m_wallsScreen.IsDirty = true;
		};

		m_camera.PositionChanged += delegate (object? sender, PointChangedEventArgs args)
		{
			m_screen.IsDirty = true;
			m_floorsScreen.IsDirty = true;
			m_wallsScreen.IsDirty = true;
			m_playerScreen.IsDirty = true;
			m_uiScreen.IsDirty = true;
		};

		for (int i = -m_floorsScreen.Height; i < m_floorsScreen.Height * 2; i++)
		{
			for (int j = -m_floorsScreen.Width; j < m_floorsScreen.Width * 2; j++)
			{
				if (m_rng.Next(0, 100) < 95)
				{
					AddFloor(j, i);
				}
			}
		}

		for (int i = -m_wallsScreen.Height; i < m_wallsScreen.Height * 2; i++)
		{
			for (int j = -m_wallsScreen.Width; j < m_wallsScreen.Width * 2; j++)
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
		if (consoleKeyInfo.KeyChar == 'w')
		{
			m_screen.IsVisible = !m_screen.IsVisible;
		}
		else if (consoleKeyInfo.KeyChar == 'e')
		{
			m_floorsScreen.IsVisible = !m_floorsScreen.IsVisible;
		}
		else if (consoleKeyInfo.KeyChar == 'r')
		{
			m_wallsScreen.IsVisible = !m_wallsScreen.IsVisible;
		}
		else if (consoleKeyInfo.KeyChar == 't')
		{
			m_playerScreen.IsVisible = !m_playerScreen.IsVisible;
		}
		else if (consoleKeyInfo.KeyChar == 'y')
		{
			m_uiScreen.IsVisible = !m_uiScreen.IsVisible;
		}

		if (consoleKeyInfo.KeyChar == 'u')
		{
			m_camera.TranslateX(1);
		}
		else if (consoleKeyInfo.KeyChar == 'i')
		{
			m_camera.TranslateX(-1);
		}
		else if (consoleKeyInfo.KeyChar == 'o')
		{
			m_camera.TranslateY(1);
		}
		else if (consoleKeyInfo.KeyChar == 'p')
		{
			m_camera.TranslateY(-1);
		}

		if (consoleKeyInfo.KeyChar == 'a')
		{
			m_floors.TranslateX(-1);
		}
		else if (consoleKeyInfo.KeyChar == 's')
		{
			m_floors.TranslateX(1);
		}
		else if (consoleKeyInfo.KeyChar == 'd')
		{
			m_floors.TranslateY(-1);
		}
		else if (consoleKeyInfo.KeyChar == 'f')
		{
			m_floors.TranslateY(1);
		}

		if (consoleKeyInfo.KeyChar == 'g')
		{
			m_walls.TranslateX(-1);
		}
		else if (consoleKeyInfo.KeyChar == 'h')
		{
			m_walls.TranslateX(1);
		}
		else if (consoleKeyInfo.KeyChar == 'j')
		{
			m_walls.TranslateY(-1);
		}
		else if (consoleKeyInfo.KeyChar == 'k')
		{
			m_walls.TranslateY(1);
		}

		if (consoleKeyInfo.KeyChar == 'z')
		{
			m_uiScreen.TranslateX(-1);
		}
		else if (consoleKeyInfo.KeyChar == 'x')
		{
			m_uiScreen.TranslateX(1);
		}
		else if (consoleKeyInfo.KeyChar == 'c')
		{
			m_uiScreen.TranslateY(-1);
		}
		else if (consoleKeyInfo.KeyChar == 'v')
		{
			m_uiScreen.TranslateY(1);
		}

		if (consoleKeyInfo.KeyChar == 'l')
		{
			int c = m_rng.Next(0, 2);
			Node2D node = c == 0 ? m_floors : m_walls;
			for (int i = 0; i < node.Children.Count; i++)
			{
				if (m_rng.Next(0, 100) < 20)
				{
					node.RemoveChild(i);
				}
			}
		}

		if (consoleKeyInfo.KeyChar == 'b')
		{
			m_player.Health = m_rng.Next(0, 100);
		}
		else if (consoleKeyInfo.KeyChar == 'n')
		{
			m_player.MaxHealth = m_rng.Next(0, 100);
		}
		else if (consoleKeyInfo.KeyChar == 'm')
		{
			m_player.MaxHealth = 99;
			m_player.Health = 99;
		}

		TickPlayer(consoleKeyInfo);

		m_camera.CenterOnPosition(m_player.GlobalPosition);


		int x = m_camera.Position.X;
		int y = m_camera.Position.Y;
		int w = m_camera.Width - 1;
		int h = m_camera.Height - 1;

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

		m_camera.SetPosition(x, y);
	}

	public void Draw()
	{
		if (m_screen.IsDirty)
		{
			m_screen.IsDirty = false;
			m_screen.Clear();
		}

		if (m_floorsScreen.IsDirty)
		{
			m_floorsScreen.IsDirty = false;
			m_floorsScreen.Clear();
			Point floorsGlobalPosition = m_floors.GlobalPosition - m_camera.GlobalPosition;
			foreach (GameObject floor in m_floors.Children)
			{
				if (!floor.IsVisible) continue;

				Point floorGlobalPosition = floorsGlobalPosition + floor.Position;
				if (m_floorsScreen.IsPositionOnScreen(floorGlobalPosition))
				{
					m_floorsScreen.SetSymbol(floorGlobalPosition, floor.Symbol);
				}
			}
		}

		if (m_wallsScreen.IsDirty)
		{
			m_wallsScreen.IsDirty = false;
			m_wallsScreen.Clear();
			Point wallsGlobalPosition = m_walls.GlobalPosition - m_camera.GlobalPosition;
			foreach (GameObject wall in m_walls.Children)
			{
				if (!wall.IsVisible) continue;

				Point wallGlobalPosition = wallsGlobalPosition + wall.Position;
				if (m_wallsScreen.IsPositionOnScreen(wallGlobalPosition))
				{
					m_wallsScreen.SetSymbol(wallGlobalPosition, wall.Symbol);
				}
			}
		}

		if (m_playerScreen.IsDirty)
		{
			m_playerScreen.IsDirty = false;
			m_playerScreen.Clear();
			if (m_player.IsVisible)
			{
				Point playerGlobalPosition = m_player.GlobalPosition - m_camera.GlobalPosition;
				if (m_playerScreen.IsPositionOnScreen(playerGlobalPosition))
				{
					m_playerScreen.SetSymbol(playerGlobalPosition, m_player.Symbol);
				}
			}
		}

		if (m_uiScreen.IsDirty)
		{
			m_uiScreen.IsDirty = false;
			UpdateUI();
		}

		if (m_screens.IsVisible && m_screen.IsVisible)
		{
			if (m_gameScreens.IsVisible)
			{
				foreach (Screen screen in m_gameScreens.Screens)
				{
					if (!screen.IsVisible) continue;

					Point screenGlobalPosition = screen.GlobalPosition;
					for (int i = 0; i < screen.Height; i++)
					{
						for (int j = 0; j < screen.Width; j++)
						{
							int x = j + screenGlobalPosition.X;
							int y = i + screenGlobalPosition.Y;

							if (x < 0 || x >= m_screen.Width) continue;
							if (y < 0 || y >= m_screen.Height) continue;

							char symbol = screen.GetSymbol(j, i);

							if (symbol != ' ' || !screen.IsSpaceTransparent)
							{
								m_screen.SetSymbol(x, y, symbol);
							}
						}
					}
				}
			}
			if (m_uiScreens.IsVisible)
			{
				foreach (Screen screen in m_uiScreens.Screens)
				{
					if (!screen.IsVisible) continue;

					for (int i = 0; i < screen.Height; i++)
					{
						for (int j = 0; j < screen.Width; j++)
						{
							int x = j + screen.GlobalPosition.X;
							int y = i + screen.GlobalPosition.Y;

							if (x < 0 || x >= m_screen.Width) continue;
							if (y < 0 || y >= m_screen.Height) continue;

							char symbol = screen.GetSymbol(j, i);

							if (symbol != ' ' || !screen.IsSpaceTransparent)
							{
								m_screen.SetSymbol(x, y, symbol);
							}
						}
					}
				}
			}
		}

		Console.SetCursorPosition(0, 0);
		Console.Write(m_screen.ToString());
	}

	public void AddFloor(int x, int y)
	{
		GameObject floor = new GameObject("Floor", x, y, '.');
		m_floors.AddChild(floor);
	}

	public void AddWall(int x, int y)
	{
		GameObject wall = new GameObject("Wall", x, y, '#');
		m_walls.AddChild(wall);
	}

	#endregion // Public methods



	#region Private methods

	private void TickPlayer(ConsoleKeyInfo i)
	{
		Point position = m_player.Position;
		Point globalPosition = m_player.GlobalPosition;

		if (i.KeyChar == '4')
		{
			if (IsClear(globalPosition.X - 1, globalPosition.Y))
			{
				position.X--;
			}
		}
		else if (i.KeyChar == '6')
		{
			if (IsClear(globalPosition.X + 1, globalPosition.Y))
			{
				position.X++;
			}
		}
		else if (i.KeyChar == '8')
		{
			if (IsClear(globalPosition.X, globalPosition.Y - 1))
			{
				position.Y--;
			}
		}
		else if (i.KeyChar == '2')
		{
			if (IsClear(globalPosition.X, globalPosition.Y + 1))
			{
				position.Y++;
			}
		}
		else if (i.KeyChar == '7')
		{
			if (IsClear(globalPosition.X - 1, globalPosition.Y - 1))
			{
				position.X--;
				position.Y--;
			}
		}
		else if (i.KeyChar == '9')
		{
			if (IsClear(globalPosition.X + 1, globalPosition.Y - 1))
			{
				position.X++;
				position.Y--;
			}
		}
		else if (i.KeyChar == '1')
		{
			if (IsClear(globalPosition.X - 1, globalPosition.Y + 1))
			{
				position.X--;
				position.Y++;
			}
		}
		else if (i.KeyChar == '3')
		{
			if (IsClear(globalPosition.X + 1, globalPosition.Y + 1))
			{
				position.X++;
				position.Y++;
			}
		}

		m_player.SetPosition(position.X, position.Y);
	}

	private bool IsClear(Point position)
	{
		Point floorsGlobalPosition = m_floors.GlobalPosition;
		List<Node2D> floors = new List<Node2D>();
		for (int i = 0; i < m_floors.Children.Count; i++)
		{
			Node2D floor = m_floors.Children[i];

			Point floorGlobalPosition = floor.Position + floorsGlobalPosition;

			if (position == floorGlobalPosition)
			{
				floors.Add(floor);
			}
		}
		bool isFloor = floors.Count > 0;

		Point wallsGlobalPosition = m_walls.GlobalPosition;
		List<Node2D> walls = new List<Node2D>();
		for (int i = 0; i < m_walls.Children.Count; i++)
		{
			Node2D wall = m_walls.Children[i];

			Point wallGlobalPosition = wall.Position + wallsGlobalPosition;

			if (position == wallGlobalPosition)
			{
				walls.Add(wall);
			}
		}
		bool isWall = walls.Count > 0;

		return isFloor && !isWall;
	}

	private bool IsClear(int x, int y)
	{
		return IsClear(new Point(x, y));
	}

	private void UpdateUI()
	{
		m_uiScreen.Clear();

		m_uiScreen.FillRow(0, '─');
		m_uiScreen.FillRow(m_uiScreen.Height - 1, '─');
		m_uiScreen.FillColumn(0, '│');
		m_uiScreen.FillColumn(m_uiScreen.Width - 1, '│');
		m_uiScreen.SetSymbol(0, 0, '┌');
		m_uiScreen.SetSymbol(m_uiScreen.Width - 1, 0, '┐');
		m_uiScreen.SetSymbol(0, m_uiScreen.Height - 1, '└');
		m_uiScreen.SetSymbol(m_uiScreen.Width - 1, m_uiScreen.Height - 1, '┘');

		m_uiScreen.DrawText(1, 1, Screen.EDirection.Right, $"Player HP: {m_player.Health}/{m_player.MaxHealth}");
		m_uiScreen.DrawText(1, 2, Screen.EDirection.Right, $"Player position: x={m_player.Position.X} y={m_player.Position.Y} gx={m_player.GlobalPosition.X} gy={m_player.GlobalPosition.Y}");
		m_uiScreen.DrawText(1, 3, Screen.EDirection.Right, $"Camera position: x={m_camera.Position.X} y={m_camera.Position.Y} w={m_camera.Width} h={m_camera.Height}");
		m_uiScreen.DrawText(1, 4, Screen.EDirection.Right, $"Floors position: x={m_floors.Position.X} y={m_floors.Position.Y} gx={m_floors.GlobalPosition.X} gy={m_floors.GlobalPosition.Y}");
		m_uiScreen.DrawText(1, 5, Screen.EDirection.Right, $"Walls position: x={m_walls.Position.X} y={m_walls.Position.Y} gx={m_walls.GlobalPosition.X} gy={m_walls.GlobalPosition.Y}");
		m_uiScreen.DrawText(1, 6, Screen.EDirection.Right, $"UI position: x={m_uiScreen.Position.X} y={m_uiScreen.Position.Y} gx={m_uiScreen.GlobalPosition.X} gy={m_uiScreen.GlobalPosition.Y}");
	}

	#endregion // Private methods

}

