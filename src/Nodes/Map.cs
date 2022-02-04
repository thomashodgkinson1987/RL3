public class Map : Node2D
{

	#region Nodes

	private Node2D node_floors = new Node2D();
	private Node2D node_walls = new Node2D();

	#endregion // Nodes



	#region Properties

	public int Width { get; private set; }
	public int Height { get; private set; }

	public int MinX { get; private set; }
	public int MinY { get; private set; }
	public int MaxX { get; private set; }
	public int MaxY { get; private set; }

	public List<GameObject> Floors { get; }
	public List<GameObject> Walls { get; }

	#endregion // Properties



	#region Events

	public event EventHandler<GameObjectAddedEventArgs>? FloorAdded;
	public event EventHandler<GameObjectRemovedEventArgs>? FloorRemoved;

	public event EventHandler<GameObjectAddedEventArgs>? WallAdded;
	public event EventHandler<GameObjectRemovedEventArgs>? WallRemoved;

	#endregion // Events



	#region Constructors

	public Map(string name, int x, int y, int width, int height) : base(name, x, y)
	{
		Width = width;
		Height = height;

		MinX = 0;
		MinY = 0;
		MaxX = width - 1;
		MaxY = height - 1;

		Floors = new List<GameObject>();
		Walls = new List<GameObject>();
	}

	public Map(string name, int width, int height) : this(name, 0, 0, width, height) { }

	public Map(int x, int y, int width, int height) : this("World", x, y, width, height) { }

	public Map(int width, int height) : this("World", 0, 0, width, height) { }

	public Map() : this(64, 64) { }

	#endregion // Constructors



	#region Node2D methods

	public override void Init()
	{
		base.Init();

		node_floors = GetNode("Floors");
		node_walls = GetNode("Walls");

		foreach(Node2D node in node_floors.Children)
		{
			if (node is GameObject gameObject)
			{
				Floors.Add(gameObject);
			}
		}

		foreach(Node2D node in node_walls.Children)
		{
			if (node is GameObject gameObject)
			{
				Walls.Add(gameObject);
			}
		}
	}

	#endregion // Node2D methods



	#region Public methods

	public void AddFloor(int x, int y)
	{
		GameObject floor = new GameObject("Floor", x, y, '.');
		node_floors.AddChild(floor);
		Floors.Add(floor);

		GameObjectAddedEventArgs args = new GameObjectAddedEventArgs();
		args.GameObject = floor;
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

	#endregion // Protected methods

}

