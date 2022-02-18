public class SpriteAnimation
{

	#region Properties

	public string Name { get; set; }
	public List<SpriteFrame> Frames { get; }
	public float FPS { get; set; }
	public bool IsRepeat { get; set; }

	#endregion // Properties



	#region Constructors

	public SpriteAnimation(string name, List<SpriteFrame> frames, float fps, bool isRepeat)
	{
		Name = name;
		Frames = new List<SpriteFrame>();
		frames.ForEach(_ => Frames.Add(_));
		FPS = fps;
		IsRepeat = isRepeat;
	}

	#endregion // Constructors

}

