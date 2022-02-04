public class NodeRemovedEventArgs : EventArgs
{

	public Node2D Node { get; set; } = new Node2D();
	public Node2D PreviousParent { get; set; } = new Node2D();

}

