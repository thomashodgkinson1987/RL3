public class NPCAddedEventArgs : EventArgs
{

	public NPC NPC { get; set; } = new NPC(new Random());

}

