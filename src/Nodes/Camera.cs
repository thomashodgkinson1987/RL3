public class Camera : Node2D
{

	#region Properties

	public int Width { get; set; }
	public int Height { get; set; }

	public Bounds Bounds { get; set; }

	#endregion // Properties



	#region Constructors

	public Camera(string name, int x, int y, int width, int height) : base(name, x, y)
	{
		Width = width;
		Height = height;

		Bounds = new Bounds(x, y, x + width - 1, y + height - 1);
	}

	public Camera(string name, int width, int height) : this(name, 0, 0, width, height) { }

	public Camera(int x, int y, int width, int height) : this("Camera", x, y, width, height) { }

	public Camera(int width, int height) : this("Camera", 0, 0, width, height) { }

	public Camera() : this(64, 32) { }

	#endregion // Constructors



	#region Public methods

	public void CenterOnPosition(Point position)
	{
		int x = position.X - (int)(Width / 2f);
		int y = position.Y - (int)(Height / 2f);
		SetPosition(x, y);
	}
	public void CenterOnPosition(int x, int y)
	{
		CenterOnPosition(new Point(x, y));
	}

	public void CenterOnPositionX(int x)
	{
		SetPositionX(x - (int)(Width / 2f));
	}

	public void CenterOnPositionY(int y)
	{
		SetPositionY(y - (int)(Height / 2f));
	}

	#endregion // Public methods

}

