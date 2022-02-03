public class GameObject : Node2D
{

	#region Properties

	private char _symbol;
	public char Symbol
	{
		get => _symbol;
		set
		{
			CharChangedEventArgs args = new CharChangedEventArgs();
			args.CharBeforeChange = _symbol;
			args.CharAfterChange = value;

			_symbol = value;

			OnSymbolChanged(args);
		}
	}

	#endregion // Properties



	#region Events

	public event EventHandler<CharChangedEventArgs>? SymbolChanged;

	#endregion // Events



	#region Constructors

	public GameObject(string name, int x, int y, char symbol) : base(name, x, y)
	{
		_symbol = symbol;
	}

	public GameObject(string name, char symbol) : this(name, 0, 0, symbol) { }

	public GameObject(int x, int y, char symbol) : this("GameObject", x, y, symbol) { }

	public GameObject(char symbol) : this("GameObject", 0, 0, symbol) { }

	public GameObject() : this('G') { }

	#endregion // Constructors



	#region Protected methods

	protected virtual void OnSymbolChanged(CharChangedEventArgs e)
	{
		EventHandler<CharChangedEventArgs>? handler = SymbolChanged;
		handler?.Invoke(this, e);
	}

	#endregion // Protected methods

}

