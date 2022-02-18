public class MessagesWindow : Window
{

	#region Fields

	private readonly Game m_game;

	#endregion // Fields



	#region Constructors

	public MessagesWindow (int x, int y, Game game, int width, int height, EBorderStyle borderStyle = EBorderStyle.DashedPlus) : base ("MessagesWindow", x, y, "Messages", width, height, borderStyle)
	{
		m_game = game;
	}

	#endregion // Constructors



	#region Public methods

	public override void Refresh ()
	{
		base.Refresh();

		Point currentMapChunkPosition = m_game.CurrentMapChunkPosition;
		MapChunk currentMapChunk = m_game.Map.GetMapChunk(currentMapChunkPosition);

		Screen.DrawText(1, 0, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, $"Player: x={m_game.Player.Position.X} y={m_game.Player.Position.Y} gx={m_game.Player.GlobalPosition.X} gy={m_game.Player.GlobalPosition.Y} cx={currentMapChunkPosition.X} cy={currentMapChunkPosition.Y}", false);
		Screen.DrawText(1, 1, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, $"Camera position: x={m_game.Camera.Position.X} y={m_game.Camera.Position.Y} gx={m_game.Camera.GlobalPosition.X} gy={m_game.Camera.GlobalPosition.Y} w={m_game.Camera.Width} h={m_game.Camera.Height}", false);
		Screen.DrawText(1, 2, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, $"Camera bounds: left={m_game.Camera.Bounds.Left} right={m_game.Camera.Bounds.Right} up={m_game.Camera.Bounds.Up} down={m_game.Camera.Bounds.Down}", false);
		Screen.DrawText(1, 3, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, $"Map chunks: all={m_game.Map.AllMapChunks.Count} active={m_game.Map.ActiveMapChunks.Count}", false);
		Screen.DrawText(1, 4, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, $"Actors: all={m_game.Map.AllActors.Count} active={m_game.Map.ActiveActors.Count} chunk={currentMapChunk.Actors.Count}", false);
		Screen.DrawText(1, 5, Screen.EDirection.Right, ConsoleColor.Black, ConsoleColor.White, $"Steps: {m_game.Steps}", false);
	}

	#endregion // Public methods

}

