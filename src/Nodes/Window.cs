
public class Window : Node2D
{

	#region Enums

	public enum EBorderStyle { None, Squared, Rounded, DashedSquared, DashedRounded, DashedPlus }

	#endregion // Enums



	#region Properties

	public string Title { get; set; }
	public int Width { get; }
	public int Height { get; }
	public EBorderStyle BorderStyle { get; set; }
	public Pixel[,] Border { get; }
	public Screen Screen { get; }
	public bool IsDirty
	{
		get => Screen.IsDirty;
		set => Screen.IsDirty = value;
	}

	#endregion // Properties



	#region Fields

	private char[] m_borderStyle_0001 = new char[8] { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
	private char[] m_borderStyle_0002 = new char[8] { '│', '│', '─', '─', '┌', '┐', '└', '┘' };
	private char[] m_borderStyle_0003 = new char[8] { '│', '│', '─', '─', '╭', '╮', '╰', '╯' };
	private char[] m_borderStyle_0004 = new char[8] { '|', '|', '-', '-', '┌', '┐', '└', '┘' };
	private char[] m_borderStyle_0005 = new char[8] { '|', '|', '-', '-', '╭', '╮', '╰', '╯' };
	private char[] m_borderStyle_0006 = new char[8] { '|', '|', '-', '-', '+', '+', '+', '+' };

	#endregion // Fields



	#region Constructors

	public Window (string name, int x, int y, string title, int width, int height, EBorderStyle borderStyle = EBorderStyle.DashedPlus) : base(name, x, y)
	{
		Title = title;
		Width = width;
		Height = height;
		BorderStyle = borderStyle;
		Border = new Pixel[height,width];
		Screen = new Screen("Screen", 1, 1, width - 2, height - 2);

		AddChild(Screen);
	}

	public Window (string name, string title, int width, int height, EBorderStyle borderStyle = EBorderStyle.DashedPlus) : this(name, 0, 0, title, width, height, borderStyle) { }

	public Window (int x, int y, string title, int width, int height, EBorderStyle borderStyle = EBorderStyle.DashedPlus) : this("Window", x, y, title, width, height, borderStyle) { }

	public Window (string title, int width, int height, EBorderStyle borderStyle = EBorderStyle.DashedPlus) : this("Window", 0, 0, title, width, height, borderStyle) { }

	public Window () : this("Window", 92, 38, EBorderStyle.DashedPlus) { }

	#endregion // Constructors



	#region Node2D methods

	public override void Init ()
	{
		Clear();
		SetBorderStyle(BorderStyle);
		IsDirty = true;
	}

	#endregion // Node2D



	#region Public methods

	public void SetBorderStyle(EBorderStyle borderStyle)
	{
		BorderStyle = borderStyle;
		char[] borderSymbols = new char[8];

		switch (BorderStyle)
		{
			case EBorderStyle.None:
				Array.Copy(m_borderStyle_0001, borderSymbols, 8);
				break;
			case EBorderStyle.Squared:
				Array.Copy(m_borderStyle_0002, borderSymbols, 8);
				break;
			case EBorderStyle.Rounded:
				Array.Copy(m_borderStyle_0003, borderSymbols, 8);
				break;
			case EBorderStyle.DashedSquared:
				Array.Copy(m_borderStyle_0004, borderSymbols, 8);
				break;
			case EBorderStyle.DashedRounded:
				Array.Copy(m_borderStyle_0005, borderSymbols, 8);
				break;
			case EBorderStyle.DashedPlus:
				Array.Copy(m_borderStyle_0006, borderSymbols, 8);
				break;
			default:
				Array.Copy(m_borderStyle_0006, borderSymbols, 8);
				break;
		}

		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{
				if (j == 0 && i > 0 && i < Height - 1)
				{
					Border[i,j] = new Pixel(ConsoleColor.Black, ConsoleColor.White, borderSymbols[0], true);
				}
				if (j == Width - 1 && i > 0 && i < Height - 1)
				{
					Border[i,j] = new Pixel(ConsoleColor.Black, ConsoleColor.White, borderSymbols[1], true);
				}
				if (j > 0 && j < Width - 1 && i == 0)
				{
					Border[i,j] = new Pixel(ConsoleColor.Black, ConsoleColor.White, borderSymbols[2], true);
				}
				if (j > 0 && j < Width - 1 && i == Height - 1)
				{
					Border[i,j] = new Pixel(ConsoleColor.Black, ConsoleColor.White, borderSymbols[3], true);
				}
				if (j == 0 && i == 0)
				{
					Border[i,j] = new Pixel(ConsoleColor.Black, ConsoleColor.White, borderSymbols[4], true);
				}
				if (j == Width - 1 && i == 0)
				{
					Border[i,j] = new Pixel(ConsoleColor.Black, ConsoleColor.White, borderSymbols[5], true);
				}
				if (j == 0 && i == Height - 1)
				{
					Border[i,j] = new Pixel(ConsoleColor.Black, ConsoleColor.White, borderSymbols[6], true);
				}
				if (j == Width - 1 && i == Height - 1)
				{
					Border[i,j] = new Pixel(ConsoleColor.Black, ConsoleColor.White, borderSymbols[7], true);
				}
				else
				{
					Border[i,j] = new Pixel(ConsoleColor.Black, ConsoleColor.White, ' ', true);
				}
			}
		}
	}

	public void Clear ()
	{
		Screen.Clear();
	}

	public virtual void Refresh ()
	{
		Clear();
	}

	#endregion // Public methods

}

