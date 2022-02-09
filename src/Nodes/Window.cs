
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

	public char[] Border { get; }

	public Screen Screen { get; }

	public bool IsDirty
	{
		get => Screen.IsDirty;
		set => Screen.IsDirty = value;
	}

	#endregion // Properties



	#region Constructors

	public Window (string name, int x, int y, string title, int width, int height, EBorderStyle borderStyle = EBorderStyle.Rounded) : base(name, x, y)
	{
		Title = title;
		Width = width;
		Height = height;
		BorderStyle = borderStyle;

		switch (BorderStyle)
		{
			case EBorderStyle.None:
				Border = new char[8] { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
				break;
			case EBorderStyle.Squared:
				Border = new char[8] { '─', '─', '│', '│', '┌', '┐', '└', '┘' };
				break;
			case EBorderStyle.Rounded:
				Border = new char[8] { '─', '─', '│', '│', '╭', '╮', '╰', '╯' };
				break;
			case EBorderStyle.DashedSquared:
				Border = new char[8] { '-', '-', '|', '|', '┌', '┐', '└', '┘' };
				break;
			case EBorderStyle.DashedRounded:
				Border = new char[8] { '-', '-', '|', '|', '╭', '╮', '╰', '╯' };
				break;
			case EBorderStyle.DashedPlus:
				Border = new char[8] { '-', '-', '|', '|', '+', '+', '+', '+' };
				break;
			default:
				Border = new char[8] { '─', '─', '│', '│', '┌', '┐', '└', '┘' };
				break;
		}

		Screen = new Screen("Screen", 1, 1, width - 2, height - 2);
		Screen.IsSpaceTransparent = true;

		AddChild(Screen);
	}

	public Window (string name, string title, int width, int height, EBorderStyle borderStyle = EBorderStyle.Rounded) : this(name, 0, 0, title, width, height, borderStyle) { }

	public Window (int x, int y, string title, int width, int height, EBorderStyle borderStyle = EBorderStyle.Rounded) : this("Window", x, y, title, width, height, borderStyle) { }

	public Window (string title, int width, int height, EBorderStyle borderStyle = EBorderStyle.Rounded) : this("Window", 0, 0, title, width, height, borderStyle) { }

	public Window () : this("Title", 92, 38, EBorderStyle.Rounded) { }

	#endregion // Constructors



	#region Node2D methods

	public override void Init ()
	{
		Clear();
	}

	public override void Ready ()
	{
		IsDirty = true;
	}

	#endregion // Node2D



	#region Public methods

	public void Clear ()
	{
		Screen.Clear();
	}

	public virtual void Refresh ()
	{
		Screen.Clear();
	}

	#endregion // Public methods

}

