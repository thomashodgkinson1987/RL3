public class Input
{

	#region Properties

	public static ConsoleKeyInfo LastConsoleKeyInfo { get; private set; }

	#endregion // Properties



	#region Constructors

	public Input () { }

	#endregion // Constructors



	#region Public methods

	public void SetLastConsoleKeyInfo (ConsoleKeyInfo consoleKeyInfo)
	{
		LastConsoleKeyInfo = consoleKeyInfo;
	}

	#endregion // Public methods

}

