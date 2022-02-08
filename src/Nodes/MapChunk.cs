public class MapChunk : Node2D
{

	#region Nodes

	private readonly Node2D node_floors = new Node2D();
	private readonly Node2D node_walls = new Node2D();
	private readonly Node2D node_actors = new Node2D();

	#endregion // Nodes



	#region Properties

	public int Width { get; private set; }
	public int Height { get; private set; }

	public List<Floor> Floors { get; }
	public List<Wall> Walls { get; }
	public List<Actor> Actors { get; }

	public bool IsDirty { get; set; }

	#endregion // Properties



	#region Events

	public event EventHandler<FloorAddedEventArgs>? FloorAdded;
	public event EventHandler<FloorRemovedEventArgs>? FloorRemoved;

	public event EventHandler<WallAddedEventArgs>? WallAdded;
	public event EventHandler<WallRemovedEventArgs>? WallRemoved;

	public event EventHandler<ActorAddedEventArgs>? ActorAdded;
	public event EventHandler<ActorRemovedEventArgs>? ActorRemoved;

	public event EventHandler<GameObjectAddedEventArgs>? GameObjectAdded;
	public event EventHandler<GameObjectRemovedEventArgs>? GameObjectRemoved;

	#endregion // Events



	#region Constructors

	public MapChunk(string name, int x, int y) : base(name, x, y)
	{
		node_floors = new Node2D("Floors");
		node_walls = new Node2D("Walls");
		node_actors = new Node2D("Actors");

		AddChild(node_floors);
		AddChild(node_walls);
		AddChild(node_actors);

		Width = 16;
		Height = 16;

		Floors = new List<Floor>();
		Walls = new List<Wall>();
		Actors = new List<Actor>();

		IsDirty = true;
	}

	public MapChunk(string name) : this(name, 0, 0) { }

	public MapChunk(int x, int y) : this("MapChunk", x, y) { }

	public MapChunk() : this("MapChunk", 0, 0) { }

	#endregion // Constructors



	#region Public methods

	public void AddFloor(Floor floor, Point position)
	{
		floor.SetPosition(position);
		Floors.Add(floor);
		node_floors.AddChild(floor);

		IsDirty = true;

		GameObjectAddedEventArgs args1 = new GameObjectAddedEventArgs();
		args1.GameObject = floor;
		OnGameObjectAdded(args1);

		FloorAddedEventArgs args2 = new FloorAddedEventArgs();
		args2.Floor = floor;
		OnFloorAdded(args2);
	}

	public void AddFloor(Floor floor, int x, int y)
	{
		AddFloor(floor, new Point(x, y));
	}

	public void RemoveFloor(Floor floor)
	{
		Floors.Remove(floor);
		node_floors.RemoveChild(floor);

		IsDirty = true;

		GameObjectRemovedEventArgs args1 = new GameObjectRemovedEventArgs();
		args1.GameObject = floor;
		OnGameObjectRemoved(args1);

		FloorRemovedEventArgs args2 = new FloorRemovedEventArgs();
		args2.Floor = floor;
		OnFloorRemoved(args2);
	}

	public void AddWall(Wall wall, Point position)
	{
		wall.SetPosition(position);
		Walls.Add(wall);
		node_walls.AddChild(wall);

		IsDirty = true;

		GameObjectAddedEventArgs args1 = new GameObjectAddedEventArgs();
		args1.GameObject = wall;
		OnGameObjectAdded(args1);

		WallAddedEventArgs args2 = new WallAddedEventArgs();
		args2.Wall = wall;
		OnWallAdded(args2);
	}

	public void AddWall(Wall wall, int x, int y)
	{
		AddWall(wall, new Point(x, y));
	}

	public void RemoveWall(Wall wall)
	{
		Walls.Remove(wall);
		node_walls.RemoveChild(wall);

		IsDirty = true;

		GameObjectRemovedEventArgs args1 = new GameObjectRemovedEventArgs();
		args1.GameObject = wall;
		OnGameObjectRemoved(args1);

		WallRemovedEventArgs args2 = new WallRemovedEventArgs();
		args2.Wall = wall;
		OnWallRemoved(args2);
	}

	public void AddActor(Actor actor, Point position)
	{
		actor.SetPosition(position);
		Actors.Add(actor);
		node_actors.AddChild(actor);

		IsDirty = true;

		GameObjectAddedEventArgs args1 = new GameObjectAddedEventArgs();
		args1.GameObject = actor;
		OnGameObjectAdded(args1);

		ActorAddedEventArgs args2 = new ActorAddedEventArgs();
		args2.Actor = actor;
		OnActorAdded(args2);
	}

	public void AddActor(Actor actor, int x, int y)
	{
		AddActor(actor, new Point(x, y));
	}

	public void RemoveActor(Actor actor)
	{
		Actors.Remove(actor);
		node_actors.RemoveChild(actor);

		IsDirty = true;

		GameObjectRemovedEventArgs args1 = new GameObjectRemovedEventArgs();
		args1.GameObject = actor;
		OnGameObjectRemoved(args1);

		ActorRemovedEventArgs args2 = new ActorRemovedEventArgs();
		args2.Actor = actor;
		OnActorRemoved(args2);
	}

	#endregion // Public methods



	#region Protected methods

	protected virtual void OnFloorAdded(FloorAddedEventArgs e)
	{
		EventHandler<FloorAddedEventArgs>? handler = FloorAdded;
		handler?.Invoke(this, e);
	}

	protected virtual void OnFloorRemoved(FloorRemovedEventArgs e)
	{
		EventHandler<FloorRemovedEventArgs>? handler = FloorRemoved;
		handler?.Invoke(this, e);
	}

	protected virtual void OnWallAdded(WallAddedEventArgs e)
	{
		EventHandler<WallAddedEventArgs>? handler = WallAdded;
		handler?.Invoke(this, e);
	}

	protected virtual void OnWallRemoved(WallRemovedEventArgs e)
	{
		EventHandler<WallRemovedEventArgs>? handler = WallRemoved;
		handler?.Invoke(this, e);
	}

	protected virtual void OnActorAdded(ActorAddedEventArgs e)
	{
		EventHandler<ActorAddedEventArgs>? handler = ActorAdded;
		handler?.Invoke(this, e);
	}

	protected virtual void OnActorRemoved(ActorRemovedEventArgs e)
	{
		EventHandler<ActorRemovedEventArgs>? handler = ActorRemoved;
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

