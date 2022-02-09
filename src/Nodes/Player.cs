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

	public Player(string name, int x, int y) : base(name, x, y, '@')
	{
		_health = 10;
		_maxHealth = 10;
	}

	public Player(string name) : this(name, 0, 0) { }

	public Player(int x, int y) : this("Player", x, y) { }

	public Player() : this("Player", 0, 0) { }

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

