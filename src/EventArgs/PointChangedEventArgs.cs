public class PointChangedEventArgs : EventArgs
{

	public Point PointBeforeChange { get; set; } = new Point();
	public Point PointAfterChange { get; set; } = new Point();

}

