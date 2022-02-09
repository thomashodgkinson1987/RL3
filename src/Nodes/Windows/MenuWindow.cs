public class MenuWindow : Window
{

	#region Properties

	public int SelectionIndex { get; private set; }

	public Button StartGameButton { get; }
	public Button DeleteGameButton { get; }
	public Button TutorialButton { get; }
	public Button OptionsButton { get; }
	public Button ExitGameButton { get; }

	public List<Button> Buttons { get; }

	public char SelectionSymbol { get; }

	#endregion // Properties



	#region Constructors

	public MenuWindow(int x, int y, int width, int height, EBorderStyle borderStyle = EBorderStyle.DashedPlus) : base("MapWindow", x, y, "Menu", width, height, borderStyle)
	{
		StartGameButton = new Button("Start Game");
		DeleteGameButton = new Button("Delete Game");
		TutorialButton = new Button("Tutorial");
		OptionsButton = new Button("Options");
		ExitGameButton = new Button("Exit Game");

		Buttons = new List<Button>()
		{
			StartGameButton,
			DeleteGameButton,
			TutorialButton,
			OptionsButton,
			ExitGameButton
		};

		SelectionSymbol = '>';
		SelectionIndex = 0;
	}

	#endregion // Constructors



	#region Node2D methods

	public override void Tick()
	{
		ConsoleKey key = Input.LastConsoleKeyInfo.Key;

		if (key == ConsoleKey.UpArrow || key == ConsoleKey.NumPad8)
		{
			int oldSelectionIndex = SelectionIndex;

			SelectionIndex--;
			if (SelectionIndex < 0)
				SelectionIndex = Buttons.Count - 1;
			IsDirty = true;

			Buttons[oldSelectionIndex].Unselect();
			Buttons[SelectionIndex].Select();
		}
		else if (key == ConsoleKey.DownArrow || key == ConsoleKey.NumPad2)
		{
			int oldSelectionIndex = SelectionIndex;

			SelectionIndex++;
			if (SelectionIndex > Buttons.Count - 1)
				SelectionIndex = 0;
			IsDirty = true;

			Buttons[oldSelectionIndex].Unselect();
			Buttons[SelectionIndex].Select();
		}
		else if (key == ConsoleKey.Spacebar || key == ConsoleKey.Enter || key == ConsoleKey.NumPad5)
		{
			Buttons[SelectionIndex].Press();
			IsDirty = true;
		}
	}

	#endregion Node2D methods



	#region Window methods

	public override void Refresh()
	{
		base.Refresh();

		Screen.DrawText(0, 0, Screen.EDirection.Right, "Start menu");

		for (int i = 0; i < Buttons.Count; i++)
		{
			string entryString = string.Empty;
			entryString = i == SelectionIndex ? $"{SelectionSymbol} {Buttons[i].Name}" : $"  {Buttons[i].Name}";
			Screen.DrawText(0, i + 1, Screen.EDirection.Right, entryString);
		}
	}

	#endregion // Window methods

}

