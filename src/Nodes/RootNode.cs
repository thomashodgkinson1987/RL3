public sealed class RootNode : Node2D
{

	#region Properties

	static RootNode? _instance = null;
	public static RootNode Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new RootNode();
			}
			return _instance;
		}
	}

	public bool IsQuit { get; set; }

	#endregion // Properties



	#region Constructors

	RootNode () { }

	#endregion // Constructors



	#region Public methods

	public void Quit ()
	{
		IsQuit = true;
	}

	#endregion // Public methods

}

