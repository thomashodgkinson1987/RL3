using System.Text;

public class Screen : Node2D
{

	#region Enums

	public enum EDirection { Left, Right, Up, Down }

	#endregion // Enums



	#region Properties

	public int Width { get; }
	public int Height { get; }

	public char[,] Pixels { get; }

	public char ClearSymbol { get; set; } = ' ';

	public bool IsSpaceTransparent { get; set; } = true;

	public bool IsDirty { get; set; } = true;

	#endregion // Properties



	#region Constructors

	public Screen(string name, int x, int y, int width, int height) : base(name, x, y)
	{
		Width = width;
		Height = height;
		Pixels = new char[height,width];
	}

	public Screen(string name, int width, int height) : this(name, 0, 0, width, height) { }

	public Screen(int x, int y, int width, int height) : this("Screen", x, y, width, height) { }

	public Screen(int width, int height) : this("Screen", 0, 0, width, height) { }

	public Screen() : this(92, 38) { }

	#endregion // Constructors



	#region Node2D methods

	public override void Init()
	{
		base.Init();

		Clear();
	}

	#endregion // Node2D methods



	#region Public methods

	public char GetSymbol(int x, int y)
	{
		return Pixels[y,x];
	}

	public void SetSymbol(int x, int y, char symbol)
	{
		Pixels[y,x] = symbol;
	}

	public char GetSymbol(Point position)
	{
		return GetSymbol(position.X, position.Y);
	}

	public void SetSymbol(Point position, char symbol)
	{
		SetSymbol(position.X, position.Y, symbol);
	}

	public void Clear()
	{
		FillScreen(ClearSymbol);
	}

	public bool IsPositionOnScreen(int x, int y)
	{
		return x >= 0 && x < Width && y >= 0 && y < Height;
	}

	public bool IsPositionOnScreen(Point position)
	{
		return IsPositionOnScreen(position.X, position.Y);
	}

	public void FillScreen(char symbol)
	{
		for(int i = 0; i < Pixels.GetLength(0); i++)
		{
			for(int j = 0; j < Pixels.GetLength(1); j++)
			{
				Pixels[i,j] = symbol;
			}
		}
	}

	public void FillScreen(Screen screen, bool skipSpaces = false)
	{
		int rows = Pixels.GetLength(0) < screen.Pixels.GetLength(0) ? Pixels.GetLength(0) : screen.Pixels.GetLength(0);
		int columns = Pixels.GetLength(1) < screen.Pixels.GetLength(1) ? Pixels.GetLength(1) : screen.Pixels.GetLength(1);

		for(int i = 0; i < rows; i++)
		{
			for(int j = 0; j < columns; j++)
			{
				if (!skipSpaces || Pixels[i,j] != ' ')
				{
					Pixels[i,j] = screen.Pixels[i,j];
				}
			}
		}
	}

	public void FillRow(int row, char symbol)
	{
		for(int i = 0; i < Pixels.GetLength(1); i++)
		{
			Pixels[row, i] = symbol;
		}
	}

	public void FillColumn(int column, char symbol)
	{
		for(int i = 0; i < Pixels.GetLength(0); i++)
		{
			Pixels[i, column] = symbol;
		}
	}

	public void DrawLine(int x, int y, EDirection direction, int length, char symbol)
	{
		int x2 = x;
		int y2 = y;

		switch(direction)
		{
			case EDirection.Left:
				x2 -= (length - 1);
				break;
			case EDirection.Right:
				x2 += (length - 1);
				break;
			case EDirection.Up:
				y2 -= (length - 1);
				break;
			case EDirection.Down:
				y2 += (length - 1);
				break;
		}

		int _x1 = x < x2 ? x : x > x2 ? x2 : x;
		int _y1 = y < y2 ? y : y > y2 ? y2 : y;
		int _x2 = x < x2 ? x2 : x > x2 ? x : x2;
		int _y2 = y < y2 ? y2 : y > y2 ? y : y2;

		for(int i = _y1; i <= _y2; i++)
		{
			if (i < 0 || i > Height - 1) continue;
			for(int j = _x1; j <= _x2; j++)
			{
				if (j < 0 || j > Width - 1) continue;
				Pixels[i,j] = symbol;
			}
		}
	}

	public void DrawText(int x, int y, EDirection direction, string text)
	{
		int x2 = x;
		int y2 = y;
		int length = text.Length;

		switch(direction)
		{
			case EDirection.Left:
				x2 -= (length - 1);
				break;
			case EDirection.Right:
				x2 += (length - 1);
				break;
			case EDirection.Up:
				y2 -= (length - 1);
				break;
			case EDirection.Down:
				y2 += (length - 1);
				break;
		}

		int _x1 = x < x2 ? x : x > x2 ? x2 : x;
		int _y1 = y < y2 ? y : y > y2 ? y2 : y;
		int _x2 = x < x2 ? x2 : x > x2 ? x : x2;
		int _y2 = y < y2 ? y2 : y > y2 ? y : y2;

		int c = -1;

		for(int i = _y1; i <= _y2; i++)
		{
			if (i < 0 || i > Height - 1) continue;
			for(int j = _x1; j <= _x2; j++)
			{
				c++;
				if (j < 0 || j > Width - 1) continue;
				char symbol = text[c];
				Pixels[i,j] = symbol;
			}
		}
	}

	public void DrawRectangle(int x, int y, int width, int height, char symbol, bool fill = true)
	{
		for(int i = y; i < y + height; i++)
		{
			if (i < 0 || i > Height - 1) continue;
			for(int j = x; j < x + width; j++)
			{
				if (j < 0 || j > Width - 1) continue;
				if (fill)
				{
					Pixels[i,j] = symbol;
				}
				else
				{
					if((i == y || i == y + (height - 1)) || (j == x || j == x + (width - 1)))
					{
						Pixels[i,j] = symbol;
					}
				}
			}
		}
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();

		for(int i = 0; i < Pixels.GetLength(0); i++)
		{
			for(int j = 0; j < Pixels.GetLength(1); j++)
			{
				sb.Append(Pixels[i,j]);
			}
			if (i < Pixels.GetLength(0) - 1)
			{
				sb.AppendLine();
			}
		}

		return sb.ToString();
	}

	#endregion Public methods

}

