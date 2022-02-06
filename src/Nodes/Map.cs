public class Map : Node2D
{

	#region Nodes

	private readonly Node2D node_floors = new Node2D();
	private readonly Node2D node_walls = new Node2D();
	private readonly Node2D node_actors = new Node2D();

	#endregion // Nodes



	#region Properties

	public int Width { get; private set; }
	public int Height { get; private set; }

	public List<GameObject> Floors { get; }
	public List<GameObject> Walls { get; }
	public List<NPC> Actors { get; }

	public bool IsDirty { get; set; }

	#endregion // Properties



	#region Events

	public event EventHandler<GameObjectAddedEventArgs>? FloorAdded;
	public event EventHandler<GameObjectRemovedEventArgs>? FloorRemoved;

	public event EventHandler<GameObjectAddedEventArgs>? WallAdded;
	public event EventHandler<GameObjectRemovedEventArgs>? WallRemoved;

	public event EventHandler<NPCAddedEventArgs>? ActorAdded;
	public event EventHandler<NPCRemovedEventArgs>? ActorRemoved;

	public event EventHandler<GameObjectAddedEventArgs>? GameObjectAdded;
	public event EventHandler<GameObjectRemovedEventArgs>? GameObjectRemoved;

	#endregion // Events



	#region Constructors

	public Map(string name, int x, int y) : base(name, x, y)
	{
		node_floors = new Node2D("Floors");
		node_walls = new Node2D("Walls");
		node_actors = new Node2D("Actors");

		AddChild(node_floors);
		AddChild(node_walls);
		AddChild(node_actors);

		Width = 16;
		Height = 16;

		Floors = new List<GameObject>();
		Walls = new List<GameObject>();
		Actors = new List<NPC>();

		IsDirty = true;
	}

	public Map(string name) : this(name, 0, 0) { }

	public Map(int x, int y) : this("Map", x, y) { }

	public Map() : this("Map", 0, 0) { }

	#endregion // Constructors



	#region Public methods

	public void AddFloor(GameObject floor, Point position)
	{
		floor.SetPosition(position);
		node_floors.AddChild(floor);
		Floors.Add(floor);

		GameObjectAddedEventArgs args = new GameObjectAddedEventArgs();
		args.GameObject = floor;
		OnGameObjectAdded(args);
		OnFloorAdded(args);
	}

	public void AddFloor(GameObject floor, int x, int y)
	{
		AddFloor(floor, new Point(x, y));
	}

	public void RemoveFloor(GameObject floor)
	{
		node_floors.RemoveChild(floor);
		Floors.Remove(floor);

		GameObjectRemovedEventArgs args = new GameObjectRemovedEventArgs();
		args.GameObject = floor;
		OnGameObjectRemoved(args);
		OnFloorRemoved(args);
	}

	public void AddWall(GameObject wall, Point position)
	{
		wall.SetPosition(position);
		node_walls.AddChild(wall);
		Walls.Add(wall);

		GameObjectAddedEventArgs args = new GameObjectAddedEventArgs();
		args.GameObject = wall;
		OnGameObjectAdded(args);
		OnWallAdded(args);
	}

	public void AddWall(GameObject wall, int x, int y)
	{
		AddWall(wall, new Point(x, y));
	}

	public void RemoveWall(GameObject wall)
	{
		node_walls.RemoveChild(wall);
		Walls.Remove(wall);

		GameObjectRemovedEventArgs args = new GameObjectRemovedEventArgs();
		args.GameObject = wall;
		OnGameObjectRemoved(args);
		OnWallRemoved(args);
	}

	public void AddActor(NPC actor, Point position)
	{
		actor.SetPosition(position);
		node_actors.AddChild(actor);
		Actors.Add(actor);

		GameObjectAddedEventArgs args = new GameObjectAddedEventArgs();
		args.GameObject = actor;
		OnGameObjectAdded(args);

		NPCAddedEventArgs args1 = new NPCAddedEventArgs();
		args1.NPC = actor;
		OnActorAdded(args1);
	}

	public void AddActor(NPC actor, int x, int y)
	{
		AddActor(actor, new Point(x, y));
	}

	public void RemoveActor(NPC actor)
	{
		node_actors.RemoveChild(actor);
		Actors.Remove(actor);

		GameObjectRemovedEventArgs args = new GameObjectRemovedEventArgs();
		args.GameObject = actor;
		OnGameObjectRemoved(args);

		NPCRemovedEventArgs args1 = new NPCRemovedEventArgs();
		args1.NPC = actor;
		OnActorRemoved(args1);
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

	protected virtual void OnActorAdded(NPCAddedEventArgs e)
	{
		EventHandler<NPCAddedEventArgs>? handler = ActorAdded;
		handler?.Invoke(this, e);
	}

	protected virtual void OnActorRemoved(NPCRemovedEventArgs e)
	{
		EventHandler<NPCRemovedEventArgs>? handler = ActorRemoved;
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

