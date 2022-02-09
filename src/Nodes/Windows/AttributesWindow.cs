public class AttributesWindow : Window
{

	#region Fields

	private readonly Game m_game;

	#endregion // Fields



	#region Constructors

	public AttributesWindow (int x, int y, Game game, int width, int height, EBorderStyle borderStyle = EBorderStyle.DashedPlus) : base ("MapWindow", x, y, "Attributes", width, height, borderStyle)
	{
		m_game = game;
	}

	#endregion // Constructors



	#region Public methods

	public override void Refresh ()
	{
		base.Refresh();

		Screen.DrawText(0, 0, Screen.EDirection.Right, $"Hp:{m_game.Player.Health}/{m_game.Player.MaxHealth} Mp: 7/ 7 Ep: 10 Rep: 0 Food: 4");
		Screen.DrawText(0, 1, Screen.EDirection.Right, "Melee: 60%");
		Screen.DrawText(Screen.Width - 1, 1, Screen.EDirection.Left, "Vision: 3 Noise: 7");
		Screen.DrawText(Screen.Width - 1, 2, Screen.EDirection.Left, "Thievery: 25%");
	}

	#endregion // Public methods

}

