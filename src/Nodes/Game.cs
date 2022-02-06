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
	private Screen node_actorsScreen = new Screen();
	private Screen node_playerScreen = new Screen();

	private ScreenGroup node_uiScreens = new ScreenGroup();

	private Screen node_uiScreen = new Screen();

	private Camera node_camera = new Camera();

	#endregion // Nodes



	#region Fields

	private readonly Random m_rng;

	private Map m_currentMap;
	private (int x, int y) m_currentChunk;

	private int m_steps;

	private readonly EventHandler<MapAddedEventArgs> a_OnMapAdded;
	private readonly EventHandler<MapRemovedEventArgs> a_OnMapRemoved;
	private readonly EventHandler<MapLoadedEventArgs> a_OnMapLoaded;
	private readonly EventHandler<MapUnloadedEventArgs> a_OnMapUnloaded;

	private readonly EventHandler<GameObjectAddedEventArgs> a_OnFloorAddedToMap;
	private readonly EventHandler<GameObjectRemovedEventArgs> a_OnFloorRemovedFromMap;

	private readonly EventHandler<GameObjectAddedEventArgs> a_OnWallAddedToMap;
	private readonly EventHandler<GameObjectRemovedEventArgs> a_OnWallRemovedFromMap;

	private readonly EventHandler<NPCAddedEventArgs> a_OnActorAddedToMap;
	private readonly EventHandler<NPCRemovedEventArgs> a_OnActorRemovedFromMap;

	private readonly EventHandler<PointChangedEventArgs> a_OnPlayerPositionChanged;
	private readonly EventHandler<IntChangedEventArgs> a_OnPlayerHealthChanged;
	private readonly EventHandler<IntChangedEventArgs> a_OnPlayerMaxHealthChanged;

	private readonly EventHandler<PointChangedEventArgs> a_OnActorPositionChanged;

	private readonly EventHandler<PointChangedEventArgs> a_OnCameraPositionChanged;

	#endregion // Fields



	#region Constructors

	public Game(string name, int x, int y, Random rng) : base(name, x, y)
	{
		m_rng = rng;
		m_currentMap = new Map();
		m_currentChunk = (0, 0);
		m_steps = 0;

		a_OnMapAdded = delegate (object? sender, MapAddedEventArgs args)
		{
			int left = args.Map.Position.X;
			int right = args.Map.Position.X + args.Map.Width - 1;
			int up = args.Map.Position.Y;
			int down = args.Map.Position.Y + args.Map.Height - 1;

			node_camera.ExpandBoundsTo(left, right, up, down);

			args.Map.IsDirty = true;
		};

		a_OnMapRemoved = delegate (object? sender, MapRemovedEventArgs args)
		{
			Point position = node_maps.GetMapPosition(args.Map);
			int width = args.Map.Width;
			int height = args.Map.Height;

			int left = node_camera.Bounds.Left;
			int right = node_camera.Bounds.Right;
			int up = node_camera.Bounds.Up;
			int down = node_camera.Bounds.Down;

			if (position.X == left || position.X + width - 1 == right || position.Y == up || position.Y + height - 1 == down)
			{
				RecalculateCameraBounds();
			}

			args.Map.IsDirty = true;
		};

		a_OnMapLoaded = delegate (object? sender, MapLoadedEventArgs args)
		{
			args.Map.FloorAdded += a_OnFloorAddedToMap;
			args.Map.FloorRemoved += a_OnFloorRemovedFromMap;
			args.Map.WallAdded += a_OnWallAddedToMap;
			args.Map.WallRemoved += a_OnWallRemovedFromMap;
			args.Map.ActorAdded += a_OnActorAddedToMap;
			args.Map.ActorRemoved += a_OnActorRemovedFromMap;

			args.Map.IsDirty = true;

			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
			node_actorsScreen.IsDirty = true;
			node_playerScreen.IsDirty = true;
		};

		a_OnMapUnloaded = delegate (object? sender, MapUnloadedEventArgs args)
		{
			args.Map.FloorAdded -= a_OnFloorAddedToMap;
			args.Map.FloorRemoved -= a_OnFloorRemovedFromMap;
			args.Map.WallAdded -= a_OnWallAddedToMap;
			args.Map.WallRemoved -= a_OnWallRemovedFromMap;
			args.Map.ActorAdded -= a_OnActorAddedToMap;
			args.Map.ActorRemoved -= a_OnActorRemovedFromMap;

			args.Map.IsDirty = true;

			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
			node_actorsScreen.IsDirty = true;
			node_playerScreen.IsDirty = true;
		};

		a_OnPlayerPositionChanged = delegate (object? sender, PointChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_playerScreen.IsDirty = true;
			node_uiScreen.IsDirty = true;

			m_currentMap.IsDirty = true;
		};

		a_OnPlayerHealthChanged = delegate (object? sender, IntChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_uiScreen.IsDirty = true;
		};

		a_OnPlayerMaxHealthChanged = delegate (object? sender, IntChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_uiScreen.IsDirty = true;
		};

		a_OnActorPositionChanged = delegate (object? sender, PointChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_actorsScreen.IsDirty = true;
		};

		a_OnCameraPositionChanged = delegate (object? sender, PointChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
			node_playerScreen.IsDirty = true;
			node_actorsScreen.IsDirty = true;
			node_uiScreen.IsDirty = true;
		};

		a_OnFloorAddedToMap = delegate (object? sender, GameObjectAddedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
		};

		a_OnFloorRemovedFromMap = delegate (object? sender, GameObjectRemovedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
		};

		a_OnWallAddedToMap = delegate (object? sender, GameObjectAddedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
		};

		a_OnWallRemovedFromMap = delegate (object? sender, GameObjectRemovedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
		};

		a_OnActorAddedToMap = delegate (object? sender, NPCAddedEventArgs args)
		{
			args.NPC.PositionChanged += a_OnActorPositionChanged;

			node_screen.IsDirty = true;
			node_actorsScreen.IsDirty = true;
		};

		a_OnActorRemovedFromMap = delegate (object? sender, NPCRemovedEventArgs args)
		{
			args.NPC.PositionChanged -= a_OnActorPositionChanged;

			node_screen.IsDirty = true;
			node_actorsScreen.IsDirty = true;
		};
	}

	public Game(string name, Random rng) : this(name, 0, 0, rng) { }

	public Game(int x, int y, Random rng) : this("Game", x, y, rng) { }

	public Game(Random rng) : this("Game", 0, 0, rng) { }

	#endregion // Constructors



	#region Node2D methods

	public override void Init()
	{
		base.Init();

		// Get nodes

		node_maps = RootNode.GetNode<MapGroup>("Maps");

		node_player = RootNode.GetNode<Player>("Player");

		node_screens = RootNode.GetNode<ScreenGroup>("Screens");

		node_screen = node_screens.GetNode<Screen>("Screen");

		node_gameScreens = node_screens.GetNode<ScreenGroup>("GameScreens");

		node_floorsScreen = node_gameScreens.GetNode<Screen>("FloorsScreen");
		node_wallsScreen = node_gameScreens.GetNode<Screen>("WallsScreen");
		node_actorsScreen = node_gameScreens.GetNode<Screen>("ActorsScreen");
		node_playerScreen = node_gameScreens.GetNode<Screen>("PlayerScreen");

		node_uiScreens = node_screens.GetNode<ScreenGroup>("UIScreens");

		node_uiScreen = node_uiScreens.GetNode<Screen>("UIScreen");

		node_camera = RootNode.GetNode<Camera>("Camera");

		// Wire up events

		node_maps.MapAdded += a_OnMapAdded;
		node_maps.MapRemoved += a_OnMapRemoved;
		node_maps.MapLoaded += a_OnMapLoaded;
		node_maps.MapUnloaded += a_OnMapUnloaded;

		node_player.PositionChanged += a_OnPlayerPositionChanged;
		node_player.HealthChanged += a_OnPlayerHealthChanged;
		node_player.MaxHealthChanged += a_OnPlayerMaxHealthChanged;

		node_camera.PositionChanged += a_OnCameraPositionChanged;

		// Populate and load maps

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

					int x = m_rng.Next(0, 15);
					int y = m_rng.Next(0, 15);
					char symbol = symbols[m_rng.Next(0, 8)];
					NPC actor = new NPC("NPC", x, y, m_rng);
					actor.Symbol = symbol;
					actor.IsFollowPlayer = m_rng.Next(0, 2) == 0;
					actor.ChanceToDoMove = m_rng.Next(0, 75);
					map.AddActor(actor, x, y);
				}

				node_maps.AddMap(map, j, i);
			}
		}

		m_currentMap = node_maps.GetMap(0, 0);
		m_currentChunk = (0, 0);

		for (int i = -node_maps.MapLoadDistance; i <= node_maps.MapLoadDistance; i++)
		{
			for (int j = -node_maps.MapLoadDistance; j <= node_maps.MapLoadDistance; j++)
			{
				node_maps.LoadMap(j, i);
			}
		}

		// Final clears

		Console.Clear();
		TickCamera();
		Draw();
	}

	#endregion // Node2D methods



	#region Public methods

	public void Tick(ConsoleKeyInfo consoleKeyInfo)
	{
		m_steps++;

		{
			node_player.Tick(consoleKeyInfo, this);

			int newChunkX = (int)MathF.Floor(node_player.GlobalPosition.X / 16f);
			int newChunkY = (int)MathF.Floor(node_player.GlobalPosition.Y / 16f);

			if (newChunkX != m_currentChunk.x || newChunkY != m_currentChunk.y)
			{
				UpdateLoadedMaps(newChunkX, newChunkY);
			}
		}

		foreach (NPC actor in node_maps.ActiveActors)
		{
			Point previousPosition = actor.Position;
			Point previousGlobalPosition = actor.GlobalPosition;
			int previousChunkX = (int)MathF.Floor(previousGlobalPosition.X / 16f);
			int previousChunkY = (int)MathF.Floor(previousGlobalPosition.Y / 16f);
			Point previousChunk = new Point(previousChunkX, previousChunkY);

			actor.Tick(this);

			Point newPosition = actor.Position;
			Point newGlobalPosition = actor.GlobalPosition;
			int newChunkX = (int)MathF.Floor(newGlobalPosition.X / 16f);
			int newChunkY = (int)MathF.Floor(newGlobalPosition.Y / 16f);
			Point newChunk = new Point(newChunkX, newChunkY);

			if (previousChunk != newChunk)
			{
				Map previousMap = node_maps.GetMap(previousChunk);
				Map newMap = node_maps.GetMap(newChunk);

				int dirX = previousChunkX < newChunkX ? 1 : previousChunkX > newChunkX ? -1 : 0;
				int dirY = previousChunkY < newChunkY ? 1 : previousChunkY > newChunkY ? -1 : 0;

				previousMap.RemoveActor(actor);

				int _x = dirX == -1 ? 15 : dirX == 1 ? 0 : actor.Position.X;
				int _y = dirY == -1 ? 15 : dirY == 1 ? 0 : actor.Position.Y;

				actor.SetPosition(_x, _y);

				newMap.AddActor(actor, actor.Position);
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

		if (node_floorsScreen.IsDirty || node_wallsScreen.IsDirty || node_actorsScreen.IsDirty)
		{
			bool wasFloorsScreenDirty = node_floorsScreen.IsDirty;
			bool wasWallsScreenDirty = node_wallsScreen.IsDirty;
			bool wasActorsScreenDirty = node_actorsScreen.IsDirty;

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
			if (node_actorsScreen.IsDirty)
			{
				node_actorsScreen.IsDirty = false;
				node_actorsScreen.Clear();
			}

			foreach (Map map in node_maps.ActiveMaps)
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

				if (wasActorsScreenDirty)
				{
					foreach (NPC actor in map.Actors)
					{
						if (!actor.IsVisible) continue;

						Point actorGlobalPosition = globalPosition + actor.Position;
						if (node_actorsScreen.IsPositionOnScreen(actorGlobalPosition))
						{
							node_actorsScreen.SetSymbol(actorGlobalPosition, actor.Symbol);
						}
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
		bool isWall = false;
		bool isPlayer = position == node_player.GlobalPosition;
		bool isActor = false;

		foreach (Map map in node_maps.ActiveMaps)
		{
			Point globalPosition = map.GlobalPosition;
			foreach (GameObject floor in map.Floors)
			{
				Point floorGlobalPosition = floor.Position + globalPosition;
				if (position == floorGlobalPosition)
				{
					isFloor = true;
					break;
				}
			}
			foreach (GameObject wall in map.Walls)
			{
				Point wallGlobalPosition = wall.Position + globalPosition;
				if (position == wallGlobalPosition)
				{
					isWall = true;
					break;
				}
			}
			foreach (NPC actor in map.Actors)
			{
				Point actorGlobalPosition = actor.Position + globalPosition;
				if (position == actorGlobalPosition)
				{
					isActor = true;
					break;
				}
			}
			if (isFloor && isWall && isActor) break;
		}

		return isFloor && !isWall && !isPlayer && !isActor;
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
		node_actorsScreen.IsDirty = true;
		node_playerScreen.IsDirty = true;
		node_uiScreen.IsDirty = true;

		m_currentChunk = (chunkX, chunkY);
		m_currentMap = node_maps.GetMap(m_currentChunk.x, m_currentChunk.y);

		(int x, int y) chunk = (chunkX, chunkY);

		for (int i = -1; i >= -node_maps.MapLoadDistance; i--)
		{
			if (!node_maps.IsMapAtPosition(chunkX + i, chunkY))
			{
				chunk.x += 1;
			}
			if (!node_maps.IsMapAtPosition(chunkX, chunkY + i))
			{
				chunk.y += 1;
			}
		}
		for (int i = 1; i <= node_maps.MapLoadDistance; i++)
		{
			if (!node_maps.IsMapAtPosition(chunkX + i, chunkY))
			{
				chunk.x -= 1;
			}
			if (!node_maps.IsMapAtPosition(chunkX, chunkY + i))
			{
				chunk.y -= 1;
			}
		}

		List<Map> newMaps = new List<Map>();

		for (int i = -node_maps.MapLoadDistance; i <= node_maps.MapLoadDistance; i++)
		{
			for (int j = -node_maps.MapLoadDistance; j <= node_maps.MapLoadDistance; j++)
			{
				int x = chunk.x + j;
				int y = chunk.y + i;
				Map map = node_maps.GetMap(x, y);
				newMaps.Add(map);
				if (!node_maps.IsMapLoaded(map))
				{
					node_maps.LoadMap(map);
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
		int right = node_maps.AllMaps.Max(_ => _.GlobalPosition.X) + m_currentMap.Width - 1;
		int up = node_maps.AllMaps.Min(_ => _.GlobalPosition.Y);
		int down = node_maps.AllMaps.Max(_ => _.GlobalPosition.Y) + m_currentMap.Height - 1;

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
		node_uiScreen.DrawText(node_uiScreen.Width - 2, 1, Screen.EDirection.Left, $"Steps: {m_steps}");
		node_uiScreen.DrawText(1, 2, Screen.EDirection.Right, $"Camera position: x={node_camera.Position.X} y={node_camera.Position.Y} gx={node_camera.GlobalPosition.X} gy={node_camera.GlobalPosition.Y} w={node_camera.Width} h={node_camera.Height}");
		node_uiScreen.DrawText(1, 3, Screen.EDirection.Right, $"Camera bounds: left={node_camera.Bounds.Left} right={node_camera.Bounds.Right} up={node_camera.Bounds.Up} down={node_camera.Bounds.Down}");
		node_uiScreen.DrawText(1, 4, Screen.EDirection.Right, $"Maps: all={node_maps.AllMaps.Count} active={node_maps.ActiveMaps.Count}");
		node_uiScreen.DrawText(node_uiScreen.Width - 2, 4, Screen.EDirection.Left, $"Actors: all={node_maps.AllActors.Count} active={node_maps.ActiveActors.Count}");
		node_uiScreen.DrawText(1, 5, Screen.EDirection.Right, $"Actors in this chunk: {m_currentMap.Actors.Count}");
	}

	#endregion // Private methods

}

