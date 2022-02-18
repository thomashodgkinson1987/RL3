using System.Text;

public class Screen : Node2D
{

	#region Enums

	public enum EDirection { Left, Right, Up, Down }

	#endregion // Enums



	#region Properties

	public int Width { get; }
	public int Height { get; }
	public Pixel[,] Pixels { get; }
	public Pixel ClearPixel { get; set; }
	public bool IsDirty { get; set; }

	#endregion // Properties



	#region Constructors

	public Screen(string name, int x, int y, int width, int height) : base(name, x, y)
	{
		Width = width;
		Height = height;
		Pixels = new Pixel[height,width];
		ClearPixel = new Pixel(ConsoleColor.Black, ConsoleColor.White, ' ', false);
		IsDirty = true;

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				Pixels[i,j] = new Pixel(ConsoleColor.Black, ConsoleColor.White, ' ', false);
			}
		}
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

	public Pixel GetPixel(int x, int y) => Pixels[y,x];
	public Pixel GetPixel(Point position) => GetPixel(position.X, position.Y);

	public void SetPixel(int x, int y, ConsoleColor backgroundColor, ConsoleColor foregroundColor, char symbol, bool isTransparent)
	{
		Pixels[y,x].Set(backgroundColor, foregroundColor, symbol, isTransparent);
	}
	public void SetPixel(Point position, ConsoleColor backgroundColor, ConsoleColor foregroundColor, char symbol, bool isTransparent)
	{
		SetPixel(position.X, position.Y, backgroundColor, foregroundColor, symbol, isTransparent);
	}
	public void SetPixelFromPixel(int x, int y, Pixel pixel)
	{
		Pixels[y,x].BackgroundColor = pixel.BackgroundColor;
		Pixels[y,x].ForegroundColor = pixel.ForegroundColor;
		Pixels[y,x].Symbol = pixel.Symbol;
		Pixels[y,x].IsTransparent = pixel.IsTransparent;
	}
	public void SetPixelFromPixel(Point position, Pixel pixel)
	{
		SetPixelFromPixel(position.X, position.Y, pixel);
	}

	public void Clear() => FillScreenWithPixel(ClearPixel);

	public bool IsPositionOnScreen(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;
	public bool IsPositionOnScreen(Point position) => IsPositionOnScreen(position.X, position.Y);

	public void FillScreen(ConsoleColor backgroundColor, ConsoleColor foregroundColor, char symbol, bool isTransparent)
	{
		for(int i = 0; i < Pixels.GetLength(0); i++)
		{
			for(int j = 0; j < Pixels.GetLength(1); j++)
			{
				SetPixel(j, i, backgroundColor, foregroundColor, symbol, isTransparent);
			}
		}
	}

	public void FillScreenWithPixel(Pixel pixel)
	{
		for(int i = 0; i < Pixels.GetLength(0); i++)
		{
			for(int j = 0; j < Pixels.GetLength(1); j++)
			{
				FillScreen(pixel.BackgroundColor, pixel.ForegroundColor, pixel.Symbol, pixel.IsTransparent);
			}
		}
	}

	public void FillScreenFromScreen(Screen screen)
	{
		int rows = Pixels.GetLength(0) < screen.Pixels.GetLength(0) ? Pixels.GetLength(0) : screen.Pixels.GetLength(0);
		int columns = Pixels.GetLength(1) < screen.Pixels.GetLength(1) ? Pixels.GetLength(1) : screen.Pixels.GetLength(1);

		for(int i = 0; i < rows; i++)
		{
			for(int j = 0; j < columns; j++)
			{
				SetPixelFromPixel(j, i, screen.GetPixel(j, i));
			}
		}
	}

	public void FillRow(int row, ConsoleColor backgroundColor, ConsoleColor foregroundColor, char symbol, bool isTransparent)
	{
		for(int i = 0; i < Pixels.GetLength(1); i++)
		{
			SetPixel(i, row, backgroundColor, foregroundColor, symbol, isTransparent);
		}
	}

	public void FillRowWithPixel(int row, Pixel pixel)
	{
		FillRow(row, pixel.BackgroundColor, pixel.ForegroundColor, pixel.Symbol, pixel.IsTransparent);
	}

	public void FillColumn(int column, ConsoleColor backgroundColor, ConsoleColor foregroundColor, char symbol, bool isTransparent)
	{
		for(int i = 0; i < Pixels.GetLength(0); i++)
		{
			SetPixel(column, i, backgroundColor, foregroundColor, symbol, isTransparent);
		}
	}

	public void FillColumnWithPixel(int column, Pixel pixel)
	{
		FillColumn(column, pixel.BackgroundColor, pixel.ForegroundColor, pixel.Symbol, pixel.IsTransparent);
	}

	public void DrawLine(int x, int y, EDirection direction, int length, ConsoleColor backgroundColor, ConsoleColor foregroundColor, char symbol, bool isTransparent)
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
				SetPixel(j, i, backgroundColor, foregroundColor, symbol, isTransparent);
			}
		}
	}

	public void DrawText(int x, int y, EDirection direction, ConsoleColor backgroundColor, ConsoleColor foregroundColor, string text, bool isTransparent)
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
				SetPixel(j, i, backgroundColor, foregroundColor, symbol, isTransparent);
			}
		}
	}

	public void DrawRectangle(int x, int y, int width, int height, ConsoleColor backgroundColor, ConsoleColor foregroundColor, char symbol, bool isTransparent, bool fill = true)
	{
		for(int i = y; i < y + height; i++)
		{
			if (i < 0 || i > Height - 1) continue;
			for(int j = x; j < x + width; j++)
			{
				if (j < 0 || j > Width - 1) continue;
				if (fill)
				{
					SetPixel(j, i, backgroundColor, foregroundColor, symbol, isTransparent);
				}
				else
				{
					if((i == y || i == y + (height - 1)) || (j == x || j == x + (width - 1)))
					{
						SetPixel(j, i, backgroundColor, foregroundColor, symbol, isTransparent);
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
				sb.Append(Pixels[i,j].Symbol);
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

