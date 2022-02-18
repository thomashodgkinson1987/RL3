public class TitleWindow : Window
{

	#region Constructors

	public TitleWindow (int x, int y, int width, int height, EBorderStyle borderStyle = EBorderStyle.DashedPlus) : base ("TitleWindow", x, y, "Title", width, height, borderStyle) { }

	#endregion // Constructors



	#region Window methods

	public override void Refresh ()
	{
		Screen.DrawText(28, 1, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "***", false);
		Screen.DrawText(23, 2, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "************", false);
		Screen.DrawText(16, 3, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "**************************", false);
		Screen.DrawText(16, 4, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "********** Rowg **********", false);
		Screen.DrawText(16, 5, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "**************************", false);
		Screen.DrawText(23, 6, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "************", false);
		Screen.DrawText(28, 7, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "***", false);

		Screen.DrawText(15, 9, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "A roguelike game by Team Bean", false);
		Screen.DrawText(22, 10, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "Copyright 2022", false);

		Screen.DrawText(15, 12, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "For news and downloads visit:", false);
		Screen.DrawText(21, 13, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, "www.teambean.com", false);

		Screen.DrawText(Width - 3, Height - 3, Screen.EDirection.Left, ConsoleColor.Black, ConsoleColor.White, "v0.1", false);
	}

	#endregion // Window methods

}

