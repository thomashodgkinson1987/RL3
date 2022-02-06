public class MapGroup : Node2D
{

	#region Properties

	public Dictionary<(int x, int y), Map> Maps { get; private set; }

	public List<Map> AllMaps { get; private set; }
	public List<Map> ActiveMaps { get; private set; }
	public List<Map> InactiveMaps { get; private set; }

	public List<GameObject> AllFloors { get; private set; }
	public List<GameObject> ActiveFloors { get; private set; }
	public List<GameObject> InactiveFloors { get; private set; }

	public List<GameObject> AllWalls { get; private set; }
	public List<GameObject> ActiveWalls { get; private set; }
	public List<GameObject> InactiveWalls { get; private set; }

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

	public event EventHandler<IntChangedEventArgs>? MapLoadDistanceChanged;

	#endregion // Events



	#region Constructors

	public MapGroup(string name, int x, int y) : base(name, x, y)
	{
		Maps = new Dictionary<(int x, int y), Map>();

		AllMaps = new List<Map>();
		ActiveMaps = new List<Map>();
		InactiveMaps = new List<Map>();

		AllFloors = new List<GameObject>();
		ActiveFloors = new List<GameObject>();
		InactiveFloors = new List<GameObject>();

		AllWalls = new List<GameObject>();
		ActiveWalls = new List<GameObject>();
		InactiveWalls = new List<GameObject>();

		_mapLoadDistance = 2;
	}

	public MapGroup(string name) : this(name, 0, 0) { }

	public MapGroup(int x, int y) : this("MapGroup", x, y) { }

	public MapGroup() : this("MapGroup", 0, 0) { }

	#endregion // Constructors



	#region Public methods

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
		return Maps[(position.X, position.Y)];
	}

	public Map GetMap(int x, int y)
	{
		return GetMap(new Point(x, y));
	}

	public Point GetMapPosition(Map map)
	{
		(int x, int y) position = (0, 0);

		foreach(KeyValuePair<(int x, int y), Map> kvp in Maps)
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
		Maps.Add((position.X, position.Y), map);

		AllMaps.Add(map);
		InactiveMaps.Add(map);

		foreach(GameObject floor in map.Floors)
		{
			AllFloors.Add(floor);
			InactiveFloors.Add(floor);
		}
		foreach(GameObject wall in map.Walls)
		{
			AllWalls.Add(wall);
			InactiveWalls.Add(wall);
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
		foreach(GameObject floor in map.Floors)
		{
			AllFloors.Remove(floor);
			InactiveFloors.Remove(floor);
			ActiveFloors.Remove(floor);
		}

		foreach(GameObject wall in map.Walls)
		{
			AllWalls.Remove(wall);
			InactiveWalls.Remove(wall);
			ActiveWalls.Remove(wall);
		}

		AllMaps.Remove(map);
		InactiveMaps.Remove(map);
		ActiveMaps.Remove(map);

		Point position = GetMapPosition(map);
		Maps.Remove((position.X, position.Y));

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
		InactiveMaps.Remove(map);

		foreach(GameObject floor in map.Floors)
		{
			ActiveFloors.Add(floor);
			InactiveFloors.Remove(floor);
		}

		foreach(GameObject wall in map.Walls)
		{
			ActiveWalls.Add(wall);
			InactiveWalls.Remove(wall);
		}
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
		InactiveMaps.Add(map);

		foreach(GameObject floor in map.Floors)
		{
			ActiveFloors.Remove(floor);
			InactiveFloors.Add(floor);
		}

		foreach(GameObject wall in map.Walls)
		{
			ActiveWalls.Remove(wall);
			InactiveWalls.Add(wall);
		}

		InactiveFloors.Clear();
		InactiveWalls.Clear();
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

	protected virtual void OnMapLoadDistanceChanged(IntChangedEventArgs e)
	{
		EventHandler<IntChangedEventArgs>? handler = MapLoadDistanceChanged;
		handler?.Invoke(this, e);
	}

	#endregion // Protected methods

}

