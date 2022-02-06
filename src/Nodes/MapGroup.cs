public class MapGroup : Node2D
{

	#region Properties

	public List<Map> AllMaps { get; }
	public List<Map> ActiveMaps { get; }

	public List<GameObject> AllFloors { get; }
	public List<GameObject> ActiveFloors { get; }

	public List<GameObject> AllWalls { get; }
	public List<GameObject> ActiveWalls { get; }

	public List<NPC> AllActors { get; }
	public List<NPC> ActiveActors { get; }

	private int _mapLoadDistance;
	public int MapLoadDistance
	{
		get => _mapLoadDistance;
		set
		{
			IntChangedEventArgs args = new IntChangedEventArgs();
			args.IntBeforeChange = _mapLoadDistance;

			_mapLoadDistance = value;

			args.IntAfterChange = _mapLoadDistance;
			OnMapLoadDistanceChanged(args);
		}
	}

	#endregion // Properties



	#region Events

	public event EventHandler<MapAddedEventArgs>? MapAdded;
	public event EventHandler<MapRemovedEventArgs>? MapRemoved;

	public event EventHandler<MapLoadedEventArgs>? MapLoaded;
	public event EventHandler<MapUnloadedEventArgs>? MapUnloaded;

	public event EventHandler<IntChangedEventArgs>? MapLoadDistanceChanged;

	#endregion // Events



	#region Fields

	private readonly Dictionary<(int x, int y), Map> m_maps;

	#endregion // Fields



	#region Constructors

	public MapGroup(string name, int x, int y) : base(name, x, y)
	{
		m_maps = new Dictionary<(int x, int y), Map>();

		AllMaps = new List<Map>();
		ActiveMaps = new List<Map>();

		AllFloors = new List<GameObject>();
		ActiveFloors = new List<GameObject>();

		AllWalls = new List<GameObject>();
		ActiveWalls = new List<GameObject>();

		AllActors = new List<NPC>();
		ActiveActors = new List<NPC>();

		_mapLoadDistance = 2;
	}

	public MapGroup(string name) : this(name, 0, 0) { }

	public MapGroup(int x, int y) : this("MapGroup", x, y) { }

	public MapGroup() : this("MapGroup", 0, 0) { }

	#endregion // Constructors



	#region Public methods

	public bool IsMapAtPosition(Point position)
	{
		return m_maps.ContainsKey((position.X, position.Y));
	}

	public bool IsMapAtPosition(int x, int y)
	{
		return IsMapAtPosition(new Point(x, y));
	}

	public bool IsMapLoaded(Point position)
	{
		Map map = GetMap(position);
		return ActiveMaps.Contains(map);
	}

	public bool IsMapLoaded(int x, int y)
	{
		return IsMapLoaded(new Point(x, y));
	}

	public bool IsMapLoaded(Map map)
	{
		Point position = GetMapPosition(map);
		return IsMapLoaded(position);
	}

	public Map GetMap(Point position)
	{
		return m_maps[(position.X, position.Y)];
	}

	public Map GetMap(int x, int y)
	{
		return GetMap(new Point(x, y));
	}

	public Point GetMapPosition(Map map)
	{
		(int x, int y) position = (0, 0);

		foreach (KeyValuePair<(int x, int y), Map> kvp in m_maps)
		{
			if (kvp.Value == map)
			{
				position = kvp.Key;
				break;
			}
		}

		return new Point(position.x, position.y);
	}

	public void AddMap(Map map, Point position)
	{
		int x = map.Width * position.X;
		int y = map.Height * position.Y;
		map.SetPosition(x, y);

		m_maps.Add((position.X, position.Y), map);

		AllMaps.Add(map);

		foreach (GameObject floor in map.Floors)
		{
			AllFloors.Add(floor);
		}
		foreach (GameObject wall in map.Walls)
		{
			AllWalls.Add(wall);
		}
		foreach(NPC actor in map.Actors)
		{
			AllActors.Add(actor);
		}

		AddChild(map);

		MapAddedEventArgs args = new MapAddedEventArgs();
		args.Map = map;
		OnMapAdded(args);
	}

	public void AddMap(Map map, int x, int y)
	{
		AddMap(map, new Point(x, y));
	}

	public void RemoveMap(Map map)
	{
		UnloadMap(map);

		foreach (GameObject floor in map.Floors)
		{
			AllFloors.Remove(floor);
		}
		foreach (GameObject wall in map.Walls)
		{
			AllWalls.Remove(wall);
		}
		foreach(NPC actor in map.Actors)
		{
			AllActors.Remove(actor);
		}

		AllMaps.Remove(map);

		Point position = GetMapPosition(map);
		m_maps.Remove((position.X, position.Y));

		RemoveChild(map);

		MapRemovedEventArgs args = new MapRemovedEventArgs();
		args.Map = map;
		OnMapRemoved(args);
	}

	public void RemoveMap(Point position)
	{
		Map map = GetMap(position);
		RemoveMap(map);
	}

	public void RemoveMap(int x, int y)
	{
		RemoveMap(new Point(x, y));
	}

	public void LoadMap(Map map)
	{
		ActiveMaps.Add(map);

		foreach (GameObject floor in map.Floors)
		{
			ActiveFloors.Add(floor);
		}
		foreach (GameObject wall in map.Walls)
		{
			ActiveWalls.Add(wall);
		}
		foreach(NPC actor in map.Actors)
		{
			ActiveActors.Add(actor);
		}

		MapLoadedEventArgs args = new MapLoadedEventArgs();
		args.Map = map;
		OnMapLoaded(args);
	}

	public void LoadMap(Point position)
	{
		Map map = GetMap(position);
		LoadMap(map);
	}

	public void LoadMap(int x, int y)
	{
		LoadMap(new Point(x, y));
	}

	public void UnloadMap(Map map)
	{
		ActiveMaps.Remove(map);

		foreach (GameObject floor in map.Floors)
		{
			ActiveFloors.Remove(floor);
		}
		foreach (GameObject wall in map.Walls)
		{
			ActiveWalls.Remove(wall);
		}
		foreach (NPC actor in map.Actors)
		{
			ActiveActors.Remove(actor);
		}

		MapUnloadedEventArgs args = new MapUnloadedEventArgs();
		args.Map = map;
		OnMapUnloaded(args);
	}

	public void UnloadMap(Point position)
	{
		Map map = GetMap(position);
		UnloadMap(map);
	}

	public void UnloadMap(int x, int y)
	{
		UnloadMap(new Point(x, y));
	}

	#endregion // Public methods



	#region Protected methods

	protected virtual void OnMapAdded(MapAddedEventArgs e)
	{
		EventHandler<MapAddedEventArgs>? handler = MapAdded;
		handler?.Invoke(this, e);
	}

	protected virtual void OnMapRemoved(MapRemovedEventArgs e)
	{
		EventHandler<MapRemovedEventArgs>? handler = MapRemoved;
		handler?.Invoke(this, e);
	}

	protected virtual void OnMapLoaded(MapLoadedEventArgs e)
	{
		EventHandler<MapLoadedEventArgs>? handler = MapLoaded;
		handler?.Invoke(this, e);
	}

	protected virtual void OnMapUnloaded(MapUnloadedEventArgs e)
	{
		EventHandler<MapUnloadedEventArgs>? handler = MapUnloaded;
		handler?.Invoke(this, e);
	}

	protected virtual void OnMapLoadDistanceChanged(IntChangedEventArgs e)
	{
		EventHandler<IntChangedEventArgs>? handler = MapLoadDistanceChanged;
		handler?.Invoke(this, e);
	}

	#endregion // Protected methods

}

