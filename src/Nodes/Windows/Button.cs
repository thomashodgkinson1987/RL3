public class Button
{

	#region Properties

	public string Name { get; set; }

	#endregion // Properties



	#region Events

	public event EventHandler? Selected;
	public event EventHandler? Unselected;
	public event EventHandler? Pressed;

	#endregion // Events



	#region Constructors

	public Button(string name)
	{
		Name = name;
	}

	#endregion // Constructors



	#region Public methods

	public void Select()
	{
		OnSelected();
	}

	public void Unselect()
	{
		OnUnselected();
	}

	public void Press()
	{
		OnPressed();
	}

	#endregion // Public methods



	#region Protected methods

	protected virtual void OnSelected()
	{
		EventHandler? handler = Selected;
		handler?.Invoke(this, EventArgs.Empty);
	}

	protected virtual void OnUnselected()
	{
		EventHandler? handler = Unselected;
		handler?.Invoke(this, EventArgs.Empty);
	}

	protected virtual void OnPressed()
	{
		EventHandler? handler = Pressed;
		handler?.Invoke(this, EventArgs.Empty);
	}

	#endregion // Protected methods

}

