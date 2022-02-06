public class MapGroup : Node2D
{

	#region Properties

	public List<Map> AllMaps { get; private set; }
	public List<Map> ActiveMaps { get; private set; }

	public List<GameObject> AllFloors { get; private set; }
	public List<GameObject> ActiveFloors { get; private set; }

	public List<GameObject> AllWalls { get; private set; }
	public List<GameObject> ActiveWalls { get; private set; }

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

	public Map CreateMap(Point position)
	{
		Map map = new Map();

		int x = map.Width * position.X;
		int y = map.Height * position.Y;
		map.SetPosition(x, y);

		AddMap(position, map);

		return map;
	}

	public Map CreateMap(int x, int y)
	{
		return CreateMap(new Point(x, y));
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

	public void AddMap(Point position, Map map)
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

		AddChild(map);

		MapAddedEventArgs args = new MapAddedEventArgs();
		args.Map = map;
		OnMapAdded(args);
	}

	public void AddMap(int x, int y, Map map)
	{
		AddMap(new Point(x, y), map);
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
		Map map = GetMap(x, y);
		RemoveMap(map);
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
		Map map = GetMap(x, y);
		LoadMap(map);
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
		Map map = GetMap(x, y);
		UnloadMap(map);
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

