public class Player : GameObject
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

	public Player(string name, int x, int y, int health) : base(name, x, y, '@')
	{
		_health = health;
		_maxHealth = health;
	}

	public Player(string name, int health) : this(name, 0, 0, health) { }

	public Player(int x, int y, int health) : this("Player", x, y, health) { }

	public Player(int health) : this("Player", 0, 0, health) { }

	public Player() : this(10) { }

	#endregion // Constructors



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

