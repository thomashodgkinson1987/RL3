public class ActorAddedEventArgs : EventArgs
{

	public Actor Actor { get; set; } = new Actor('A', new Random());

}

