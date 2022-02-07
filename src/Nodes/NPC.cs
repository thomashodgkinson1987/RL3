public class NPC : Actor
{

	#region Properties

	public bool IsFollowPlayer { get; set; }
	public int PlayerSearchDistance { get; set; }

	public int ChanceToDoMove { get; set; }

	#endregion // Properties



	#region Constructors

	public NPC (string name, int x, int y, char symbol, Random rng) : base(name, x, y, symbol, rng)
	{
		IsFollowPlayer = false;
		PlayerSearchDistance = 8;
		ChanceToDoMove = 50;
	}

	public NPC (string name, char symbol, Random rng) : this(name, 0, 0, symbol, rng) { }

	public NPC (int x, int y, char symbol, Random rng) : this("NPC", x, y, symbol, rng) { }

	public NPC (char symbol, Random rng) : this("NPC", 0, 0, symbol, rng) { }

	#endregion // Constructors



	#region Actor methods

	public override void Tick()
	{
		Game game = RootNode.GetNode<Game>("Game");

		Point globalPosition = GlobalPosition;

		if (IsFollowPlayer)
		{
			if (m_rng.Next(0, 100) <= ChanceToDoMove)
			{
				Player player = RootNode.GetNode<Player>("Player");
				Point playerGlobalPosition = player.GlobalPosition;
				if (MathF.Abs(playerGlobalPosition.X - globalPosition.X) <= PlayerSearchDistance && MathF.Abs(playerGlobalPosition.Y - globalPosition.Y) <= PlayerSearchDistance)
				{
					int dx = playerGlobalPosition.X < globalPosition.X ? -1 : playerGlobalPosition.X > globalPosition.X ? 1 : 0;
					int dy = playerGlobalPosition.Y < globalPosition.Y ? -1 : playerGlobalPosition.Y > globalPosition.Y ? 1 : 0;
					Point dp = new Point(dx, dy);
					if ((dx != 0 || dy != 0) && game.IsClear(globalPosition + dp))
					{
						Translate(dp);
					}
				}
				else
				{
					if (m_rng.Next(0, 100) <= ChanceToDoMove)
					{
						int dx = m_rng.Next(0, 2) == 0 ? -1 : 1;
						int dy = m_rng.Next(0, 2) == 0 ? -1 : 1;
						Point dp = new Point(dx, dy);

						if (game.IsClear(globalPosition + dp))
						{
							Translate(dp);
						}
					}
				}
			}
		}
		else
		{
			if (m_rng.Next(0, 100) <= ChanceToDoMove)
			{
				int dx = m_rng.Next(0, 2) == 0 ? -1 : 1;
				int dy = m_rng.Next(0, 2) == 0 ? -1 : 1;
				Point dp = new Point(dx, dy);

				if (game.IsClear(globalPosition + dp))
				{
					Translate(dp);
				}
			}
		}
	}

	#endregion // Actor methods

}

