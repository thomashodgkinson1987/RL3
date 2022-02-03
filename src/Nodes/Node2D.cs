public class Node2D
{

	#region Properties

	private static Node2D? _rootNode = null;
	public Node2D RootNode
	{
		get
		{
			if (_rootNode == null)
			{
				_rootNode = this;
			}

			return _rootNode;
		}
	}

	private string _name = string.Empty;
	public string Name
	{
		get => _name;
		set
		{
			string nameBeforeChange = Name;
			_name = value;

			StringChangedEventArgs args = new StringChangedEventArgs();
			args.ValueBeforeChange = nameBeforeChange;
			args.ValueAfterChange = Name;
			OnNameChanged(args);
		}
	}

	private Point _position = new Point();
	public Point Position
	{
		get => _position;
		set
		{
			Point positionBeforeChange = Position;
			_position = value;

			PointChangedEventArgs args = new PointChangedEventArgs();
			args.ValueBeforeChange = positionBeforeChange;
			args.ValueAfterChange = Position;
			OnPositionChanged(args);
		}
	}

	public Point GlobalPosition
	{
		get
		{
			//Point position = Position;
			if (Parent != null)
			{
				return Position + Parent.GlobalPosition;
			//	Point parentGlobalPosition = Parent.GlobalPosition;
			//	position.X += parentGlobalPosition.X;
			//	position.Y += parentGlobalPosition.Y;
			}
			else
			{
				return Position;
			}
		}
	}

	private bool _isVisible = true;
	public bool IsVisible
	{
		get => _isVisible;
		set
		{
			bool isVisibleBeforeChange = IsVisible;
			_isVisible = value;

			BoolChangedEventArgs args = new BoolChangedEventArgs();
			args.ValueBeforeChange = isVisibleBeforeChange;
			args.ValueAfterChange = IsVisible;
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
		SetPosition(new Point(dx, dy));
	}

	public void TranslateX(int dx)
	{
		Translate(dx, 0);
	}

	public void TranslateY(int dy)
	{
		Translate(0, dy);
	}

	public Node2D? GetNode(int index)
	{
		return Children[index];
	}

	public Node2D? GetNode(string name)
	{
		return Children.Find(_ => _.Name == name);
	}

	public T? GetNode<T>(int index) where T : Node2D
	{
		return (T)Children[index];
	}

	public T? GetNode<T>(string name) where T : Node2D
	{
		return (T?)Children.Find(_ => _.Name == name);
	}

	public void AddChild(Node2D node)
	{
		node.Parent = this;
		Children.Add(node);

		NodeAddedEventArgs args = new NodeAddedEventArgs();
		args.NodeAdded = node;
		OnNodeAdded(args);
	}

	public void RemoveChild(int index)
	{
		Node2D node = Children[index];
		RemoveChild(node);
	}

	public void RemoveChild(Node2D node)
	{
		node.Parent = null;
		Children.Remove(node);

		NodeRemovedEventArgs args = new NodeRemovedEventArgs();
		args.NodeRemoved = node;
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

