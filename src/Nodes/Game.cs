public class Game : Node2D
{

	#region Nodes

	private MapGroup node_maps = new MapGroup();

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

	private readonly Random m_rng;

	private Map m_currentMap;
	public (int x, int y) m_currentChunk;

	#endregion // Fields



	#region Constructors

	public Game(string name, int x, int y, Random rng) : base(name, x, y)
	{
		m_rng = rng;
		m_currentMap = new Map();
		m_currentChunk = (0, 0);
	}

	public Game(string name, Random rng) : this(name, 0, 0, rng) { }

	public Game(int x, int y, Random rng) : this("Game", x, y, rng) { }

	public Game(Random rng) : this("Game", 0, 0, rng) { }

	#endregion // Constructors



	#region Node2D methods

	public override void Init()
	{
		base.Init();

		node_maps = RootNode.GetNode<MapGroup>("Maps");

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

		node_camera.PositionChanged += delegate (object? sender, PointChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
			node_playerScreen.IsDirty = true;
			node_uiScreen.IsDirty = true;
		};

		for (int i = -8; i < 8; i++)
		{
			for(int j = -8; j < 8; j++)
			{
				Map map = node_maps.CreateMap(j, i);
				for(int k = 0; k < map.Height; k++)
				{
					for(int l = 0; l < map.Width; l++)
					{
						map.AddFloor(l, k);
						if (m_rng.Next(0, 500) < 2)
						{
							map.AddWall(l, k);
						}
					}
				}
			}
		}

		m_currentMap = node_maps.Maps[(0, 0)];
		m_currentChunk = (0, 0);

		for(int i = -node_maps.MapLoadDistance; i <= node_maps.MapLoadDistance; i++)
		{
			for(int j = -node_maps.MapLoadDistance; j <= node_maps.MapLoadDistance; j++)
			{
				node_maps.LoadMap(j, i);
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

		int oldChunkX = m_currentChunk.x;
		int oldChunkY = m_currentChunk.y;

		int newChunkX = (int)MathF.Floor(node_player.GlobalPosition.X / 16f);
		int newChunkY = (int)MathF.Floor(node_player.GlobalPosition.Y / 16f);

		if (newChunkX != m_currentChunk.x || newChunkY != m_currentChunk.y)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
			node_wallsScreen.IsDirty = true;

			m_currentChunk = (newChunkX, newChunkY);
			m_currentMap = node_maps.GetMap(m_currentChunk.x, m_currentChunk.y);

			List<Map> newMaps = new List<Map>();

			for(int i = -node_maps.MapLoadDistance; i <= node_maps.MapLoadDistance; i++)
			{
				for(int j = -node_maps.MapLoadDistance; j <= node_maps.MapLoadDistance; j++)
				{
					int x = m_currentChunk.x + j;
					int y = m_currentChunk.y + i;
					if (node_maps.Maps.ContainsKey((x, y)))
					{
						Map map = node_maps.GetMap(x, y);
						newMaps.Add(map);
						if (node_maps.InactiveMaps.Contains(map))
						{
							node_maps.LoadMap(map);
						}
					}
				}
			}

			for (int i = 0; i < node_maps.ActiveMaps.Count; i++)
			{
				if (!newMaps.Contains(node_maps.ActiveMaps[i]))
				{
					node_maps.UnloadMap(node_maps.ActiveMaps[i]);
					i--;
				}
			}

		}

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
			foreach(Map map in node_maps.ActiveMaps)
			{
				Point globalPosition = map.GlobalPosition - node_camera.GlobalPosition;
				foreach (GameObject floor in map.Floors)
				{
					if (!floor.IsVisible) continue;

					Point floorGlobalPosition = globalPosition + floor.Position;
					if (node_floorsScreen.IsPositionOnScreen(floorGlobalPosition))
					{
						node_floorsScreen.SetSymbol(floorGlobalPosition, floor.Symbol);
					}
				}
			}
		}

		if (node_wallsScreen.IsDirty)
		{
			node_wallsScreen.IsDirty = false;
			node_wallsScreen.Clear();
			foreach(Map map in node_maps.ActiveMaps)
			{
				Point globalPosition = map.GlobalPosition - node_camera.GlobalPosition;
				foreach (GameObject wall in map.Walls)
				{
					if (!wall.IsVisible) continue;

					Point wallGlobalPosition = globalPosition + wall.Position;
					if (node_wallsScreen.IsPositionOnScreen(wallGlobalPosition))
					{
						node_wallsScreen.SetSymbol(wallGlobalPosition, wall.Symbol);
					}
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
		bool isFloor = false;
		foreach(Map map in node_maps.ActiveMaps)
		{
			Point globalPosition = map.GlobalPosition;
			foreach(GameObject floor in map.Floors)
			{
				Point floorGlobalPosition = floor.Position + globalPosition;

				if (position == floorGlobalPosition)
				{
					isFloor = true;
					break;
				}
			}
			if (isFloor) break;
		}

		bool isWall = false;
		foreach(Map map in node_maps.ActiveMaps)
		{
			Point globalPosition = map.GlobalPosition;
			foreach(GameObject wall in map.Walls)
			{
				Point wallGlobalPosition = wall.Position + globalPosition;

				if (position == wallGlobalPosition)
				{
					isWall = true;
					break;
				}
				if (isWall) break;
			}
		}

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

		int left = node_maps.ActiveMaps.Min(_ => _.GlobalPosition.X);
		int right = node_maps.ActiveMaps.Max(_ => _.GlobalPosition.X) + m_currentMap.Width;
		int up = node_maps.ActiveMaps.Min(_ => _.GlobalPosition.Y);
		int down = node_maps.ActiveMaps.Max(_ => _.GlobalPosition.Y) + m_currentMap.Height;

		int w = right - (int)(MathF.Abs(left));
		int h = down - (int)(MathF.Abs(up));

		if (left < 0)
		{
			if (right < 0)
			{
				w = (int)MathF.Abs(left - right);
			}
			else
			{
				w = right + (int)MathF.Abs(left);
			}
		}
		else
		{
			w = right - left;
		}

		if (up < 0)
		{
			if (down < 0)
			{
				h = (int)MathF.Abs(up - down);
			}
			else
			{
				h = down + (int)MathF.Abs(up);
			}
		}
		else
		{
			w = down - up;
		}

		Bounds bounds = new Bounds(left, right, up, down);

		node_camera.Bounds = bounds;

		if (node_camera.Width < w)
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
			int x = 0;
			if (left < 0)
			{
				x = right + (int)MathF.Abs(left);
				x = (int)(x / 2f);
				x -= (int)MathF.Abs(left);
			}
			else
			{
				x = right - left;
				x = (int)(x / 2f);
				x += left;
			}
			node_camera.CenterOnPositionX(x);
		}

		if (node_camera.Height < h)
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
			int y = 0;
			if (up < 0)
			{
				y = down + (int)MathF.Abs(up);
				y = (int)(y / 2f);
				y -= (int)MathF.Abs(up);
			}
			else
			{
				y = down - up;
				y = (int)(y / 2f);
				y += up;
			}
			node_camera.CenterOnPositionY(y);
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

		int playerChunkX = (int)MathF.Floor(node_player.GlobalPosition.X / 16f);
		int playerChunkY = (int)MathF.Floor(node_player.GlobalPosition.Y / 16f);

		node_uiScreen.DrawText(1, 1, Screen.EDirection.Right, $"Player position: x={node_player.Position.X} y={node_player.Position.Y} gx={node_player.GlobalPosition.X} gy={node_player.GlobalPosition.Y} cx={playerChunkX} cy={playerChunkY}");
		node_uiScreen.DrawText(1, 2, Screen.EDirection.Right, $"Camera position: x={node_camera.Position.X} y={node_camera.Position.Y} w={node_camera.Width} h={node_camera.Height}");
		node_uiScreen.DrawText(1, 3, Screen.EDirection.Right, $"Camera bounds: left={node_camera.Bounds.Left} right={node_camera.Bounds.Right} up={node_camera.Bounds.Up} down={node_camera.Bounds.Down}");
		node_uiScreen.DrawText(1, 4, Screen.EDirection.Right, $"Current chunk: x={m_currentChunk.x} y={m_currentChunk.y}");
	}

	#endregion // Private methods

}

