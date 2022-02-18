
public class Pixel
{

	#region Properties

	public ConsoleColor BackgroundColor { get; set; }
	public ConsoleColor ForegroundColor { get; set; }
	public char Symbol { get; set; }
	public bool IsTransparent { get; set; }

	#endregion // Properties



	#region Constructors

	public Pixel(ConsoleColor backgroundColor, ConsoleColor foregroundColor, char symbol, bool isTransparent)
	{
		BackgroundColor = backgroundColor;
		ForegroundColor = foregroundColor;
		Symbol = symbol;
		IsTransparent = isTransparent;
	}

	#endregion // Constructors



	#region Public methods

	public void Set(ConsoleColor backgroundColor, ConsoleColor foregroundColor, char symbol, bool isTransparent)
	{
		BackgroundColor = backgroundColor;
		ForegroundColor = foregroundColor;
		Symbol = symbol;
		IsTransparent = isTransparent;
	}

	#endregion // Public methods

}

