public class MenuWindow : Window
{

	#region Fields

	private readonly Game m_game;

	#endregion // Fields



	#region Constructors

	public MenuWindow (int x, int y, Game game, int width, int height, EBorderStyle borderStyle = EBorderStyle.DashedPlus) : base ("MapWindow", x, y, "Menu", width, height, borderStyle)
	{
		m_game = game;
	}

	#endregion // Constructors



	#region Public methods

	public override void Refresh ()
	{
		base.Refresh();

		Screen.DrawText(0, 0, Screen.EDirection.Right, "Start menu");
		Screen.DrawText(0, 1, Screen.EDirection.Right, "> Start Game");
		Screen.DrawText(0, 2, Screen.EDirection.Right, "  Delete Game");
		Screen.DrawText(0, 3, Screen.EDirection.Right, "  Tutorial");
		Screen.DrawText(0, 4, Screen.EDirection.Right, "  Options");
		Screen.DrawText(0, 5, Screen.EDirection.Right, "  Exit Game");
	}

	#endregion // Public methods

}

