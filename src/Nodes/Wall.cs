public class Wall : GameObject
{

	#region Constructors

	public Wall(string name, int x, int y) : base(name, x, y, '#') { }

	public Wall(string name) : this(name, 0, 0) { }

	public Wall(int x, int y) : this("Wall", x, y) { }

	public Wall() : this("Wall", 0, 0) { }

	#endregion // Constructors

}

