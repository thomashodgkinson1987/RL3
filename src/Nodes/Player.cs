public class Player : Actor
{

	#region Nodes

	public AnimatedSprite node_AnimatedSprite { get; }

	#endregion // Nodes



	#region Properties

	private int _health;
	public int Health
	{
		get => _health;
		set
		{
			IntChangedEventArgs args = new IntChangedEventArgs();
			args.IntBeforeChange = _health;

			_health = value >= 0 && value <= MaxHealth ? value : _health;

			args.IntAfterChange = _health;
			OnHealthChanged(args);
		}
	}

	private int _maxHealth;
	public int MaxHealth
	{
		get => _maxHealth;
		set
		{
			IntChangedEventArgs args = new IntChangedEventArgs();
			args.IntBeforeChange = _maxHealth;

			int newMaxHealth = value >= 0 && value <= 99 ? value : _maxHealth;

			if (Health > newMaxHealth)
			{
				Health = newMaxHealth;
			}

			_maxHealth = newMaxHealth;

			args.IntAfterChange = _maxHealth;
			OnMaxHealthChanged(args);
		}
	}

	#endregion // Properties



	#region Events

	public event EventHandler<IntChangedEventArgs>? HealthChanged;
	public event EventHandler<IntChangedEventArgs>? MaxHealthChanged;

	#endregion // Events



	#region Constructors

	public Player(string name, int x, int y) : base(name, x, y, '@')
	{
		List<SpriteFrame> frames = new List<SpriteFrame>()
		{
			new SpriteFrame(ConsoleColor.Blue, ConsoleColor.White, '@'),
			new SpriteFrame(ConsoleColor.Magenta, ConsoleColor.White, '@'),
			new SpriteFrame(ConsoleColor.Green, ConsoleColor.White, '@')
		};

		List<SpriteAnimation> spriteAnimations = new List<SpriteAnimation>()
		{
			new SpriteAnimation("default", frames, 1, true)
		};

		node_AnimatedSprite = new AnimatedSprite(spriteAnimations);

		AddChild(node_AnimatedSprite);

		_health = 10;
		_maxHealth = 10;
	}

	public Player(string name) : this(name, 0, 0) { }

	public Player(int x, int y) : this("Player", x, y) { }

	public Player() : this("Player", 0, 0) { }

	#endregion // Constructors



	#region Node2D methods

	public override void Ready()
	{
		base.Ready();

		node_AnimatedSprite.Play("default");
	}

	public override void Tick(float delta)
	{
		node_AnimatedSprite.AdvanceAnimation(delta);
	}

	#endregion // Node2D methods



	#region Protected methods

	protected virtual void OnHealthChanged(IntChangedEventArgs e)
	{
		EventHandler<IntChangedEventArgs>? handler = HealthChanged;
		handler?.Invoke(this, e);
	}

	protected virtual void OnMaxHealthChanged(IntChangedEventArgs e)
	{
		EventHandler<IntChangedEventArgs>? handler = MaxHealthChanged;
		handler?.Invoke(this, e);
	}

	#endregion // Protected methods

}

