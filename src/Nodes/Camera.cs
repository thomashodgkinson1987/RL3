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

	public void SetBounds(int left, int right, int up, int down)
	{
		Bounds = new Bounds(left, right, up, down);
	}

	public void SetBoundsLeft(int left)
	{
		SetBounds(left, Bounds.Right, Bounds.Up, Bounds.Down);
	}

	public void SetBoundsRight(int right)
	{
		SetBounds(Bounds.Left, right, Bounds.Up, Bounds.Down);
	}

	public void SetBoundsUp(int up)
	{
		SetBounds(Bounds.Left, Bounds.Right, up, Bounds.Down);
	}

	public void SetBoundsDown(int down)
	{
		SetBounds(Bounds.Left, Bounds.Right, Bounds.Up, down);
	}

	public void ClampToBounds()
	{
		int w = (int)MathF.Abs(Bounds.Right - Bounds.Left);
		int h = (int)MathF.Abs(Bounds.Down - Bounds.Up);

		if (Width < w)
		{
			if (Position.X < Bounds.Left)
			{
				SetPositionX(Bounds.Left);
			}
			else if (Position.X + Width > Bounds.Right)
			{
				SetPositionX(Bounds.Right - Width);
			}
		}
		else
		{
			int x = Bounds.Left + ((int)(w / 2f));
			CenterOnPositionX(x);
		}

		if (Height < h)
		{
			if (Position.Y < Bounds.Up)
			{
				SetPositionY(Bounds.Up);
			}
			else if (Position.Y + Height > Bounds.Down)
			{
				SetPositionY(Bounds.Down - Height);
			}
		}
		else
		{
			int y = Bounds.Up + (int)(h / 2f);
			CenterOnPositionY(y);
		}
	}

	public void ExpandBoundsTo(int left, int right, int up, int down)
	{
		Bounds newBounds = new Bounds();

		newBounds.Left = Bounds.Left < left ? Bounds.Left : left;
		newBounds.Right = Bounds.Right > right ? Bounds.Right : right;
		newBounds.Up = Bounds.Up < up ? Bounds.Up : up;
		newBounds.Down = Bounds.Down > down ? Bounds.Down : down;

		Bounds = newBounds;
	}

	#endregion // Public methods

}

