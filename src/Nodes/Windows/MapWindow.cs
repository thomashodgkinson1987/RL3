public class MapWindow : Window
{

	#region Fields

	private readonly Game m_game;

	#endregion // Fields



	#region Events

	public event EventHandler? PressedEscapeOnMapWindow;

	#endregion // Events



	#region Constructors

	public MapWindow (int x, int y, Game game, int width, int height, EBorderStyle borderStyle = EBorderStyle.DashedPlus) : base ("MapWindow", x, y, "Map", width, height, borderStyle)
	{
		m_game = game;
	}

	#endregion // Constructors



	#region Public methods

	public override void InputTick (float delta)
	{
		ConsoleKey key = Input.LastConsoleKeyInfo.Key;

		if (key == ConsoleKey.Escape)
		{
			EventHandler? handler = PressedEscapeOnMapWindow;
			handler?.Invoke(this, EventArgs.Empty);
		}
		else
		{
			m_game.Steps++;
			m_game.TickPlayer();
			m_game.TickActors(delta);
			m_game.TickCamera();
		}
	}

	public override void Tick(float delta)
	{
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
				Screen.SetPixel(playerGlobalPosition, m_game.Player.node_AnimatedSprite.BackgroundColor, m_game.Player.node_AnimatedSprite.ForegroundColor, m_game.Player.node_AnimatedSprite.Symbol, false);
			}
		}
	}

	public override void Refresh ()
	{
		base.Refresh();

	}

	#endregion // Public methods



	#region Private methods


	private void ProcessMapChunk (object? obj)
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
					Screen.SetPixel(floorGlobalPosition, ConsoleColor.Black, ConsoleColor.White, floor.Symbol, false);
				}
			}
			foreach (Wall wall in mapChunk.Walls)
			{
				if (!wall.IsVisible) continue;

				Point wallGlobalPosition = globalPosition + wall.Position;
				if (Screen.IsPositionOnScreen(wallGlobalPosition))
				{
					Screen.SetPixel(wallGlobalPosition, ConsoleColor.Black, ConsoleColor.White, wall.Symbol, false);
				}
			}
			foreach (Actor actor in mapChunk.Actors)
			{
				if (!actor.IsVisible) continue;

				Point actorGlobalPosition = globalPosition + actor.Position;
				if (Screen.IsPositionOnScreen(actorGlobalPosition))
				{
					if (actor is NPC npc)
					{
						Screen.SetPixel(actorGlobalPosition, npc.node_AnimatedSprite.BackgroundColor, npc.node_AnimatedSprite.ForegroundColor, npc.node_AnimatedSprite.Symbol, false);
					}
					else
					{
						Screen.SetPixel(actorGlobalPosition, ConsoleColor.Black, ConsoleColor.White, actor.Symbol, false);
					}
				}
			}
		}
	}

	#endregion // Private methods

}

