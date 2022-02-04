public class Node2D
{

	#region Properties

	private static Node2D? _rootNode = null;
	public Node2D RootNode
	{
		get
		{
			if (_rootNode == null) _rootNode = this;
			return _rootNode;
		}
	}

	private string _name;
	public string Name
	{
		get => _name;
		set
		{
			StringChangedEventArgs args = new StringChangedEventArgs();
			args.StringBeforeChange = Name;
			args.StringAfterChange = value;

			_name = value;

			OnNameChanged(args);
		}
	}

	private Point _position;
	public Point Position
	{
		get => _position;
		set
		{
			PointChangedEventArgs args = new PointChangedEventArgs();
			args.PointBeforeChange = Position;
			args.PointAfterChange = value;

			_position = value;

			OnPositionChanged(args);
		}
	}

	public Point GlobalPosition
	{
		get
		{
			return Parent != null ? Position + Parent.GlobalPosition : Position;
		}
	}

	private bool _isVisible = true;
	public bool IsVisible
	{
		get => _isVisible;
		set
		{
			BoolChangedEventArgs args = new BoolChangedEventArgs();
			args.BoolBeforeChange = IsVisible;
			args.BoolAfterChange = value;

			_isVisible = value;

			OnIsVisibleChanged(args);
		}
	}

	public Node2D? Parent { get; set; }

	public List<Node2D> Children { get; init; }

	#endregion // Properties



	#region Events

	public event EventHandler<StringChangedEventArgs>? NameChanged;
	public event EventHandler<PointChangedEventArgs>? PositionChanged;
	public event EventHandler<BoolChangedEventArgs>? IsVisibleChanged;

	public event EventHandler<NodeAddedEventArgs>? NodeAdded;
	public event EventHandler<NodeRemovedEventArgs>? NodeRemoved;

	#endregion // Events



	#region Constructors

	public Node2D(string name, int x, int y)
	{
		_name = name;
		_position = new Point(x, y);

		Children = new List<Node2D>();
	}

	public Node2D(string name) : this(name, 0, 0) { }

	public Node2D(int x, int y) : this("Node2D", x, y) { }

	public Node2D() : this("Node2D", 0, 0) { }

	#endregion // Constructors



	#region Public methods

	public virtual void Init()
	{
		foreach (Node2D node in Children)
		{
			node.Init();
		}
	}

	public void SetPosition(Point position)
	{
		Position = position;
	}
	public void SetPosition(int x, int y)
	{
		SetPosition(new Point(x, y));
	}

	public void SetPositionX(int x)
	{
		SetPosition(x, Position.Y);
	}

	public void SetPositionY(int y)
	{
		SetPosition(Position.X, y);
	}

	public void Translate(Point translation)
	{
		SetPosition(Position + translation);
	}

	public void Translate(int dx, int dy)
	{
		Translate(new Point(dx, dy));
	}

	public void TranslateX(int dx)
	{
		Translate(dx, 0);
	}

	public void TranslateY(int dy)
	{
		Translate(0, dy);
	}

	public Node2D GetNode(int index)
	{
		return Children[index];
	}

	public Node2D GetNode(string name)
	{
		Node2D? node = Children.Find(_ => _.Name == name);

		if (node == null)
		{
			throw new Exception();
		}
		else
		{
			return node;
		}
	}

	public T GetNode<T>(int index) where T : Node2D
	{
		return (T)Children[index];
	}

	public T GetNode<T>(string name) where T : Node2D
	{
		T? node = (T?)Children.Find(_ => _.Name == name);

		if (node != null && node is T t)
		{
			return t;
		}
		else
		{
			throw new Exception();
		}
	}

	public void AddChild(Node2D node)
	{
		NodeAddedEventArgs args = new NodeAddedEventArgs();
		args.PreviousParent = node.Parent;
		args.Node = node;

		node.Parent = this;
		Children.Add(node);

		OnNodeAdded(args);
	}

	public void RemoveChild(int index)
	{
		RemoveChild(Children[index]);
	}

	public void RemoveChild(Node2D node)
	{
		NodeRemovedEventArgs args = new NodeRemovedEventArgs();
		args.Node = node;
		args.PreviousParent = this;

		node.Parent = null;
		Children.Remove(node);

		OnNodeRemoved(args);
	}

	#endregion // Public methods



	#region Private methods

	protected virtual void OnNameChanged(StringChangedEventArgs e)
	{
		EventHandler<StringChangedEventArgs>? handler = NameChanged;
		handler?.Invoke(this, e);
	}

	protected virtual void OnPositionChanged(PointChangedEventArgs e)
	{
		EventHandler<PointChangedEventArgs>? handler = PositionChanged;
		handler?.Invoke(this, e);
	}

	protected virtual void OnIsVisibleChanged(BoolChangedEventArgs e)
	{
		EventHandler<BoolChangedEventArgs>? handler = IsVisibleChanged;
		handler?.Invoke(this, e);
	}



	protected virtual void OnNodeAdded(NodeAddedEventArgs e)
	{
		EventHandler<NodeAddedEventArgs>? handler = NodeAdded;
		handler?.Invoke(this, e);
	}

	protected virtual void OnNodeRemoved(NodeRemovedEventArgs e)
	{
		EventHandler<NodeRemovedEventArgs>? handler = NodeRemoved;
		handler?.Invoke(this, e);
	}

	#endregion // Private methods

}

