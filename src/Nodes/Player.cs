public class Player : Node2D
{

	#region Properties

	public char Symbol { get; set; }
	public int Health { get; set; } = 10;
	public int MaxHealth { get; set; } = 10;

	#endregion // Properties



	#region Events

	public event EventHandler<IntChangedEventArgs>? HealthChanged;
	public event EventHandler<IntChangedEventArgs>? MaxHealthChanged;

	#endregion // Events



	#region Constructors

	public Player(string name, int x, int y, char symbol, int health, int maxHealth) : base(name, x, y)
	{
		Symbol = symbol;
		Health = health;
		MaxHealth = maxHealth;
	}

	public Player(int x, int y, char symbol, int health, int maxHealth) : this("Player", x, y, symbol, health, maxHealth) { }

	public Player(char symbol, int health, int maxHealth) : this(0, 0, symbol, health, maxHealth) { }

	public Player(char symbol, int health) : this(symbol, health, health) { }

	public Player(char symbol) : this(symbol, 10) { }

	public Player(string name, int x, int y) : this(name, x, y, '@', 10, 10) { }

	public Player() : this('@') { }

	#endregion // Constructors



	#region Public methods

	public void SetHealth(int health)
	{
		int healthBeforeChange = Health;
		Health = health >= 0 && health <= MaxHealth ? health : Health;

		IntChangedEventArgs args = new IntChangedEventArgs();
		args.ValueBeforeChange = healthBeforeChange;
		args.ValueAfterChange = Health;
		OnHealthChanged(args);
	}

	public void SetMaxHealth(int maxHealth)
	{
		int maxHealthBeforeChange = MaxHealth;
		MaxHealth = maxHealth;

		if (Health > MaxHealth)
		{
			SetHealth(MaxHealth);
		}

		IntChangedEventArgs args = new IntChangedEventArgs();
		args.ValueBeforeChange = maxHealthBeforeChange;
		args.ValueAfterChange = MaxHealth;
		OnMaxHealthChanged(args);
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

