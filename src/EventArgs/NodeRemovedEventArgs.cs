public class NodeRemovedEventArgs : EventArgs
{

	public Node2D? Node { get; set; }
	public Node2D? PreviousParent { get; set; }

}

