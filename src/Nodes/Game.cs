public class Game : Node2D
{

	#region Nodes

	private Map node_map = new Map();

	private Player node_player = new Player('@', new Random());

	private ScreenGroup node_screens = new ScreenGroup();

	private Screen node_screen = new Screen();

	private ScreenGroup node_gameScreens = new ScreenGroup();
	private ScreenGroup node_uiScreens = new ScreenGroup();

	private Screen node_floorsScreen = new Screen();
	private Screen node_wallsScreen = new Screen();
	private Screen node_actorsScreen = new Screen();
	private Screen node_playerScreen = new Screen();
	private Screen node_uiScreen = new Screen();

	private Camera node_camera = new Camera();

	#endregion // Nodes



	#region Fields

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
		node_map = new Map("Map", 0, 0);

		node_player = new Player("Player", 0, 0, '@', rng);

		int screenX = 0;
		int screenY = 0;
		int screenWidth = 92;
		int screenHeight = 38;

		node_screen = new Screen("Screen", screenX, screenY, screenWidth, screenHeight);

		node_gameScreens = new ScreenGroup("GameScreens", 0, 0);
		node_uiScreens = new ScreenGroup("UIScreens", 0, 0);

		int gameWindowX = screenX;
		int gameWindowY = screenY;
		int gameWindowWidth = screenWidth;
		int gameWindowHeight = 22;

		int uiWindowX = screenX;
		int uiWindowY = 22;
		int uiWindowWidth = screenWidth;
		int uiWindowHeight = 16;

		node_floorsScreen = new Screen("FloorsScreen", gameWindowX, gameWindowY, gameWindowWidth, gameWindowHeight);
		node_wallsScreen = new Screen("WallsScreen", gameWindowX, gameWindowY, gameWindowWidth, gameWindowHeight);
		node_actorsScreen = new Screen("ActorsScreen", gameWindowX, gameWindowY, gameWindowWidth, gameWindowHeight);
		node_playerScreen = new Screen("PlayerScreen", gameWindowX, gameWindowY, gameWindowWidth, gameWindowHeight);
		node_uiScreen = new Screen("UIScreen", uiWindowX, uiWindowY, uiWindowWidth, uiWindowHeight);

		node_gameScreens.AddChild(node_floorsScreen);
		node_gameScreens.AddChild(node_wallsScreen);
		node_gameScreens.AddChild(node_actorsScreen);
		node_gameScreens.AddChild(node_playerScreen);
		node_uiScreens.AddChild(node_uiScreen);

		node_camera = new Camera("Camera", screenX, screenY, screenWidth, gameWindowHeight);

		AddChild(node_map);
		AddChild(node_player);
		AddChild(node_screen);
		AddChild(node_gameScreens);
		AddChild(node_uiScreens);
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
			node_uiScreen.IsDirty = true;
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
			node_uiScreen.IsDirty = true;
		};

		a_OnPlayerPositionChanged = delegate (object? sender, PointChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_playerScreen.IsDirty = true;
			node_uiScreen.IsDirty = true;

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
			node_uiScreen.IsDirty = true;
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

				System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

				int count = node_map.ActiveMapChunks.Count;

				using (ManualResetEvent resetEvent = new ManualResetEvent(false))
				{
					for (int i = 0; i < node_map.ActiveMapChunks.Count; i++)
					{
						ThreadPool.QueueUserWorkItem(
								new WaitCallback(x => {
									ProcessMapChunk(x);
									if (Interlocked.Decrement(ref count) == 0)
									{
										resetEvent.Set();
									}
								}), node_map.ActiveMapChunks[i]);
					}
					
					resetEvent.WaitOne();
				}

				sw.Stop();

				File.AppendAllText("timelog.txt", sw.Elapsed.ToString() + "\n");
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

		foreach (MapChunk mapChunk in node_map.ActiveMapChunks)
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

	private void UpdateLoadedMaps(int mapChunkPositionX, int mapChunkPositionY)
	{
		node_screen.IsDirty = true;
		node_floorsScreen.IsDirty = true;
		node_wallsScreen.IsDirty = true;
		node_actorsScreen.IsDirty = true;
		node_playerScreen.IsDirty = true;
		node_uiScreen.IsDirty = true;

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
		node_uiScreen.Clear();

		node_uiScreen.FillRow(0, '─');
		node_uiScreen.FillRow(node_uiScreen.Height - 1, '─');
		node_uiScreen.FillColumn(0, '│');
		node_uiScreen.FillColumn(node_uiScreen.Width - 1, '│');
		node_uiScreen.SetSymbol(0, 0, '┌');
		node_uiScreen.SetSymbol(node_uiScreen.Width - 1, 0, '┐');
		node_uiScreen.SetSymbol(0, node_uiScreen.Height - 1, '└');
		node_uiScreen.SetSymbol(node_uiScreen.Width - 1, node_uiScreen.Height - 1, '┘');

		node_uiScreen.DrawText(1, 1, Screen.EDirection.Right, $"Player: x={node_player.Position.X} y={node_player.Position.Y} gx={node_player.GlobalPosition.X} gy={node_player.GlobalPosition.Y} cx={m_currentMapChunkPosition.x} cy={m_currentMapChunkPosition.y}");
		node_uiScreen.DrawText(node_uiScreen.Width - 2, 1, Screen.EDirection.Left, $"Steps: {m_steps}");
		node_uiScreen.DrawText(1, 2, Screen.EDirection.Right, $"Camera position: x={node_camera.Position.X} y={node_camera.Position.Y} gx={node_camera.GlobalPosition.X} gy={node_camera.GlobalPosition.Y} w={node_camera.Width} h={node_camera.Height}");
		node_uiScreen.DrawText(1, 3, Screen.EDirection.Right, $"Camera bounds: left={node_camera.Bounds.Left} right={node_camera.Bounds.Right} up={node_camera.Bounds.Up} down={node_camera.Bounds.Down}");
		node_uiScreen.DrawText(1, 4, Screen.EDirection.Right, $"Maps: all={node_map.AllMapChunks.Count} active={node_map.ActiveMapChunks.Count}");
		node_uiScreen.DrawText(node_uiScreen.Width - 2, 4, Screen.EDirection.Left, $"Actors: all={node_map.AllActors.Count} active={node_map.ActiveActors.Count}");

		Point currentMapChunkPosition = new Point(m_currentMapChunkPosition.x, m_currentMapChunkPosition.y);
		MapChunk currentMapChunk = node_map.GetMapChunk(currentMapChunkPosition);

		node_uiScreen.DrawText(1, 5, Screen.EDirection.Right, $"Actors in this chunk: {currentMapChunk.Actors.Count}");
	}

	#endregion // Private methods

}

