public class Actor : GameObject
{

	#region Fields

	protected readonly Random m_rng;

	#endregion // Fields



	#region Constructors

	public Actor (string name, int x, int y, char symbol, Random rng) : base(name, x, y, symbol)
	{
		m_rng = rng;
	}

	public Actor (string name, char symbol, Random rng) : this(name, 0, 0, symbol, rng) { }

	public Actor (int x, int y, char symbol, Random rng) : this("Actor", x, y, symbol, rng) { }

	public Actor (char symbol, Random rng) : this("Actor", 0, 0, symbol, rng) { }

	#endregion // Constructors

}

