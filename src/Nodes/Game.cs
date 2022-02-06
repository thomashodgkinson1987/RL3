public class Game : Node2D
{

	#region Nodes

	private MapGroup node_maps = new MapGroup();

	private Node2D node_actors = new Node2D();

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
	private (int x, int y) m_currentChunk;

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

		node_actors = RootNode.GetNode<Node2D>("Actors");

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

		node_maps.MapAdded += delegate (object? sender, MapAddedEventArgs args)
		{
			args.Map.FloorAdded += delegate (object? sender, GameObjectAddedEventArgs args1)
			{
				node_screen.IsDirty = true;
				node_floorsScreen.IsDirty = true;
			};

			args.Map.FloorRemoved += delegate (object? sender, GameObjectRemovedEventArgs args1)
			{
				node_screen.IsDirty = true;
				node_floorsScreen.IsDirty = true;
			};

			args.Map.WallAdded += delegate (object? sender, GameObjectAddedEventArgs args1)
			{
				node_screen.IsDirty = true;
				node_wallsScreen.IsDirty = true;
			};

			args.Map.WallRemoved += delegate (object? sender, GameObjectRemovedEventArgs args1)
			{
				node_screen.IsDirty = true;
				node_wallsScreen.IsDirty = true;
			};

			int left = args.Map.Position.X;
			int right = args.Map.Position.X + args.Map.Width;
			int up = args.Map.Position.Y;
			int down = args.Map.Position.Y + args.Map.Height;

			node_camera.ExpandBoundsTo(left, right, up, down);
		};

		node_maps.MapRemoved += delegate (object? sender, MapRemovedEventArgs args)
		{
			if (args.Map.Position.X == node_camera.Bounds.Left || args.Map.Position.X + args.Map.Width - 1 == node_camera.Bounds.Right
					|| args.Map.Position.Y == node_camera.Bounds.Up || args.Map.Position.Y + args.Map.Height - 1 == node_camera.Bounds.Down)
			{
				RecalculateCameraBounds();
			}
		};

		node_maps.MapLoaded += delegate (object? sender, MapLoadedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
		};

		node_maps.MapUnloaded += delegate (object? sender, MapUnloadedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
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

		node_camera.PositionChanged += delegate (object? sender, PointChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
			node_playerScreen.IsDirty = true;
			node_uiScreen.IsDirty = true;
		};
	
		List<Map> maps = new List<Map>();
		maps.Add(StaticMaps.GetMap1());
		maps.Add(StaticMaps.GetMap2(m_rng));
		maps.Add(StaticMaps.GetMap3());
		maps.Add(StaticMaps.GetMap4(m_rng));
		maps.Add(StaticMaps.GetMap5(m_rng));

		for (int i = -8; i < 8; i++)
		{
			for (int j = -8; j < 8; j++)
			{
				Map map = new Map();

				int c = m_rng.Next(0, 5);

				if (c == 0) map = StaticMaps.GetMap1();
				else if (c == 1) map = StaticMaps.GetMap2(m_rng);
				else if (c == 2) map = StaticMaps.GetMap3();
				else if (c == 3) map = StaticMaps.GetMap4(m_rng);
				else if (c == 4) map = StaticMaps.GetMap5(m_rng);

				for (int k = 0; k < m_rng.Next(0, 2); k++)
				{
					char[] symbols = { 'A', 'B', '%', '&', '$', '£', 'P', 'M' };

					int x = m_rng.Next(0, 15) * j;
					int y = m_rng.Next(0, 15) * i;
					char symbol = symbols[m_rng.Next(0, 8)];
					NPC npc = new NPC("NPC", x, y, symbol, m_rng);
					npc.IsFollowPlayer = m_rng.Next(0, 2) == 0;
					npc.ChanceToDoRandomMove = m_rng.Next(0, 75);
					node_actors.AddChild(npc);
				}

				node_maps.AddMap(j, i, map);
			}
		}

		m_currentMap = node_maps.GetMap(0, 0);
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
		foreach(NPC npc in node_actors.Children)
		{
			npc.Tick(this);
		}

		node_player.Tick(consoleKeyInfo, this);

		int newChunkX = (int)MathF.Floor(node_player.GlobalPosition.X / 16f);
		int newChunkY = (int)MathF.Floor(node_player.GlobalPosition.Y / 16f);

		if (newChunkX != m_currentChunk.x || newChunkY != m_currentChunk.y)
		{
			UpdateLoadedMaps(newChunkX, newChunkY);
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

		if (node_floorsScreen.IsDirty && node_wallsScreen.IsDirty)
		{
			bool wasFloorsScreenDirty = node_floorsScreen.IsDirty;
			bool wasWallsScreenDirty = node_wallsScreen.IsDirty;

			if (node_floorsScreen.IsDirty)
			{
				node_floorsScreen.IsDirty = false;
				node_floorsScreen.Clear();
			}

			if (node_wallsScreen.IsDirty)
			{
				node_wallsScreen.IsDirty = false;
				node_wallsScreen.Clear();
			}

			foreach(Map map in node_maps.ActiveMaps)
			{
				Point globalPosition = map.GlobalPosition - node_camera.GlobalPosition;

				if (wasFloorsScreenDirty)
				{
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

				if (wasWallsScreenDirty)
				{
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
		}

		if (node_playerScreen.IsDirty)
		{
			node_playerScreen.IsDirty = false;
			node_playerScreen.Clear();
			foreach(NPC npc in node_actors.Children)
			{
				Point npcGlobalPosition = npc.GlobalPosition - node_camera.GlobalPosition;
				if (node_playerScreen.IsPositionOnScreen(npcGlobalPosition))
				{
					node_playerScreen.SetSymbol(npcGlobalPosition, npc.Symbol);
				}
			}
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
		bool isWall = false;

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
			foreach(GameObject wall in map.Walls)
			{
				Point wallGlobalPosition = wall.Position + globalPosition;
				if (position == wallGlobalPosition)
				{
					isWall = true;
					break;
				}
			}
			if (isFloor && isWall) break;
		}

		return isFloor && !isWall;
	}

	public bool IsClear(int x, int y)
	{
		return IsClear(new Point(x, y));
	}

	#endregion // Public methods



	#region Private methods

	private void UpdateLoadedMaps(int chunkX, int chunkY)
	{
		node_screen.IsDirty = true;
		node_floorsScreen.IsDirty = true;
		node_wallsScreen.IsDirty = true;

		m_currentChunk = (chunkX, chunkY);
		m_currentMap = node_maps.GetMap(m_currentChunk.x, m_currentChunk.y);

		List<Map> newMaps = new List<Map>();

		for (int i = -node_maps.MapLoadDistance; i <= node_maps.MapLoadDistance; i++)
		{
			for (int j = -node_maps.MapLoadDistance; j <= node_maps.MapLoadDistance; j++)
			{
				int x = m_currentChunk.x + j;
				int y = m_currentChunk.y + i;
				if (node_maps.IsMapAtPosition(x, y))
				{
					Map map = node_maps.GetMap(x, y);
					newMaps.Add(map);
					if (!node_maps.IsMapLoaded(map))
					{
						node_maps.LoadMap(map);
					}
				}
			}
		}

		for (int i = 0; i < node_maps.ActiveMaps.Count; i++)
		{
			Map map = node_maps.ActiveMaps[i];
			if (!newMaps.Contains(map))
			{
				node_maps.UnloadMap(map);
				i--;
			}
		}
	}

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

	private void RecalculateCameraBounds()
	{
		int left = node_maps.AllMaps.Min(_ => _.GlobalPosition.X);
		int right = node_maps.AllMaps.Max(_ => _.GlobalPosition.X) + m_currentMap.Width;
		int up = node_maps.AllMaps.Min(_ => _.GlobalPosition.Y);
		int down = node_maps.AllMaps.Max(_ => _.GlobalPosition.Y) + m_currentMap.Height;

		node_camera.SetBounds(left, right, up, down);
	}

	private void TickCamera()
	{
		node_camera.CenterOnPosition(node_player.GlobalPosition);
		node_camera.ClampToBounds();
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

		node_uiScreen.DrawText(1, 1, Screen.EDirection.Right, $"Player: x={node_player.Position.X} y={node_player.Position.Y} gx={node_player.GlobalPosition.X} gy={node_player.GlobalPosition.Y} cx={m_currentChunk.x} cy={m_currentChunk.y}");
		node_uiScreen.DrawText(1, 2, Screen.EDirection.Right, $"Camera position: x={node_camera.Position.X} y={node_camera.Position.Y} gx={node_camera.GlobalPosition.X} gy={node_camera.GlobalPosition.Y} w={node_camera.Width} h={node_camera.Height}");
		node_uiScreen.DrawText(1, 3, Screen.EDirection.Right, $"Camera bounds: left={node_camera.Bounds.Left} right={node_camera.Bounds.Right} up={node_camera.Bounds.Up} down={node_camera.Bounds.Down}");
		node_uiScreen.DrawText(1, 4, Screen.EDirection.Right, $"Maps: all={node_maps.AllMaps.Count} active={node_maps.ActiveMaps.Count}");
		node_uiScreen.DrawText(1, 5, Screen.EDirection.Right, $"Floors: all={node_maps.AllFloors.Count} active={node_maps.ActiveFloors.Count}");
		node_uiScreen.DrawText(1, 6, Screen.EDirection.Right, $"Walls: all={node_maps.AllWalls.Count} active={node_maps.ActiveWalls.Count}");
	}

	#endregion // Private methods

}

