public class TitleWindow : Window
{

	#region Constructors

	public TitleWindow (int x, int y, int width, int height, EBorderStyle borderStyle = EBorderStyle.DashedPlus) : base ("TitleWindow", x, y, "Title", width, height, borderStyle) { }

	#endregion // Constructors



	#region Window methods

	public override void Refresh ()
	{
		Screen.DrawText(28, 1, Screen.EDirection.Right, "***");
		Screen.DrawText(23, 2, Screen.EDirection.Right, "************");
		Screen.DrawText(16, 3, Screen.EDirection.Right, "**************************");
		Screen.DrawText(16, 4, Screen.EDirection.Right, "********** Rowg **********");
		Screen.DrawText(16, 5, Screen.EDirection.Right, "**************************");
		Screen.DrawText(23, 6, Screen.EDirection.Right, "************");
		Screen.DrawText(28, 7, Screen.EDirection.Right, "***");

		Screen.DrawText(15, 9, Screen.EDirection.Right, "A roguelike game by Team Bean");
		Screen.DrawText(22, 10, Screen.EDirection.Right, "Copyright 2022");

		Screen.DrawText(15, 12, Screen.EDirection.Right, "For news and downloads visit:");
		Screen.DrawText(21, 13, Screen.EDirection.Right, "www.teambean.com");

		Screen.DrawText(Width - 3, Height - 3, Screen.EDirection.Left, "v0.1");
	}

	#endregion // Window methods

}

