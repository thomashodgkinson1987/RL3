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

	#endregion // Properties



	#region Constructors

	RootNode () { }

	#endregion // Constructors

}

