public struct Bounds
{

	#region Properties

	public int Left { get; set; }
	public int Right { get; set; }
	public int Up { get; set; }
	public int Down { get; set; }

	#endregion // Properties



	#region Constructors

	public Bounds(int left, int right, int up, int down)
	{
		Left = left;
		Right = right;
		Up = up;
		Down = down;
	}

	public Bounds() : this (0, 15, 0, 15) { }

	#endregion // Constructors

}

