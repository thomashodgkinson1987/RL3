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

	#endregion // Operator overloads



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

}

