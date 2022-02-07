public class ActorRemovedEventArgs : EventArgs
{

	public Actor Actor { get; set; } = new Actor('A', new Random());

}

