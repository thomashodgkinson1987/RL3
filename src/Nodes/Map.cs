public class Map : Node2D
{

	#region Properties

	public List<MapChunk> AllMapChunks { get; }
	public List<MapChunk> ActiveMapChunks { get; }

	public List<Floor> AllFloors { get; }
	public List<Floor> ActiveFloors { get; }

	public List<Wall> AllWalls { get; }
	public List<Wall> ActiveWalls { get; }

	public List<Actor> AllActors { get; }
	public List<Actor> ActiveActors { get; }

	private int _mapChunkLoadDistance;
	public int MapChunkLoadDistance
	{
		get => _mapChunkLoadDistance;
		set
		{
			IntChangedEventArgs args = new IntChangedEventArgs();
			args.IntBeforeChange = _mapChunkLoadDistance;

			_mapChunkLoadDistance = value;

			args.IntAfterChange = _mapChunkLoadDistance;
			OnMapChunkLoadDistanceChanged(args);
		}
	}

	#endregion // Properties



	#region Events

	public event EventHandler<MapChunkAddedEventArgs>? MapChunkAdded;
	public event EventHandler<MapChunkRemovedEventArgs>? MapChunkRemoved;

	public event EventHandler<MapChunkLoadedEventArgs>? MapChunkLoaded;
	public event EventHandler<MapChunkUnloadedEventArgs>? MapChunkUnloaded;

	public event EventHandler<IntChangedEventArgs>? MapChunkLoadDistanceChanged;

	#endregion // Events



	#region Fields

	private readonly Dictionary<(int x, int y), MapChunk> m_mapChunks;

	#endregion // Fields



	#region Constructors

	public Map(string name, int x, int y) : base(name, x, y)
	{
		m_mapChunks = new Dictionary<(int x, int y), MapChunk>();

		AllMapChunks = new List<MapChunk>();
		ActiveMapChunks = new List<MapChunk>();

		AllFloors = new List<Floor>();
		ActiveFloors = new List<Floor>();

		AllWalls = new List<Wall>();
		ActiveWalls = new List<Wall>();

		AllActors = new List<Actor>();
		ActiveActors = new List<Actor>();

		_mapChunkLoadDistance = 2;
	}

	public Map(string name) : this(name, 0, 0) { }

	public Map(int x, int y) : this("Map", x, y) { }

	public Map() : this("Map", 0, 0) { }

	#endregion // Constructors



	#region Public methods

	public bool IsMapChunkAtPosition(Point position)
	{
		return m_mapChunks.ContainsKey((position.X, position.Y));
	}

	public bool IsMapChunkAtPosition(int x, int y)
	{
		return IsMapChunkAtPosition(new Point(x, y));
	}

	public bool IsMapChunkLoaded(Point position)
	{
		MapChunk mapChunk = GetMapChunk(position);
		return ActiveMapChunks.Contains(mapChunk);
	}

	public bool IsMapChunkLoaded(int x, int y)
	{
		return IsMapChunkLoaded(new Point(x, y));
	}

	public bool IsMapChunkLoaded(MapChunk mapChunk)
	{
		Point position = GetMapChunkPosition(mapChunk);
		return IsMapChunkLoaded(position);
	}

	public MapChunk GetMapChunk(Point position)
	{
		return m_mapChunks[(position.X, position.Y)];
	}

	public MapChunk GetMapChunk(int x, int y)
	{
		return GetMapChunk(new Point(x, y));
	}

	public Point GetMapChunkPosition(MapChunk mapChunk)
	{
		(int x, int y) position = (0, 0);

		foreach (KeyValuePair<(int x, int y), MapChunk> kvp in m_mapChunks)
		{
			if (kvp.Value == mapChunk)
			{
				position = kvp.Key;
				break;
			}
		}

		return new Point(position.x, position.y);
	}

	public void AddMapChunk(MapChunk mapChunk, Point position)
	{
		int x = mapChunk.Width * position.X;
		int y = mapChunk.Height * position.Y;
		mapChunk.SetPosition(x, y);

		m_mapChunks.Add((position.X, position.Y), mapChunk);
		AllMapChunks.Add(mapChunk);

		foreach (Floor floor in mapChunk.Floors)
		{
			AllFloors.Add(floor);
		}
		foreach (Wall wall in mapChunk.Walls)
		{
			AllWalls.Add(wall);
		}
		foreach(Actor actor in mapChunk.Actors)
		{
			AllActors.Add(actor);
		}

		AddChild(mapChunk);

		mapChunk.IsDirty = true;

		MapChunkAddedEventArgs args = new MapChunkAddedEventArgs();
		args.MapChunk = mapChunk;
		OnMapChunkAdded(args);
	}

	public void AddMapChunk(MapChunk mapChunk, int x, int y)
	{
		AddMapChunk(mapChunk, new Point(x, y));
	}

	public void RemoveMapChunk(MapChunk mapChunk)
	{
		UnloadMapChunk(mapChunk);

		foreach (Floor floor in mapChunk.Floors)
		{
			AllFloors.Remove(floor);
		}
		foreach (Wall wall in mapChunk.Walls)
		{
			AllWalls.Remove(wall);
		}
		foreach (Actor actor in mapChunk.Actors)
		{
			AllActors.Remove(actor);
		}

		AllMapChunks.Remove(mapChunk);

		Point position = GetMapChunkPosition(mapChunk);
		m_mapChunks.Remove((position.X, position.Y));

		RemoveChild(mapChunk);

		mapChunk.IsDirty = true;

		MapChunkRemovedEventArgs args = new MapChunkRemovedEventArgs();
		args.MapChunk = mapChunk;
		OnMapChunkRemoved(args);
	}

	public void RemoveMapChunk(Point position)
	{
		MapChunk mapChunk = GetMapChunk(position);
		RemoveMapChunk(mapChunk);
	}

	public void RemoveMapChunk(int x, int y)
	{
		RemoveMapChunk(new Point(x, y));
	}

	public void LoadMapChunk(MapChunk mapChunk)
	{
		ActiveMapChunks.Add(mapChunk);

		foreach (Floor floor in mapChunk.Floors)
		{
			ActiveFloors.Add(floor);
		}
		foreach (Wall wall in mapChunk.Walls)
		{
			ActiveWalls.Add(wall);
		}
		foreach (Actor actor in mapChunk.Actors)
		{
			ActiveActors.Add(actor);
		}

		mapChunk.IsDirty = true;

		MapChunkLoadedEventArgs args = new MapChunkLoadedEventArgs();
		args.MapChunk = mapChunk;
		OnMapChunkLoaded(args);
	}

	public void LoadMapChunk(Point position)
	{
		MapChunk mapChunk = GetMapChunk(position);
		LoadMapChunk(mapChunk);
	}

	public void LoadMapChunk(int x, int y)
	{
		LoadMapChunk(new Point(x, y));
	}

	public void UnloadMapChunk(MapChunk mapChunk)
	{
		ActiveMapChunks.Remove(mapChunk);

		foreach (Floor floor in mapChunk.Floors)
		{
			ActiveFloors.Remove(floor);
		}
		foreach (Wall wall in mapChunk.Walls)
		{
			ActiveWalls.Remove(wall);
		}
		foreach (Actor actor in mapChunk.Actors)
		{
			ActiveActors.Remove(actor);
		}

		mapChunk.IsDirty = true;

		MapChunkUnloadedEventArgs args = new MapChunkUnloadedEventArgs();
		args.MapChunk = mapChunk;
		OnMapChunkUnloaded(args);
	}

	public void UnloadMapChunk(Point position)
	{
		MapChunk mapChunk = GetMapChunk(position);
		UnloadMapChunk(mapChunk);
	}

	public void UnloadMapChunk(int x, int y)
	{
		UnloadMapChunk(new Point(x, y));
	}

	#endregion // Public methods



	#region Protected methods

	protected virtual void OnMapChunkAdded(MapChunkAddedEventArgs e)
	{
		EventHandler<MapChunkAddedEventArgs>? handler = MapChunkAdded;
		handler?.Invoke(this, e);
	}

	protected virtual void OnMapChunkRemoved(MapChunkRemovedEventArgs e)
	{
		EventHandler<MapChunkRemovedEventArgs>? handler = MapChunkRemoved;
		handler?.Invoke(this, e);
	}

	protected virtual void OnMapChunkLoaded(MapChunkLoadedEventArgs e)
	{
		EventHandler<MapChunkLoadedEventArgs>? handler = MapChunkLoaded;
		handler?.Invoke(this, e);
	}

	protected virtual void OnMapChunkUnloaded(MapChunkUnloadedEventArgs e)
	{
		EventHandler<MapChunkUnloadedEventArgs>? handler = MapChunkUnloaded;
		handler?.Invoke(this, e);
	}

	protected virtual void OnMapChunkLoadDistanceChanged(IntChangedEventArgs e)
	{
		EventHandler<IntChangedEventArgs>? handler = MapChunkLoadDistanceChanged;
		handler?.Invoke(this, e);
	}

	#endregion // Protected methods

}

