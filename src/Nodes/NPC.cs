public class NPC : Actor
{

	#region Properties

	public bool IsFollowPlayer { get; set; }
	public int PlayerSearchDistance { get; set; }
	public int ChanceToDoMove { get; set; }

	#endregion // Properties



	#region Fields

	private readonly Random m_rng;

	#endregion // Fields



	#region Constructors

	public NPC (int x, int y, Random rng) : base("NPC", x, y, 'N')
	{
		m_rng = rng;

		IsFollowPlayer = false;
		PlayerSearchDistance = 8;
		ChanceToDoMove = 50;
	}

	public NPC (Random rng) : this(0, 0, rng) { }

	#endregion // Constructors



	#region Actor methods

	public override void Tick()
	{
		Game game = RootNode.Instance.GetNode<Game>("Game");

		Point globalPosition = GlobalPosition;

		if (IsFollowPlayer)
		{
			if (m_rng.Next(0, 100) <= ChanceToDoMove)
			{
				Player player = game.GetNode<Player>("Player");
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

