public class MapWindow : Window
{

	#region Fields

	private readonly Game m_game;

	#endregion // Fields



	#region Constructors

	public MapWindow (int x, int y, Game game, int width, int height, EBorderStyle borderStyle = EBorderStyle.DashedPlus) : base ("MapWindow", x, y, "Map", width, height, borderStyle)
	{
		m_game = game;
	}

	#endregion // Constructors



	#region Public methods

	public override void Refresh ()
	{
		base.Refresh();

		void ProcessMapChunk (object? obj)
		{
			if (obj != null && obj is MapChunk mapChunk)
			{
				Point globalPosition = mapChunk.GlobalPosition - m_game.Camera.GlobalPosition;
				foreach (Floor floor in mapChunk.Floors)
				{
					if (!floor.IsVisible) continue;

					Point floorGlobalPosition = globalPosition + floor.Position;
					if (Screen.IsPositionOnScreen(floorGlobalPosition))
					{
						Screen.SetSymbol(floorGlobalPosition, floor.Symbol);
					}
				}
				foreach (Wall wall in mapChunk.Walls)
				{
					if (!wall.IsVisible) continue;

					Point wallGlobalPosition = globalPosition + wall.Position;
					if (Screen.IsPositionOnScreen(wallGlobalPosition))
					{
						Screen.SetSymbol(wallGlobalPosition, wall.Symbol);
					}
				}
				foreach (Actor actor in mapChunk.Actors)
				{
					if (!actor.IsVisible) continue;

					Point actorGlobalPosition = globalPosition + actor.Position;
					if (Screen.IsPositionOnScreen(actorGlobalPosition))
					{
						Screen.SetSymbol(actorGlobalPosition, actor.Symbol);
					}
				}
			}
		}

		int activeChunksLeftCount = m_game.Map.ActiveMapChunks.Count;
		using (ManualResetEvent resetEvent = new ManualResetEvent(false))
		{
			for (int i = 0; i < m_game.Map.ActiveMapChunks.Count; i++)
			{
				ThreadPool.QueueUserWorkItem(
					new WaitCallback(x => {
						ProcessMapChunk(x);
						if (Interlocked.Decrement(ref activeChunksLeftCount) == 0)
						{
							resetEvent.Set();
						}
					}), m_game.Map.ActiveMapChunks[i]);
			}
			resetEvent.WaitOne();
		}

		if (m_game.Player.IsVisible)
		{
			Point playerGlobalPosition = m_game.Player.GlobalPosition - m_game.Camera.GlobalPosition;
			if (Screen.IsPositionOnScreen(playerGlobalPosition))
			{
				Screen.SetSymbol(playerGlobalPosition, m_game.Player.Symbol);
			}
		}
	}

	#endregion // Public methods

}

