public class NPC : Actor
{

	#region Nodes

	public AnimatedSprite node_AnimatedSprite { get; }

	#endregion // Nodes



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

		List<SpriteFrame> default_frames = new List<SpriteFrame>()
		{
			new SpriteFrame(ConsoleColor.Black, ConsoleColor.White, 'N')
		};

		List<SpriteFrame> idle_frames = new List<SpriteFrame>()
		{
			new SpriteFrame(ConsoleColor.DarkGray, ConsoleColor.White, 'N'),
			new SpriteFrame(ConsoleColor.Gray, ConsoleColor.White, 'N')
		};

		List<SpriteFrame> hostile_frames = new List<SpriteFrame>()
		{
			new SpriteFrame(ConsoleColor.DarkRed, ConsoleColor.White, 'N'),
			new SpriteFrame(ConsoleColor.Red, ConsoleColor.White, 'N')
		};

		List<SpriteFrame> neutral_frames = new List<SpriteFrame>()
		{
			new SpriteFrame(ConsoleColor.DarkGreen, ConsoleColor.White, 'N'),
			new SpriteFrame(ConsoleColor.Green, ConsoleColor.White, 'N')
		};

		List<SpriteFrame> friendly_frames = new List<SpriteFrame>()
		{
			new SpriteFrame(ConsoleColor.DarkBlue, ConsoleColor.White, 'N'),
			new SpriteFrame(ConsoleColor.Blue, ConsoleColor.White, 'N')
		};

		List<SpriteAnimation> animations = new List<SpriteAnimation>()
		{
			new SpriteAnimation("default", default_frames, 1, true),
			new SpriteAnimation("idle", idle_frames, 1, true),
			new SpriteAnimation("hostile", hostile_frames, 1, true),
			new SpriteAnimation("neutral", neutral_frames, 1, true),
			new SpriteAnimation("friendly", friendly_frames, 1, true)
		};

		node_AnimatedSprite = new AnimatedSprite(animations);

		AddChild(node_AnimatedSprite);

		IsFollowPlayer = false;
		PlayerSearchDistance = 8;
		ChanceToDoMove = 50;
	}

	public NPC (Random rng) : this(0, 0, rng) { }

	#endregion // Constructors



	#region Node2D methods

	public override void Ready()
	{
		node_AnimatedSprite.Play("idle");
	}

	public override void InputTick(float delta)
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
					node_AnimatedSprite.Play("hostile");
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
					node_AnimatedSprite.Play("neutral");
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
			node_AnimatedSprite.Play("friendly");
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

	public override void Tick(float delta)
	{
		node_AnimatedSprite.AdvanceAnimation(delta);
	}

	#endregion // Node2D methods

}

