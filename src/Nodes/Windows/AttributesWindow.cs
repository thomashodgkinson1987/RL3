public class AttributesWindow : Window
{

	#region Fields

	private readonly Game m_game;

	#endregion // Fields



	#region Constructors

	public AttributesWindow (int x, int y, Game game, int width, int height, EBorderStyle borderStyle = EBorderStyle.DashedPlus) : base ("AttributesWindow", x, y, "Attributes", width, height, borderStyle)
	{
		m_game = game;
	}

	#endregion // Constructors



	#region Public methods

	public override void Refresh ()
	{
		base.Refresh();

		Screen.DrawText(0, 0, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, $"Hp:{m_game.Player.Health}/{m_game.Player.MaxHealth} Mp: 7/ 7 Ep: 10 Rep: 0 Food: 4", false);
		Screen.DrawText(0, 1, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "Melee: 60%", false);
		Screen.DrawText(Screen.Width - 1, 1, Screen.EDirection.Left, ConsoleColor.Black, ConsoleColor.White, "Vision: 3 Noise: 7", false);
		Screen.DrawText(Screen.Width - 1, 2, Screen.EDirection.Left, ConsoleColor.Black, ConsoleColor.White, "Thievery: 25%", false);
	}

	#endregion // Public methods

}

