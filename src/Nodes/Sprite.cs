public class Sprite : Node2D
{

	#region Properties

	public ConsoleColor BackgroundColor { get; set; }
	public ConsoleColor ForegroundColor { get; set; }
	public char Symbol { get; set; }

	#endregion // Properties



	#region Constructors

	public Sprite(string name, int x, int y, ConsoleColor backgroundColor, ConsoleColor foregroundColor, char symbol) : base(name, x, y)
	{
		BackgroundColor = backgroundColor;
		ForegroundColor = foregroundColor;
		Symbol = symbol;
	}

	public Sprite(string name, ConsoleColor backgroundColor, ConsoleColor foregroundColor, char symbol) : this(name, 0, 0, backgroundColor, foregroundColor, symbol) { }

	public Sprite(int x, int y, ConsoleColor backgroundColor, ConsoleColor foregroundColor, char symbol) : this("Sprite", x, y, backgroundColor, foregroundColor, symbol) { }

	public Sprite(ConsoleColor backgroundColor, ConsoleColor foregroundColor, char symbol) : this("Sprite", 0, 0, backgroundColor, foregroundColor, symbol) { }

	public Sprite() : this("Sprite", 0, 0, ConsoleColor.Black, ConsoleColor.White, 'S') { }
	
	#endregion // Constructors

}

