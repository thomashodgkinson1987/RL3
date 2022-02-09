public class Game : Node2D
{

	#region Nodes

	private Map node_map;

	private Player node_player;

	private ScreenGroup node_screens;
	private ScreenGroup node_gameScreens;
	private ScreenGroup node_uiScreens;
	private ScreenGroup node_borderScreens;

	private Screen node_screen;

	private Screen node_floorsScreen;
	private Screen node_wallsScreen;
	private Screen node_actorsScreen;
	private Screen node_playerScreen;

	private Screen node_attributesScreen;
	private Screen node_messageLogScreen;
	private Screen node_menuScreen;
	private Screen node_statusScreen;

	private Screen node_gameScreenBorder;
	private Screen node_attributesScreenBorder;
	private Screen node_messageLogScreenBorder;
	private Screen node_menuScreenBorder;
	private Screen node_statusScreenBorder;

	private Camera node_camera;

	#endregion // Nodes



	#region Fields

	public const int SCREEN_X = 0;
	public const int SCREEN_Y = 0;
	public const int SCREEN_W = 92;
	public const int SCREEN_H = 38;
	
	public const int GAME_SCREEN_X = 1;
	public const int GAME_SCREEN_Y = 1;
	public const int GAME_SCREEN_W = 58;
	public const int GAME_SCREEN_H = 20;

	public const int ATTRIBUTES_SCREEN_X = 1;
	public const int ATTRIBUTES_SCREEN_Y = 23;
	public const int ATTRIBUTES_SCREEN_W = 58;
	public const int ATTRIBUTES_SCREEN_H = 3;

	public const int MESSAGE_LOG_SCREEN_X = 1;
	public const int MESSAGE_LOG_SCREEN_Y = 28;
	public const int MESSAGE_LOG_SCREEN_W = 58;
	public const int MESSAGE_LOG_SCREEN_H = 9;

	public const int MENU_SCREEN_X = 62;
	public const int MENU_SCREEN_Y = 1;
	public const int MENU_SCREEN_W = 28;
	public const int MENU_SCREEN_H = 25;

	public const int STATUS_SCREEN_X = 62;
	public const int STATUS_SCREEN_Y = 28;
	public const int STATUS_SCREEN_W = 28;
	public const int STATUS_SCREEN_H = 9;

	private readonly Random m_rng;

	private (int x, int y) m_currentMapChunkPosition;

	private int m_steps;

	private EventHandler<MapChunkAddedEventArgs>? a_OnMapChunkAdded;
	private EventHandler<MapChunkRemovedEventArgs>? a_OnMapChunkRemoved;
	private EventHandler<MapChunkLoadedEventArgs>? a_OnMapChunkLoaded;
	private EventHandler<MapChunkUnloadedEventArgs>? a_OnMapChunkUnloaded;

	private EventHandler<FloorAddedEventArgs>? a_OnFloorAddedToMapChunk;
	private EventHandler<FloorRemovedEventArgs>? a_OnFloorRemovedFromMapChunk;

	private EventHandler<WallAddedEventArgs>? a_OnWallAddedToMapChunk;
	private EventHandler<WallRemovedEventArgs>? a_OnWallRemovedFromMapChunk;

	private EventHandler<ActorAddedEventArgs>? a_OnActorAddedToMapChunk;
	private EventHandler<ActorRemovedEventArgs>? a_OnActorRemovedFromMapChunk;

	private EventHandler<PointChangedEventArgs>? a_OnPlayerPositionChanged;
	private EventHandler<IntChangedEventArgs>? a_OnPlayerHealthChanged;
	private EventHandler<IntChangedEventArgs>? a_OnPlayerMaxHealthChanged;

	private EventHandler<PointChangedEventArgs>? a_OnActorPositionChanged;

	private EventHandler<PointChangedEventArgs>? a_OnCameraPositionChanged;

	#endregion // Fields



	#region Constructors

	public Game(string name, int x, int y, Random rng) : base(name, x, y)
	{
		node_map = new Map();

		node_player = new Player('@', rng);

		node_screens = new ScreenGroup("Screens");
		node_gameScreens = new ScreenGroup("GameScreens");
		node_uiScreens = new ScreenGroup("UIScreens");
		node_borderScreens = new ScreenGroup("BorderScreens");

		node_screen = new Screen("Screen", SCREEN_X, SCREEN_Y, SCREEN_W, SCREEN_H);

		node_screens.AddChild(node_screen);

		node_floorsScreen = new Screen("FloorsScreen", GAME_SCREEN_X, GAME_SCREEN_Y, GAME_SCREEN_W, GAME_SCREEN_H);
		node_wallsScreen = new Screen("WallsScreen", GAME_SCREEN_X, GAME_SCREEN_Y, GAME_SCREEN_W, GAME_SCREEN_H);
		node_actorsScreen = new Screen("ActorsScreen", GAME_SCREEN_X, GAME_SCREEN_Y, GAME_SCREEN_W, GAME_SCREEN_H);
		node_playerScreen = new Screen("PlayerScreen", GAME_SCREEN_X, GAME_SCREEN_Y, GAME_SCREEN_W, GAME_SCREEN_H);

		node_gameScreens.AddChild(node_floorsScreen);
		node_gameScreens.AddChild(node_wallsScreen);
		node_gameScreens.AddChild(node_actorsScreen);
		node_gameScreens.AddChild(node_playerScreen);

		node_attributesScreen = new Screen("AttributesScreen", ATTRIBUTES_SCREEN_X, ATTRIBUTES_SCREEN_Y, ATTRIBUTES_SCREEN_W, ATTRIBUTES_SCREEN_H);
		node_messageLogScreen = new Screen("MessageLogScreen", MESSAGE_LOG_SCREEN_X, MESSAGE_LOG_SCREEN_Y, MESSAGE_LOG_SCREEN_W, MESSAGE_LOG_SCREEN_H);
		node_menuScreen = new Screen("MenuScreen", MENU_SCREEN_X, MENU_SCREEN_Y, MENU_SCREEN_W, MENU_SCREEN_H);
		node_statusScreen = new Screen("StatusScreen", STATUS_SCREEN_X, STATUS_SCREEN_Y, STATUS_SCREEN_W, STATUS_SCREEN_H);

		node_uiScreens.AddChild(node_attributesScreen);
		node_uiScreens.AddChild(node_messageLogScreen);
		node_uiScreens.AddChild(node_menuScreen);
		node_uiScreens.AddChild(node_statusScreen);

		node_gameScreenBorder = new Screen("GameScreenBorder", GAME_SCREEN_X - 1, GAME_SCREEN_Y - 1, GAME_SCREEN_W + 2, GAME_SCREEN_H + 2);
		node_attributesScreenBorder = new Screen("AttributesScreenBorder", ATTRIBUTES_SCREEN_X - 1, ATTRIBUTES_SCREEN_Y - 1, ATTRIBUTES_SCREEN_W + 2, ATTRIBUTES_SCREEN_H + 2);
		node_messageLogScreenBorder = new Screen("MessageLogScreenBorder", MESSAGE_LOG_SCREEN_X - 1, MESSAGE_LOG_SCREEN_Y - 1, MESSAGE_LOG_SCREEN_W + 2, MESSAGE_LOG_SCREEN_H + 2);
		node_menuScreenBorder = new Screen("MenuScreenBorder", MENU_SCREEN_X - 1, MENU_SCREEN_Y - 1, MENU_SCREEN_W + 2, MENU_SCREEN_H + 2);
		node_statusScreenBorder = new Screen("StatusScreenBorder", STATUS_SCREEN_X - 1, STATUS_SCREEN_Y - 1, STATUS_SCREEN_W + 2, STATUS_SCREEN_H + 2);

		node_borderScreens.AddChild(node_gameScreenBorder);
		node_borderScreens.AddChild(node_attributesScreenBorder);
		node_borderScreens.AddChild(node_messageLogScreenBorder);
		node_borderScreens.AddChild(node_menuScreenBorder);
		node_borderScreens.AddChild(node_statusScreenBorder);

		node_camera = new Camera("Camera", GAME_SCREEN_X, GAME_SCREEN_Y, GAME_SCREEN_W, GAME_SCREEN_H);

		AddChild(node_map);
		AddChild(node_player);
		AddChild(node_screens);
		AddChild(node_gameScreens);
		AddChild(node_uiScreens);
		AddChild(node_borderScreens);
		AddChild(node_camera);

		m_rng = rng;

		m_currentMapChunkPosition = (0, 0);
		m_steps = 0;
	}

	public Game(string name, Random rng) : this(name, 0, 0, rng) { }

	public Game(int x, int y, Random rng) : this("Game", x, y, rng) { }

	public Game(Random rng) : this("Game", 0, 0, rng) { }

	#endregion // Constructors



	#region Node2D methods

	public override void Init()
	{
		base.Init();

		a_OnMapChunkAdded = delegate (object? sender, MapChunkAddedEventArgs args)
		{
			int left = args.MapChunk.Position.X;
			int right = args.MapChunk.Position.X + args.MapChunk.Width - 1;
			int up = args.MapChunk.Position.Y;
			int down = args.MapChunk.Position.Y + args.MapChunk.Height - 1;

			node_camera.ExpandBoundsTo(left, right, up, down);
		};

		a_OnMapChunkRemoved = delegate (object? sender, MapChunkRemovedEventArgs args)
		{
			Point position = node_map.GetMapChunkPosition(args.MapChunk);
			int width = args.MapChunk.Width;
			int height = args.MapChunk.Height;

			int left = node_camera.Bounds.Left;
			int right = node_camera.Bounds.Right;
			int up = node_camera.Bounds.Up;
			int down = node_camera.Bounds.Down;

			if (position.X == left || position.X + width - 1 == right || position.Y == up || position.Y + height - 1 == down)
			{
				RecalculateCameraBounds();
			}
		};

		a_OnMapChunkLoaded = delegate (object? sender, MapChunkLoadedEventArgs args)
		{
			args.MapChunk.FloorAdded += a_OnFloorAddedToMapChunk;
			args.MapChunk.FloorRemoved += a_OnFloorRemovedFromMapChunk;
			args.MapChunk.WallAdded += a_OnWallAddedToMapChunk;
			args.MapChunk.WallRemoved += a_OnWallRemovedFromMapChunk;
			args.MapChunk.ActorAdded += a_OnActorAddedToMapChunk;
			args.MapChunk.ActorRemoved += a_OnActorRemovedFromMapChunk;

			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
			node_actorsScreen.IsDirty = true;
			node_playerScreen.IsDirty = true;
			node_messageLogScreen.IsDirty = true;
		};

		a_OnMapChunkUnloaded = delegate (object? sender, MapChunkUnloadedEventArgs args)
		{
			args.MapChunk.FloorAdded -= a_OnFloorAddedToMapChunk;
			args.MapChunk.FloorRemoved -= a_OnFloorRemovedFromMapChunk;
			args.MapChunk.WallAdded -= a_OnWallAddedToMapChunk;
			args.MapChunk.WallRemoved -= a_OnWallRemovedFromMapChunk;
			args.MapChunk.ActorAdded -= a_OnActorAddedToMapChunk;
			args.MapChunk.ActorRemoved -= a_OnActorRemovedFromMapChunk;

			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
			node_actorsScreen.IsDirty = true;
			node_playerScreen.IsDirty = true;
			node_messageLogScreen.IsDirty = true;
		};

		a_OnPlayerPositionChanged = delegate (object? sender, PointChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_playerScreen.IsDirty = true;
			node_messageLogScreen.IsDirty = true;

			int oldChunkPositionX = (int)(MathF.Floor(args.PointBeforeChange.X / 16f));
			int oldChunkPositionY = (int)(MathF.Floor(args.PointBeforeChange.Y / 16f));
			Point oldChunkPosition = new Point(oldChunkPositionX, oldChunkPositionY);

			int newChunkPositionX = (int)(MathF.Floor(args.PointAfterChange.X / 16f));
			int newChunkPositionY = (int)(MathF.Floor(args.PointAfterChange.Y / 16f));
			Point newChunkPosition = new Point(newChunkPositionX, newChunkPositionY);

			if (oldChunkPosition != newChunkPosition)
			{
				node_map.GetMapChunk(oldChunkPosition).IsDirty = true;
				node_map.GetMapChunk(newChunkPosition).IsDirty = true;
			}
		};

		a_OnPlayerHealthChanged = delegate (object? sender, IntChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_messageLogScreen.IsDirty = true;
		};

		a_OnPlayerMaxHealthChanged = delegate (object? sender, IntChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_messageLogScreen.IsDirty = true;
		};

		a_OnActorPositionChanged = delegate (object? sender, PointChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_actorsScreen.IsDirty = true;

			int oldChunkPositionX = (int)(MathF.Floor(args.PointBeforeChange.X / 16f));
			int oldChunkPositionY = (int)(MathF.Floor(args.PointBeforeChange.Y / 16f));
			Point oldChunkPosition = new Point(oldChunkPositionX, oldChunkPositionY);

			int newChunkPositionX = (int)(MathF.Floor(args.PointAfterChange.X / 16f));
			int newChunkPositionY = (int)(MathF.Floor(args.PointAfterChange.Y / 16f));
			Point newChunkPosition = new Point(newChunkPositionX, newChunkPositionY);

			if (oldChunkPosition != newChunkPosition)
			{
				node_map.GetMapChunk(oldChunkPosition).IsDirty = true;
				node_map.GetMapChunk(newChunkPosition).IsDirty = true;
			}
		};

		a_OnCameraPositionChanged = delegate (object? sender, PointChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
			node_actorsScreen.IsDirty = true;
			node_playerScreen.IsDirty = true;
			node_messageLogScreen.IsDirty = true;
		};

		a_OnFloorAddedToMapChunk = delegate (object? sender, FloorAddedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
		};

		a_OnFloorRemovedFromMapChunk = delegate (object? sender, FloorRemovedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_floorsScreen.IsDirty = true;
		};

		a_OnWallAddedToMapChunk = delegate (object? sender, WallAddedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
		};

		a_OnWallRemovedFromMapChunk = delegate (object? sender, WallRemovedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_wallsScreen.IsDirty = true;
		};

		a_OnActorAddedToMapChunk = delegate (object? sender, ActorAddedEventArgs args)
		{
			args.Actor.PositionChanged += a_OnActorPositionChanged;

			node_screen.IsDirty = true;
			node_actorsScreen.IsDirty = true;
		};

		a_OnActorRemovedFromMapChunk = delegate (object? sender, ActorRemovedEventArgs args)
		{
			args.Actor.PositionChanged -= a_OnActorPositionChanged;

			node_screen.IsDirty = true;
			node_actorsScreen.IsDirty = true;
		};

		node_map.MapChunkAdded += a_OnMapChunkAdded;
		node_map.MapChunkRemoved += a_OnMapChunkRemoved;
		node_map.MapChunkLoaded += a_OnMapChunkLoaded;
		node_map.MapChunkUnloaded += a_OnMapChunkUnloaded;

		node_player.PositionChanged += a_OnPlayerPositionChanged;
		node_player.HealthChanged += a_OnPlayerHealthChanged;
		node_player.MaxHealthChanged += a_OnPlayerMaxHealthChanged;

		node_camera.PositionChanged += a_OnCameraPositionChanged;
	}

	public override void Ready ()
	{
		base.Ready();

		for (int i = -8; i < 8; i++)
		{
			for (int j = -8; j < 8; j++)
			{
				MapChunk mapChunk = new MapChunk();

				int c = m_rng.Next(0, 5);

				if (c == 0) mapChunk = StaticMapChunks.GetMapChunk1();
				else if (c == 1) mapChunk = StaticMapChunks.GetMapChunk2(m_rng);
				else if (c == 2) mapChunk = StaticMapChunks.GetMapChunk3();
				else if (c == 3) mapChunk = StaticMapChunks.GetMapChunk4(m_rng);
				else if (c == 4) mapChunk = StaticMapChunks.GetMapChunk5(m_rng);

				for (int k = 0; k < m_rng.Next(0, 5); k++)
				{
					char[] symbols = { 'A', 'B', '%', '&', '$', '£', 'P', 'M' };

					int x = m_rng.Next(0, 15);
					int y = m_rng.Next(0, 15);
					char symbol = symbols[m_rng.Next(0, 8)];

					NPC npc = new NPC(x, y, symbol, m_rng);
					npc.IsFollowPlayer = m_rng.Next(0, 2) == 0;
					npc.ChanceToDoMove = m_rng.Next(0, 75);

					mapChunk.AddActor(npc, x, y);
				}

				node_map.AddMapChunk(mapChunk, j, i);
			}
		}

		m_currentMapChunkPosition = (0, 0);

		for (int i = -node_map.MapChunkLoadDistance; i <= node_map.MapChunkLoadDistance; i++)
		{
			for (int j = -node_map.MapChunkLoadDistance; j <= node_map.MapChunkLoadDistance; j++)
			{
				node_map.LoadMapChunk(j, i);
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
		m_steps++;

		{
			node_player.Tick(consoleKeyInfo, this);

			Point oldMapChunkPosition = new Point(m_currentMapChunkPosition.x, m_currentMapChunkPosition.y);

			int newMapChunkPositionX = (int)MathF.Floor(node_player.GlobalPosition.X / 16f);
			int newMapChunkPositionY = (int)MathF.Floor(node_player.GlobalPosition.Y / 16f);
			Point newMapChunkPosition = new Point(newMapChunkPositionX, newMapChunkPositionY);

			if (oldMapChunkPosition != newMapChunkPosition)
			{
				UpdateLoadedMaps(newMapChunkPositionX, newMapChunkPositionY);
			}
		}

		foreach (Actor actor in node_map.ActiveActors)
		{
			Point oldActorPosition = actor.Position;
			Point oldActorGlobalPosition = actor.GlobalPosition;
			int oldMapChunkPositionX = (int)MathF.Floor(oldActorGlobalPosition.X / 16f);
			int oldMapChunkPositionY = (int)MathF.Floor(oldActorGlobalPosition.Y / 16f);
			Point oldMapChunkPosition = new Point(oldMapChunkPositionX, oldMapChunkPositionY);

			actor.Tick();

			Point newActorPosition = actor.Position;
			Point newActorGlobalPosition = actor.GlobalPosition;
			int newMapChunkPositionX = (int)MathF.Floor(newActorGlobalPosition.X / 16f);
			int newMapChunkPositionY = (int)MathF.Floor(newActorGlobalPosition.Y / 16f);
			Point newMapChunkPosition = new Point(newMapChunkPositionX, newMapChunkPositionY);

			if (oldMapChunkPosition != newMapChunkPosition)
			{
				MapChunk oldMapChunk = node_map.GetMapChunk(oldMapChunkPosition);
				MapChunk newMapChunk = node_map.GetMapChunk(newMapChunkPosition);

				int dirX = oldMapChunkPositionX < newMapChunkPositionX ? 1 : oldMapChunkPositionX > newMapChunkPositionX ? -1 : 0;
				int dirY = oldMapChunkPositionY < newMapChunkPositionY ? 1 : oldMapChunkPositionY > newMapChunkPositionY ? -1 : 0;

				oldMapChunk.RemoveActor(actor);

				int _x = dirX == -1 ? 15 : dirX == 1 ? 0 : actor.Position.X;
				int _y = dirY == -1 ? 15 : dirY == 1 ? 0 : actor.Position.Y;

				actor.SetPosition(_x, _y);

				newMapChunk.AddActor(actor, actor.Position);
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

				void ProcessMapChunk (object? obj)
				{
					if (obj != null && obj is MapChunk mapChunk)
					{
						Point globalPosition = mapChunk.GlobalPosition - node_camera.GlobalPosition;

						if (wasFloorsScreenDirty)
						{
							foreach (Floor floor in mapChunk.Floors)
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
							foreach (Wall wall in mapChunk.Walls)
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
							foreach (Actor actor in mapChunk.Actors)
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

				int activeChunksLeftCount = node_map.ActiveMapChunks.Count;

				using (ManualResetEvent resetEvent = new ManualResetEvent(false))
				{
					for (int i = 0; i < node_map.ActiveMapChunks.Count; i++)
					{
						ThreadPool.QueueUserWorkItem(
							new WaitCallback(x => {
								ProcessMapChunk(x);
								if (Interlocked.Decrement(ref activeChunksLeftCount) == 0)
								{
									resetEvent.Set();
								}
							}), node_map.ActiveMapChunks[i]);
					}
					
					resetEvent.WaitOne();
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

			if (node_messageLogScreen.IsDirty)
			{
				node_messageLogScreen.IsDirty = false;
				UpdateUI();
			}
		}

		if (node_screens.IsVisible && node_screen.IsVisible)
		{
			DrawScreenGroup(node_gameScreens, node_screen);
			DrawScreenGroup(node_uiScreens, node_screen);
			DrawScreenGroup(node_borderScreens, node_screen);
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

		void ProcessChunk(object? obj)
		{
			if (obj != null && obj is MapChunk mapChunk)
			{
				Point globalPosition = mapChunk.GlobalPosition;

				foreach (Floor floor in mapChunk.Floors)
				{
					Point floorGlobalPosition = floor.Position + globalPosition;
					if (position == floorGlobalPosition)
					{
						isFloor = true;
						break;
					}
				}
				foreach (Wall wall in mapChunk.Walls)
				{
					Point wallGlobalPosition = wall.Position + globalPosition;
					if (position == wallGlobalPosition)
					{
						isWall = true;
						break;
					}
				}
				foreach (Actor actor in mapChunk.Actors)
				{
					Point actorGlobalPosition = actor.Position + globalPosition;
					if (position == actorGlobalPosition)
					{
						isActor = true;
						break;
					}
				}
			}
		}

		int activeChunksLeftCount = node_map.ActiveMapChunks.Count;

		using (ManualResetEvent resetEvent = new ManualResetEvent(false))
		{
			for (int i = 0; i < node_map.ActiveMapChunks.Count; i++)
			{
				ThreadPool.QueueUserWorkItem(
					new WaitCallback(x => {
						ProcessChunk(x);
						if (Interlocked.Decrement(ref activeChunksLeftCount) == 0)
						{
							resetEvent.Set();
						}
					}),node_map.ActiveMapChunks[i]);
			}

			resetEvent.WaitOne();
		}

		return isFloor && !isWall && !isPlayer && !isActor;
	}

	public bool IsClear(int x, int y)
	{
		return IsClear(new Point(x, y));
	}

	#endregion // Public methods



	#region Private methods

	private void UpdateLoadedMaps(int mapChunkPositionX, int mapChunkPositionY)
	{
		node_screen.IsDirty = true;
		node_floorsScreen.IsDirty = true;
		node_wallsScreen.IsDirty = true;
		node_actorsScreen.IsDirty = true;
		node_playerScreen.IsDirty = true;
		node_messageLogScreen.IsDirty = true;

		m_currentMapChunkPosition = (mapChunkPositionX, mapChunkPositionY);

		(int x, int y) _chunk = (mapChunkPositionX, mapChunkPositionY);

		for (int i = -1; i >= -node_map.MapChunkLoadDistance; i--)
		{
			if (!node_map.IsMapChunkAtPosition(mapChunkPositionX + i, mapChunkPositionY))
			{
				_chunk.x += 1;
			}
			if (!node_map.IsMapChunkAtPosition(mapChunkPositionX, mapChunkPositionY + i))
			{
				_chunk.y += 1;
			}
		}
		for (int i = 1; i <= node_map.MapChunkLoadDistance; i++)
		{
			if (!node_map.IsMapChunkAtPosition(mapChunkPositionX + i, mapChunkPositionY))
			{
				_chunk.x -= 1;
			}
			if (!node_map.IsMapChunkAtPosition(mapChunkPositionX, mapChunkPositionY + i))
			{
				_chunk.y -= 1;
			}
		}

		List<MapChunk> newMapChunks = new List<MapChunk>();

		for (int i = -node_map.MapChunkLoadDistance; i <= node_map.MapChunkLoadDistance; i++)
		{
			for (int j = -node_map.MapChunkLoadDistance; j <= node_map.MapChunkLoadDistance; j++)
			{
				int x = _chunk.x + j;
				int y = _chunk.y + i;
				MapChunk mapChunk = node_map.GetMapChunk(x, y);

				newMapChunks.Add(mapChunk);

				if (!node_map.IsMapChunkLoaded(mapChunk))
				{
					node_map.LoadMapChunk(mapChunk);
				}
			}
		}

		for (int i = 0; i < node_map.ActiveMapChunks.Count; i++)
		{
			MapChunk mapChunk = node_map.ActiveMapChunks[i];

			if (!newMapChunks.Contains(mapChunk))
			{
				node_map.UnloadMapChunk(mapChunk);
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
		Point currentMapChunkPosition = new Point(m_currentMapChunkPosition.x, m_currentMapChunkPosition.y);
		MapChunk currentMapChunk = node_map.GetMapChunk(currentMapChunkPosition);

		int left = node_map.AllMapChunks.Min(_ => _.GlobalPosition.X);
		int right = node_map.AllMapChunks.Max(_ => _.GlobalPosition.X) + currentMapChunk.Width - 1;
		int up = node_map.AllMapChunks.Min(_ => _.GlobalPosition.Y);
		int down = node_map.AllMapChunks.Max(_ => _.GlobalPosition.Y) + currentMapChunk.Height - 1;

		node_camera.SetBounds(left, right, up, down);
	}

	private void TickCamera()
	{
		node_camera.CenterOnPosition(node_player.GlobalPosition);
		node_camera.ClampToBounds();
	}

	private void UpdateUI()
	{
		char[][] borderStyles = {
			new char[] { '─', '─', '│', '│', '┌', '┐', '└', '┘' },
			new char[] { '-', '-', '|', '|', '+', '+', '+', '+' }
		};

		void DrawBorder (Screen screen, int style, string? title = null)
		{
			screen.Clear();

			screen.FillRow(0, borderStyles[style][0]);
			screen.FillRow(screen.Height - 1, borderStyles[style][1]);
			screen.FillColumn(0, borderStyles[style][2]);
			screen.FillColumn(screen.Width - 1, borderStyles[style][3]);
			screen.SetSymbol(0, 0, borderStyles[style][4]);
			screen.SetSymbol(screen.Width - 1, 0, borderStyles[style][5]);
			screen.SetSymbol(0, screen.Height - 1, borderStyles[style][6]);
			screen.SetSymbol(screen.Width - 1, screen.Height - 1, borderStyles[style][7]);

			if (title != null)
			{
				screen.DrawText(2, 0, Screen.EDirection.Right, title);
			}
		}

		DrawBorder(node_gameScreenBorder, 1, "Map");
		DrawBorder(node_attributesScreenBorder, 1, "Attributes");
		DrawBorder(node_messageLogScreenBorder, 1, "Messages");
		DrawBorder(node_menuScreenBorder, 1, "Seen");
		DrawBorder(node_statusScreenBorder, 1, "Status");

		node_attributesScreen.DrawText(0, 0, Screen.EDirection.Right, $"Hp:{node_player.Health}/{node_player.MaxHealth} Mp: 7/ 7 Ep: 10 Rep: 0 Food: 4");
		node_attributesScreen.DrawText(0, 1, Screen.EDirection.Right, "Melee: 60%");
		node_attributesScreen.DrawText(node_attributesScreen.Width - 1, 1, Screen.EDirection.Left, "Vision: 3 Noise: 7");
		node_attributesScreen.DrawText(node_attributesScreen.Width - 1, 2, Screen.EDirection.Left, "Thievery: 25%");

		node_menuScreen.DrawText(0, 0, Screen.EDirection.Right, "Start menu");
		node_menuScreen.DrawText(0, 1, Screen.EDirection.Right, "> Start Game");
		node_menuScreen.DrawText(0, 2, Screen.EDirection.Right, "  Delete Game");
		node_menuScreen.DrawText(0, 3, Screen.EDirection.Right, "  Tutorial");
		node_menuScreen.DrawText(0, 4, Screen.EDirection.Right, "  Options");
		node_menuScreen.DrawText(0, 5, Screen.EDirection.Right, "  Exit Game");

		node_statusScreen.DrawText(1, 1, Screen.EDirection.Right, "exploring");
		node_statusScreen.DrawText(1, 2, Screen.EDirection.Right, "martial combo<1>");
		node_statusScreen.DrawText(1, 3, Screen.EDirection.Right, "duel wield");
		node_statusScreen.DrawText(1, 4, Screen.EDirection.Right, "in light");

		Point currentMapChunkPosition = new Point(m_currentMapChunkPosition.x, m_currentMapChunkPosition.y);
		MapChunk currentMapChunk = node_map.GetMapChunk(currentMapChunkPosition);

		node_messageLogScreen.DrawText(1, 0, Screen.EDirection.Right, $"Player: x={node_player.Position.X} y={node_player.Position.Y} gx={node_player.GlobalPosition.X} gy={node_player.GlobalPosition.Y} cx={m_currentMapChunkPosition.x} cy={m_currentMapChunkPosition.y}");
		node_messageLogScreen.DrawText(1, 1, Screen.EDirection.Right, $"Camera position: x={node_camera.Position.X} y={node_camera.Position.Y} gx={node_camera.GlobalPosition.X} gy={node_camera.GlobalPosition.Y} w={node_camera.Width} h={node_camera.Height}");
		node_messageLogScreen.DrawText(1, 2, Screen.EDirection.Right, $"Camera bounds: left={node_camera.Bounds.Left} right={node_camera.Bounds.Right} up={node_camera.Bounds.Up} down={node_camera.Bounds.Down}");
		node_messageLogScreen.DrawText(1, 3, Screen.EDirection.Right, $"Map chunks: all={node_map.AllMapChunks.Count} active={node_map.ActiveMapChunks.Count}");
		node_messageLogScreen.DrawText(1, 4, Screen.EDirection.Right, $"Actors: all={node_map.AllActors.Count} active={node_map.ActiveActors.Count} chunk={currentMapChunk.Actors.Count}");
		node_messageLogScreen.DrawText(1, 5, Screen.EDirection.Right, $"Steps: {m_steps}");
	}

	#endregion // Private methods

}

