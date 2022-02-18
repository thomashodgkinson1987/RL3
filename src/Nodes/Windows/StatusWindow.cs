public class StatusWindow : Window
{

	#region Fields

	private readonly Game m_game;

	#endregion // Fields



	#region Constructors

	public StatusWindow (int x, int y, Game game, int width, int height, EBorderStyle borderStyle = EBorderStyle.DashedPlus) : base ("StatusWindow", x, y, "Status", width, height, borderStyle)
	{
		m_game = game;
	}

	#endregion // Constructors



	#region Public methods

	public override void Refresh ()
	{
		base.Refresh();

		Screen.DrawText(1, 1, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "exploring", false);
		Screen.DrawText(1, 2, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "martial combo<1>", false);
		Screen.DrawText(1, 3, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "duel wield", false);
		Screen.DrawText(1, 4, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "in light", false);
	}

	#endregion // Public methods

}

