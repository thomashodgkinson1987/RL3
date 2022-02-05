public class Map : Node2D
{

	#region Nodes

	private readonly Node2D node_floors = new Node2D();
	private readonly Node2D node_walls = new Node2D();

	#endregion // Nodes



	#region Properties

	public int Width { get; private set; }
	public int Height { get; private set; }

	public List<GameObject> Floors { get; }
	public List<GameObject> Walls { get; }

	#endregion // Properties



	#region Events

	public event EventHandler<GameObjectAddedEventArgs>? FloorAdded;
	public event EventHandler<GameObjectRemovedEventArgs>? FloorRemoved;

	public event EventHandler<GameObjectAddedEventArgs>? WallAdded;
	public event EventHandler<GameObjectRemovedEventArgs>? WallRemoved;

	public event EventHandler<GameObjectAddedEventArgs>? GameObjectAdded;
	public event EventHandler<GameObjectRemovedEventArgs>? GameObjectRemoved;

	#endregion // Events



	#region Constructors

	public Map(string name, int x, int y) : base(name, x, y)
	{
		node_floors = new Node2D("Floors");
		node_walls = new Node2D("Walls");

		AddChild(node_floors);
		AddChild(node_walls);

		Width = 16;
		Height = 16;

		Floors = new List<GameObject>();
		Walls = new List<GameObject>();
	}

	public Map(string name) : this(name, 0, 0) { }

	public Map(int x, int y) : this("Map", x, y) { }

	public Map() : this("Map", 0, 0) { }

	#endregion // Constructors



	#region Public methods

	public void AddFloor(int x, int y)
	{
		GameObject floor = new GameObject("Floor", x, y, '.');
		node_floors.AddChild(floor);
		Floors.Add(floor);

		GameObjectAddedEventArgs args = new GameObjectAddedEventArgs();
		args.GameObject = floor;
		OnGameObjectAdded(args);
		OnFloorAdded(args);
	}

	public void RemoveFloor(int x, int y)
	{
		Node2D? node = node_floors.Children.Find(_ => _.Position.X == x && _.Position.Y == y);

		if (node != null && node is GameObject gameObject)
		{
			node_floors.RemoveChild(gameObject);
			Floors.Remove(gameObject);

			GameObjectRemovedEventArgs args = new GameObjectRemovedEventArgs();
			args.GameObject = gameObject;
			OnGameObjectRemoved(args);
			OnFloorRemoved(args);
		}
	}

	public void AddWall(int x, int y)
	{
		GameObject wall = new GameObject("Wall", x, y, '#');
		node_walls.AddChild(wall);
		Walls.Add(wall);

		GameObjectAddedEventArgs args = new GameObjectAddedEventArgs();
		args.GameObject = wall;
		OnGameObjectAdded(args);
		OnWallAdded(args);
	}

	public void RemoveWall(int x, int y)
	{
		Node2D? node = node_walls.Children.Find(_ => _.Position.X == x && _.Position.Y == y);

		if (node != null && node is GameObject gameObject)
		{
			node_walls.RemoveChild(gameObject);
			Walls.Remove(gameObject);

			GameObjectRemovedEventArgs args = new GameObjectRemovedEventArgs();
			args.GameObject = gameObject;
			OnGameObjectRemoved(args);
			OnWallRemoved(args);
		}
	}

	#endregion // Public methods



	#region Protected methods

	protected virtual void OnFloorAdded(GameObjectAddedEventArgs e)
	{
		EventHandler<GameObjectAddedEventArgs>? handler = FloorAdded;
		handler?.Invoke(this, e);
	}

	protected virtual void OnFloorRemoved(GameObjectRemovedEventArgs e)
	{
		EventHandler<GameObjectRemovedEventArgs>? handler = FloorRemoved;
		handler?.Invoke(this, e);
	}

	protected virtual void OnWallAdded(GameObjectAddedEventArgs e)
	{
		EventHandler<GameObjectAddedEventArgs>? handler = WallAdded;
		handler?.Invoke(this, e);
	}

	protected virtual void OnWallRemoved(GameObjectRemovedEventArgs e)
	{
		EventHandler<GameObjectRemovedEventArgs>? handler = WallRemoved;
		handler?.Invoke(this, e);
	}

	protected virtual void OnGameObjectAdded(GameObjectAddedEventArgs e)
	{
		EventHandler<GameObjectAddedEventArgs>? handler = GameObjectAdded;
		handler?.Invoke(this, e);
	}

	protected virtual void OnGameObjectRemoved(GameObjectRemovedEventArgs e)
	{
		EventHandler<GameObjectRemovedEventArgs>? handler = GameObjectRemoved;
		handler?.Invoke(this, e);
	}

	#endregion // Protected methods

}

