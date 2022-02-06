public class NPC : GameObject
{

	#region Properties

	public bool IsFollowPlayer { get; set; }
	public int PlayerSearchDistance { get; set; }

	public int ChanceToDoRandomMove { get; set; }

	#endregion // Properties



	#region Fields

	private readonly Random m_rng;

	#endregion // Fields



	#region Constructors

	public NPC (string name, int x, int y, char symbol, Random rng) : base(name, x, y, symbol)
	{
		m_rng = rng;

		IsFollowPlayer = false;
		PlayerSearchDistance = 8;
		ChanceToDoRandomMove = 50;
	}

	#endregion // Constructors



	#region Public methods

	public void Tick(Game game)
	{
		if (IsFollowPlayer)
		{
			if (m_rng.Next(0, 100) <= ChanceToDoRandomMove)
			{
				Player player = RootNode.GetNode<Player>("Player");
				if (MathF.Abs(player.GlobalPosition.X - GlobalPosition.X) <= PlayerSearchDistance && MathF.Abs(player.GlobalPosition.Y - GlobalPosition.Y) <= PlayerSearchDistance)
				{
					int dx = player.GlobalPosition.X < GlobalPosition.X ? -1 : player.GlobalPosition.X > GlobalPosition.X ? 1 : 0;
					int dy = player.GlobalPosition.Y < GlobalPosition.Y ? -1 : player.GlobalPosition.Y > GlobalPosition.Y ? 1 : 0;
					if ((dx != 0 || dy != 0) && game.IsClear(GlobalPosition.X + dx, GlobalPosition.Y + dy))
					{
						SetPosition(GlobalPosition.X + dx, GlobalPosition.Y + dy);
					}
				}
				else
				{
					if (m_rng.Next(0, 100) <= ChanceToDoRandomMove)
					{
						int dx = m_rng.Next(0, 2) == 0 ? -1 : 1;
						int dy = m_rng.Next(0, 2) == 0 ? -1 : 1;

						if (game.IsClear(GlobalPosition.X + dx, GlobalPosition.Y + dy))
						{
							SetPosition(GlobalPosition.X + dx, GlobalPosition.Y + dy);
						}
					}
				}
			}
		}
		else
		{
			if (m_rng.Next(0, 100) <= ChanceToDoRandomMove)
			{
				int dx = m_rng.Next(0, 2) == 0 ? -1 : 1;
				int dy = m_rng.Next(0, 2) == 0 ? -1 : 1;

				if (game.IsClear(GlobalPosition.X + dx, GlobalPosition.Y + dy))
				{
					SetPosition(GlobalPosition.X + dx, GlobalPosition.Y + dy);
				}
			}
		}
	}

	#endregion // Public methods

}

