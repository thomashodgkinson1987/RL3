public class Game : Node2D
{

	#region Nodes

	private readonly Map node_map;

	private readonly Player node_player;

	private readonly Screen node_screen;

	private readonly TitleWindow node_window_title;
	private readonly MapWindow node_window_map;
	private readonly AttributesWindow node_window_attributes;
	private readonly MessagesWindow node_window_messages;
	private readonly MenuWindow node_window_menu;
	private readonly StatusWindow node_window_status;

	private readonly Camera node_camera;

	#endregion // Nodes



	#region Properties

	public Map Map => node_map;
	public Player Player => node_player;
	public Camera Camera => node_camera;
	public int Steps { get; set; }
	public Point CurrentMapChunkPosition
	{
		get
		{
			Point point = new Point();
			point.X = m_currentMapChunkPosition.x;
			point.Y = m_currentMapChunkPosition.y;
			return point;
		}
	}
	public int ActiveWindowIndex { get; set; }
	public List<Window> Windows { get; }

	#endregion // Properties



	#region Fields

	public const int SCREEN_X = 0;
	public const int SCREEN_Y = 0;
	public const int SCREEN_W = 92;
	public const int SCREEN_H = 38;

	public const int WINDOW_MAP_X = 0;
	public const int WINDOW_MAP_Y = 0;
	public const int WINDOW_MAP_W = 62;
	public const int WINDOW_MAP_H = 22;

	public const int WINDOW_ATTRIBUTES_X = 0;
	public const int WINDOW_ATTRIBUTES_Y = 22;
	public const int WINDOW_ATTRIBUTES_W = 62;
	public const int WINDOW_ATTRIBUTES_H = 5;

	public const int WINDOW_MESSAGES_X = 0;
	public const int WINDOW_MESSAGES_Y = 27;
	public const int WINDOW_MESSAGES_W = 62;
	public const int WINDOW_MESSAGES_H = 11;

	public const int WINDOW_MENU_X = 62;
	public const int WINDOW_MENU_Y = 0;
	public const int WINDOW_MENU_W = 30;
	public const int WINDOW_MENU_H = 27;

	public const int WINDOW_STATUS_X = 62;
	public const int WINDOW_STATUS_Y = 27;
	public const int WINDOW_STATUS_W = 30;
	public const int WINDOW_STATUS_H = 11;

	private readonly Random m_rng;

	private (int x, int y) m_currentMapChunkPosition;

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

	private EventHandler? a_OnStartGameButtonSelected;
	private EventHandler? a_OnStartGameButtonUnselected;
	private EventHandler? a_OnStartGameButtonPressed;

	private EventHandler? a_OnDeleteGameButtonSelected;
	private EventHandler? a_OnDeleteGameButtonUnselected;
	private EventHandler? a_OnDeleteGameButtonPressed;

	private EventHandler? a_OnTutorialButtonSelected;
	private EventHandler? a_OnTutorialButtonUnselected;
	private EventHandler? a_OnTutorialButtonPressed;

	private EventHandler? a_OnOptionsButtonSelected;
	private EventHandler? a_OnOptionsButtonUnselected;
	private EventHandler? a_OnOptionsButtonPressed;

	private EventHandler? a_OnExitGameButtonSelected;
	private EventHandler? a_OnExitGameButtonUnselected;
	private EventHandler? a_OnExitGameButtonPressed;

	private EventHandler? a_OnPressedEscape;

	#endregion // Fields



	#region Constructors

	public Game(string name, int x, int y, Random rng) : base(name, x, y)
	{
		node_map = new Map();

		AddChild(node_map);

		node_player = new Player();

		AddChild(node_player);

		node_screen = new Screen("Screen", SCREEN_X, SCREEN_Y, SCREEN_W, SCREEN_H);

		AddChild(node_screen);

		node_window_title = new TitleWindow(WINDOW_MAP_X, WINDOW_MAP_Y, WINDOW_MAP_W, WINDOW_MAP_H, Window.EBorderStyle.DashedPlus);
		node_window_map = new MapWindow(WINDOW_MAP_X, WINDOW_MAP_Y, this, WINDOW_MAP_W, WINDOW_MAP_H, Window.EBorderStyle.DashedPlus);
		node_window_attributes = new AttributesWindow(WINDOW_ATTRIBUTES_X, WINDOW_ATTRIBUTES_Y, this, WINDOW_ATTRIBUTES_W, WINDOW_ATTRIBUTES_H, Window.EBorderStyle.DashedPlus);
		node_window_messages = new MessagesWindow(WINDOW_MESSAGES_X, WINDOW_MESSAGES_Y, this, WINDOW_MESSAGES_W, WINDOW_MESSAGES_H, Window.EBorderStyle.DashedPlus);
		node_window_menu = new MenuWindow(WINDOW_MENU_X, WINDOW_MENU_Y, WINDOW_MENU_W, WINDOW_MENU_H, Window.EBorderStyle.DashedPlus);
		node_window_status = new StatusWindow(WINDOW_STATUS_X, WINDOW_STATUS_Y, this, WINDOW_STATUS_W, WINDOW_STATUS_H, Window.EBorderStyle.DashedPlus);

		node_window_map.IsVisible = false;

		AddChild(node_window_title);
		AddChild(node_window_map);
		AddChild(node_window_attributes);
		AddChild(node_window_messages);
		AddChild(node_window_menu);
		AddChild(node_window_status);

		node_camera = new Camera("Camera", WINDOW_MAP_X + 1, WINDOW_MAP_Y + 1, WINDOW_MAP_W - 2, WINDOW_MAP_H - 2);

		AddChild(node_camera);

		m_rng = rng;

		m_currentMapChunkPosition = (0, 0);

		Steps = 0;

		Windows = new List<Window>()
		{
			node_window_title,
			node_window_map,
			node_window_attributes,
			node_window_messages,
			node_window_menu,
			node_window_status
		};

		ActiveWindowIndex = 4;
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
			Point globalPosition = args.MapChunk.GlobalPosition;

			int left = globalPosition.X;
			int right = globalPosition.X + args.MapChunk.Width - 1;
			int up = globalPosition.Y;
			int down = globalPosition.Y + args.MapChunk.Height - 1;

			node_camera.ExpandBoundsTo(left, right, up, down);
		};

		a_OnMapChunkRemoved = delegate (object? sender, MapChunkRemovedEventArgs args)
		{
			RecalculateCameraBounds();
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
			node_window_map.IsDirty = true;
			node_window_messages.IsDirty = true;
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
			node_window_map.IsDirty = true;
			node_window_messages.IsDirty = true;
		};

		a_OnPlayerPositionChanged = delegate (object? sender, PointChangedEventArgs args)
		{
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

			node_screen.IsDirty = true;
			node_window_map.IsDirty = true;
			node_window_messages.IsDirty = true;
		};

		a_OnPlayerHealthChanged = delegate (object? sender, IntChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_window_messages.IsDirty = true;
		};

		a_OnPlayerMaxHealthChanged = delegate (object? sender, IntChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_window_messages.IsDirty = true;
		};

		a_OnActorPositionChanged = delegate (object? sender, PointChangedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_window_map.IsDirty = true;

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
			node_window_map.IsDirty = true;
			node_window_messages.IsDirty = true;
		};

		a_OnFloorAddedToMapChunk = delegate (object? sender, FloorAddedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_window_map.IsDirty = true;
		};

		a_OnFloorRemovedFromMapChunk = delegate (object? sender, FloorRemovedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_window_map.IsDirty = true;
		};

		a_OnWallAddedToMapChunk = delegate (object? sender, WallAddedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_window_map.IsDirty = true;
		};

		a_OnWallRemovedFromMapChunk = delegate (object? sender, WallRemovedEventArgs args)
		{
			node_screen.IsDirty = true;
			node_window_map.IsDirty = true;
		};

		a_OnActorAddedToMapChunk = delegate (object? sender, ActorAddedEventArgs args)
		{
			args.Actor.PositionChanged += a_OnActorPositionChanged;

			node_screen.IsDirty = true;
			node_window_map.IsDirty = true;
		};

		a_OnActorRemovedFromMapChunk = delegate (object? sender, ActorRemovedEventArgs args)
		{
			args.Actor.PositionChanged -= a_OnActorPositionChanged;

			node_screen.IsDirty = true;
			node_window_map.IsDirty = true;
		};

		a_OnStartGameButtonSelected = delegate (object? sender, EventArgs e)
		{
			node_screen.IsDirty = true;
			node_window_messages.IsDirty = true;
		};
		a_OnStartGameButtonUnselected = delegate (object? sender, EventArgs e) { };
		a_OnStartGameButtonPressed = delegate (object? sender, EventArgs e)
		{
			node_screen.IsDirty = true;
			node_window_map.IsDirty = true;
			node_window_attributes.IsDirty = true;
			node_window_messages.IsDirty = true;
			node_window_status.IsDirty = true;

			ActiveWindowIndex = 1;
			node_window_map.IsVisible = true;
			node_window_title.IsVisible = false;
		};

		a_OnDeleteGameButtonSelected = delegate (object? sender, EventArgs e)
		{
			node_screen.IsDirty = true;
			node_window_messages.IsDirty = true;
		};
		a_OnDeleteGameButtonUnselected = delegate (object? sender, EventArgs e) { };
		a_OnDeleteGameButtonPressed = delegate (object? sender, EventArgs e) { };

		a_OnTutorialButtonSelected = delegate (object? sender, EventArgs e)
		{
			node_screen.IsDirty = true;
			node_window_messages.IsDirty = true;
		};
		a_OnTutorialButtonUnselected = delegate (object? sender, EventArgs e) { };
		a_OnTutorialButtonPressed = delegate (object? sender, EventArgs e) { };

		a_OnOptionsButtonSelected = delegate (object? sender, EventArgs e)
		{
			node_screen.IsDirty = true;
			node_window_messages.IsDirty = true;
		};
		a_OnOptionsButtonUnselected = delegate (object? sender, EventArgs e) { };
		a_OnOptionsButtonPressed = delegate (object? sender, EventArgs e) { };

		a_OnExitGameButtonSelected = delegate (object? sender, EventArgs e)
		{
			node_screen.IsDirty = true;
			node_window_messages.IsDirty = true;
		};
		a_OnExitGameButtonUnselected = delegate (object? sender, EventArgs e) { };
		a_OnExitGameButtonPressed = delegate (object? sender, EventArgs e)
		{
			RootNode.Instance.Quit();
		};

		a_OnPressedEscape = delegate (object? sender, EventArgs e)
		{
			node_window_title.IsVisible = true;
			node_window_map.IsVisible = false;
			ActiveWindowIndex = 4;

			node_screen.IsDirty = true;
			node_window_title.IsDirty = true;
		};

		node_map.MapChunkAdded += a_OnMapChunkAdded;
		node_map.MapChunkRemoved += a_OnMapChunkRemoved;
		node_map.MapChunkLoaded += a_OnMapChunkLoaded;
		node_map.MapChunkUnloaded += a_OnMapChunkUnloaded;

		node_player.PositionChanged += a_OnPlayerPositionChanged;
		node_player.HealthChanged += a_OnPlayerHealthChanged;
		node_player.MaxHealthChanged += a_OnPlayerMaxHealthChanged;

		node_camera.PositionChanged += a_OnCameraPositionChanged;

		node_window_menu.StartGameButton.Selected += a_OnStartGameButtonSelected;
		node_window_menu.StartGameButton.Unselected += a_OnStartGameButtonUnselected;
		node_window_menu.StartGameButton.Pressed += a_OnStartGameButtonPressed;

		node_window_menu.DeleteGameButton.Selected += a_OnDeleteGameButtonSelected;
		node_window_menu.DeleteGameButton.Unselected += a_OnDeleteGameButtonUnselected;
		node_window_menu.DeleteGameButton.Pressed += a_OnDeleteGameButtonPressed;

		node_window_menu.TutorialButton.Selected += a_OnTutorialButtonSelected;
		node_window_menu.TutorialButton.Unselected += a_OnTutorialButtonUnselected;
		node_window_menu.TutorialButton.Pressed += a_OnTutorialButtonPressed;

		node_window_menu.OptionsButton.Selected += a_OnOptionsButtonSelected;
		node_window_menu.OptionsButton.Unselected += a_OnOptionsButtonUnselected;
		node_window_menu.OptionsButton.Pressed += a_OnOptionsButtonPressed;

		node_window_menu.ExitGameButton.Selected += a_OnExitGameButtonSelected;
		node_window_menu.ExitGameButton.Unselected += a_OnExitGameButtonUnselected;
		node_window_menu.ExitGameButton.Pressed += a_OnExitGameButtonPressed;

		node_window_map.PressedEscapeOnMapWindow += a_OnPressedEscape;
	}

	public override void Ready()
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
				else if (c == 5) mapChunk = StaticMapChunks.GetMapChunk6();

				for (int k = 0; k < m_rng.Next(0, 5); k++)
				{
					int x = m_rng.Next(0, 15);
					int y = m_rng.Next(0, 15);

					NPC npc = new NPC(x, y, m_rng);
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

	public override void ProcessInput(ConsoleKeyInfo consoleKeyInfo)
	{
		Windows[ActiveWindowIndex].ProcessInput(consoleKeyInfo);
	}

	public override void InputTick(float delta)
	{
		Windows[ActiveWindowIndex].InputTick(delta);
	}

	public override void FixedTick(float delta)
	{
		Windows[ActiveWindowIndex].FixedTick(delta);
	}

	public override void Tick(float delta)
	{
		Windows[ActiveWindowIndex].Tick(delta);
	}

	#endregion // Node2D methods



	#region Public methods

	public void Draw()
	{
		if (node_screen.IsDirty)
		{
			node_screen.IsDirty = false;
			node_screen.Clear();

			foreach (Window window in Windows)
			{
				if (window.IsDirty)
				{
					window.IsDirty = false;
					window.Refresh();
				}
			}

			if (node_screen.IsVisible)
			{
				foreach (Window window in Windows)
				{
					DrawWindow(window, node_screen);
				}
			}

			for (int i = 0; i < node_screen.Height; i++)
			{
				Console.SetCursorPosition(0, i);
				for (int j = 0; j < node_screen.Width; j++)
				{
					Console.BackgroundColor = node_screen.Pixels[i,j].BackgroundColor;
					Console.ForegroundColor = node_screen.Pixels[i,j].ForegroundColor;
					Console.Write(node_screen.Pixels[i,j].Symbol);
				}
			}
		}
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
					new WaitCallback(x =>
					{
						ProcessChunk(x);
						if (Interlocked.Decrement(ref activeChunksLeftCount) == 0)
						{
							resetEvent.Set();
						}
					}), node_map.ActiveMapChunks[i]);
			}
			resetEvent.WaitOne();
		}

		return isFloor && !isWall && !isPlayer && !isActor;
	}

	public bool IsClear(int x, int y)
	{
		return IsClear(new Point(x, y));
	}

	public void TickPlayer()
	{
		Point position = node_player.Position;
		Point globalPosition = node_player.GlobalPosition;

		ConsoleKeyInfo consoleKeyInfo = Input.LastConsoleKeyInfo;

		if (consoleKeyInfo.KeyChar == '4')
		{
			if (IsClear(globalPosition.X - 1, globalPosition.Y))
			{
				position.X--;
			}
		}
		else if (consoleKeyInfo.KeyChar == '6')
		{
			if (IsClear(globalPosition.X + 1, globalPosition.Y))
			{
				position.X++;
			}
		}
		else if (consoleKeyInfo.KeyChar == '8')
		{
			if (IsClear(globalPosition.X, globalPosition.Y - 1))
			{
				position.Y--;
			}
		}
		else if (consoleKeyInfo.KeyChar == '2')
		{
			if (IsClear(globalPosition.X, globalPosition.Y + 1))
			{
				position.Y++;
			}
		}
		else if (consoleKeyInfo.KeyChar == '7')
		{
			if (IsClear(globalPosition.X - 1, globalPosition.Y - 1))
			{
				position.X--;
				position.Y--;
			}
		}
		else if (consoleKeyInfo.KeyChar == '9')
		{
			if (IsClear(globalPosition.X + 1, globalPosition.Y - 1))
			{
				position.X++;
				position.Y--;
			}
		}
		else if (consoleKeyInfo.KeyChar == '1')
		{
			if (IsClear(globalPosition.X - 1, globalPosition.Y + 1))
			{
				position.X--;
				position.Y++;
			}
		}
		else if (consoleKeyInfo.KeyChar == '3')
		{
			if (IsClear(globalPosition.X + 1, globalPosition.Y + 1))
			{
				position.X++;
				position.Y++;
			}
		}

		node_player.SetPosition(position);

		Point oldMapChunkPosition = new Point(m_currentMapChunkPosition.x, m_currentMapChunkPosition.y);

		int newMapChunkPositionX = (int)MathF.Floor(node_player.GlobalPosition.X / 16f);
		int newMapChunkPositionY = (int)MathF.Floor(node_player.GlobalPosition.Y / 16f);
		Point newMapChunkPosition = new Point(newMapChunkPositionX, newMapChunkPositionY);

		if (oldMapChunkPosition != newMapChunkPosition)
		{
			UpdateLoadedMapChunks(newMapChunkPositionX, newMapChunkPositionY);
		}
	}

	public void TickActors(float delta)
	{
		foreach (Actor actor in node_map.ActiveActors)
		{
			Point oldActorPosition = actor.Position;
			Point oldActorGlobalPosition = actor.GlobalPosition;
			int oldMapChunkPositionX = (int)MathF.Floor(oldActorGlobalPosition.X / 16f);
			int oldMapChunkPositionY = (int)MathF.Floor(oldActorGlobalPosition.Y / 16f);
			Point oldMapChunkPosition = new Point(oldMapChunkPositionX, oldMapChunkPositionY);

			actor.Tick(delta);

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
	}

	public void TickCamera()
	{
		node_camera.CenterOnPosition(node_player.GlobalPosition);
		node_camera.ClampToBounds();
	}

	#endregion // Public methods



	#region Private methods

	private void UpdateLoadedMapChunks(int mapChunkPositionX, int mapChunkPositionY)
	{
		node_screen.IsDirty = true;
		node_window_map.IsDirty = true;
		node_window_messages.IsDirty = true;

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

	private void DrawWindow(Window window, Screen target)
	{
		if (!window.IsVisible) return;

		Point windowGlobalPosition = window.GlobalPosition;

		target.DrawLine(windowGlobalPosition.X, windowGlobalPosition.Y + 1, Screen.EDirection.Down, window.Height - 2, ConsoleColor.Black, ConsoleColor.White, '|', false);
		target.DrawLine(windowGlobalPosition.X + window.Width - 1, windowGlobalPosition.Y + 1, Screen.EDirection.Down, window.Height - 2, ConsoleColor.Black, ConsoleColor.White, '|', false);
		target.DrawLine(windowGlobalPosition.X + 1, windowGlobalPosition.Y, Screen.EDirection.Right, window.Width - 2, ConsoleColor.Black, ConsoleColor.White, '-', false);
		target.DrawLine(windowGlobalPosition.X + 1, windowGlobalPosition.Y + window.Height - 1, Screen.EDirection.Right, window.Width - 2, ConsoleColor.Black, ConsoleColor.White, '-', false);
		target.SetPixel(windowGlobalPosition.X, windowGlobalPosition.Y, ConsoleColor.Black, ConsoleColor.White, '+', false);
		target.SetPixel(windowGlobalPosition.X + window.Width - 1, windowGlobalPosition.Y, ConsoleColor.Black, ConsoleColor.White, '+', false);
		target.SetPixel(windowGlobalPosition.X, windowGlobalPosition.Y + window.Height - 1, ConsoleColor.Black, ConsoleColor.White, '+', false);
		target.SetPixel(windowGlobalPosition.X + window.Width - 1, windowGlobalPosition.Y + window.Height - 1, ConsoleColor.Black, ConsoleColor.White, '+', false);
		target.DrawText(windowGlobalPosition.X + 2, windowGlobalPosition.Y, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, window.Title, false);

		DrawScreen(window.Screen, target);
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

				Pixel pixel = screen.GetPixel(j, i);
				ConsoleColor bgc = pixel.BackgroundColor;
				ConsoleColor fgc = pixel.ForegroundColor;
				char symbol = pixel.Symbol;
				bool isTransparent = pixel.IsTransparent;

				if (!isTransparent)
				{
					targetScreen.SetPixel(x, y, bgc, fgc, symbol, isTransparent);
				}
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

	#endregion // Private methods

}

