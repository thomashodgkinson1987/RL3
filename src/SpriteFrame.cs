public class SpriteFrame
{

	#region Properties

	public ConsoleColor BackgroundColor { get; set; }
	public ConsoleColor ForegroundColor { get; set; }
	public char Symbol { get; set; }

	#endregion // Properties



	#region Constructors

	public SpriteFrame(ConsoleColor backgroundColor, ConsoleColor foregroundColor, char symbol)
	{
		BackgroundColor = backgroundColor;
		ForegroundColor = foregroundColor;
		Symbol = symbol;
	}

	#endregion // Constructors

}

