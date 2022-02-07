public class Floor : GameObject
{

	#region Constructors

	public Floor(string name, int x, int y) : base(name, x, y, '.') { }

	public Floor(string name) : this(name, 0, 0) { }

	public Floor(int x, int y) : this("Floor", x, y) { }

	public Floor() : this("Floor", 0, 0) { }

	#endregion // Constructors

}

