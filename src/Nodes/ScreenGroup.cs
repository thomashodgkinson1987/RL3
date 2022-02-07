public class ScreenGroup : Node2D
{

	#region Properties

	public List<Screen> Screens { get; }

	#endregion // Properties



	#region Constructors

	public ScreenGroup(string name, int x, int y) : base(name, x, y)
	{
		Screens = new List<Screen>();
	}

	public ScreenGroup(int x, int y) : this("ScreenGroup", x, y) { }

	public ScreenGroup(string name) : this(name, 0, 0) { }

	public ScreenGroup() : this("ScreenGroup", 0, 0) { }

	#endregion // Constructors



	#region Node2D methods

	public override void Init()
	{
		base.Init();

		for (int i = 0; i < Children.Count; i++)
		{
			Node2D node = GetNode(i);
			if (node is Screen screen)
			{
				AddScreen(screen);
			}
		}

		NodeAdded += delegate (object? sender, NodeAddedEventArgs e)
		{
			if (e.Node is Screen screen)
			{
				AddScreen(screen);
			}
		};

		NodeRemoved += delegate (object? sender, NodeRemovedEventArgs e)
		{
			if (e.Node is Screen screen)
			{
				RemoveScreen(screen);
			}
		};
	}

	#endregion // Node2D methods



	#region Public methods

	public void ClearAllScreens()
	{
		Screens.ForEach(_ => _.Clear());
	}

	public Screen GetScreen(int index)
	{
		return Screens[index];
	}

	public void AddScreen(Screen screen)
	{
		Screens.Add(screen);
	}

	public void RemoveScreen(Screen screen)
	{
		Screens.Remove(screen);
	}

	#endregion // Public methods

}

