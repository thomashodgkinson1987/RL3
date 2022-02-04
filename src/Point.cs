public class Point
{

	#region Operator overloads

	public static Point operator +(Point a, Point b)
	{
		return new Point(a.X + b.X, a.Y + b.Y);
	}

	public static Point operator -(Point a, Point b)
	{
		return new Point(a.X - b.X, a.Y - b.Y);
	}

	public static Point operator *(Point a, Point b)
	{
		return new Point(a.X * b.X, a.Y * b.Y);
	}

	public static Point operator /(Point a, Point b)
	{
		return new Point(a.X / b.X, a.Y / b.Y);
	}

	public static Point operator ++(Point a)
	{
		return new Point(a.X + 1, a.Y + 1);
	}

	public static Point operator --(Point a)
	{
		return new Point(a.X - 1, a.Y - 1);
	}

	public static bool operator ==(Point a, Point b)
	{
		return a.X == b.X && a.Y == b.Y;
	}

	public static bool operator !=(Point a, Point b)
	{
		return a.X != b.X || a.Y != b.Y;
	}

	#endregion // Operator overloads



	#region Default variations

	public static Point Zero { get; } = new Point();
	public static Point One { get; } = new Point(1, 1);
	public static Point Left { get; } = new Point(-1, 0);
	public static Point Right { get; } = new Point(1, 0);
	public static Point Up { get; } = new Point(0, -1);
	public static Point Down { get; } = new Point(0, 1);

	#endregion // Default variations



	#region Properties

	public int X { get; set; }
	public int Y { get; set; }

	#endregion // Properties



	#region Constructors

	public Point(int x, int y)
	{
		X = x;
		Y = y;
	}

	public Point() : this(0, 0) { }

	#endregion // Contructors



	#region Public methods

	public override bool Equals(object? obj)
	{
		if (obj != null && obj is Point point)
		{
			return point == this;
		}
		
		return false;
	}

	#endregion // Public methods

}

