public class NPCRemovedEventArgs : EventArgs
{

	public NPC NPC { get; set; } = new NPC(new Random());

}

