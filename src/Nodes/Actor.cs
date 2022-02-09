public class Actor : GameObject
{

	#region Constructors

	public Actor (string name, int x, int y, char symbol = 'A') : base(name, x, y, symbol) { }

	public Actor (string name, char symbol = 'A') : this(name, 0, 0, symbol) { }

	public Actor (int x, int y, char symbol = 'A') : this("Actor", x, y, symbol) { }

	public Actor (char symbol = 'A') : this("Actor", 0, 0, symbol) { }

	#endregion // Constructors

}

