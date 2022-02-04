public class Camera : Node2D
{

	#region Properties

	public int Width { get; set; }
	public int Height { get; set; }

	public Point Center
	{
		get
		{
			return new Point(Position.X + (int)(Width / 2f), Position.Y + (int)(Height / 2f));
		}
	}

	public Point GlobalCenter
	{
		get
		{
			Point globalPosition = GlobalPosition;
			return new Point(globalPosition.X + (int)(Width / 2f), globalPosition.Y + (int)(Height / 2f));
		}
	}

	#endregion // Properties



	#region Constructors

	public Camera(string name, int x, int y, int width, int height) : base(name, x, y)
	{
		Width = width;
		Height = height;
	}

	public Camera(string name, int width, int height) : this(name, 0, 0, width, height) { }

	public Camera(int x, int y, int width, int height) : this("Camera", x, y, width, height) { }

	public Camera(int width, int height) : this("Camera", 0, 0, width, height) { }

	public Camera() : this(64, 32) { }

	#endregion // Constructors

}

