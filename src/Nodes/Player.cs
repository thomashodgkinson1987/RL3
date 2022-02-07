public class Player : Actor
{

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

	public Player(string name, int x, int y, char symbol, Random rng) : base(name, x, y, symbol, rng)
	{
		_health = 10;
		_maxHealth = 10;
	}

	public Player(string name, char symbol, Random rng) : this(name, 0, 0, symbol, rng) { }

	public Player(int x, int y, char symbol, Random rng) : this("Player", x, y, symbol, rng) { }

	public Player(char symbol, Random rng) : this("Player", 0, 0, symbol, rng) { }

	#endregion // Constructors



	#region Public methods

	public void Tick(ConsoleKeyInfo consoleKeyInfo, Game game)
	{
		Point position = Position;
		Point globalPosition = GlobalPosition;

		if (consoleKeyInfo.KeyChar == '4')
		{
			if (game.IsClear(globalPosition.X - 1, globalPosition.Y))
			{
				position.X--;
			}
		}
		else if (consoleKeyInfo.KeyChar == '6')
		{
			if (game.IsClear(globalPosition.X + 1, globalPosition.Y))
			{
				position.X++;
			}
		}
		else if (consoleKeyInfo.KeyChar == '8')
		{
			if (game.IsClear(globalPosition.X, globalPosition.Y - 1))
			{
				position.Y--;
			}
		}
		else if (consoleKeyInfo.KeyChar == '2')
		{
			if (game.IsClear(globalPosition.X, globalPosition.Y + 1))
			{
				position.Y++;
			}
		}
		else if (consoleKeyInfo.KeyChar == '7')
		{
			if (game.IsClear(globalPosition.X - 1, globalPosition.Y - 1))
			{
				position.X--;
				position.Y--;
			}
		}
		else if (consoleKeyInfo.KeyChar == '9')
		{
			if (game.IsClear(globalPosition.X + 1, globalPosition.Y - 1))
			{
				position.X++;
				position.Y--;
			}
		}
		else if (consoleKeyInfo.KeyChar == '1')
		{
			if (game.IsClear(globalPosition.X - 1, globalPosition.Y + 1))
			{
				position.X--;
				position.Y++;
			}
		}
		else if (consoleKeyInfo.KeyChar == '3')
		{
			if (game.IsClear(globalPosition.X + 1, globalPosition.Y + 1))
			{
				position.X++;
				position.Y++;
			}
		}

		SetPosition(position);
	}

	#endregion // Public methods



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

