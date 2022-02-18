public class AnimatedSprite : Node2D
{

	#region Properties

	public List<SpriteAnimation> Animations { get; }
	public int CurrentAnimationIndex { get; private set; }
	public int CurrentFrameIndex { get; private set; }

	public float ElapsedTime { get; private set; }
	public bool IsPlaying { get; set; }
	public bool IsRepeat { get; set; }

	public SpriteAnimation CurrentAnimation => Animations[CurrentAnimationIndex];
	public SpriteFrame CurrentFrame => CurrentAnimation.Frames[CurrentFrameIndex];

	public ConsoleColor BackgroundColor => CurrentFrame.BackgroundColor;
	public ConsoleColor ForegroundColor => CurrentFrame.ForegroundColor;
	public char Symbol => CurrentFrame.Symbol;

	#endregion // Properties



	#region Fields

	private readonly Dictionary<string, int> m_nameToIndexDictionary;

	#endregion // Fields



	#region Events

	public event EventHandler<SpriteAnimationFinishedEventArgs>? AnimationFinished;

	#endregion // Events



	#region Constructors

	public AnimatedSprite(string name, int x, int y, List<SpriteAnimation> animations) : base(name, x, y)
	{
		Animations = new List<SpriteAnimation>();

		CurrentAnimationIndex = 0;
		CurrentFrameIndex = 0;

		ElapsedTime = 0;
		IsPlaying = false;

		m_nameToIndexDictionary = new Dictionary<string, int>();
		for (int i = 0; i < animations.Count; i++)
		{
			SpriteAnimation animation = animations[i];
			Animations.Add(animation);
			m_nameToIndexDictionary.Add(animation.Name, i);
		}
	}

	public AnimatedSprite(string name, List<SpriteAnimation> animations) : this(name, 0, 0, animations) { }

	public AnimatedSprite(int x, int y, List<SpriteAnimation> animations) : this("AnimatedSprite", x, y, animations) { }

	public AnimatedSprite(List<SpriteAnimation> animations) : this("AnimatedSprite", 0, 0, animations) { }

	#endregion // Constructors



	#region Public methods

	public void Play(string name, int frameIndex = 0)
	{
		if (name != CurrentAnimation.Name)
		{
			CurrentAnimationIndex = m_nameToIndexDictionary[name];
			CurrentFrameIndex = frameIndex;

			ElapsedTime = 0;
			IsPlaying = true;
		}
	}

	public void Stop()
	{
		CurrentFrameIndex = 0;
		ElapsedTime = 0;
		IsPlaying = false;
	}

	public void AdvanceAnimation(float delta)
	{
		if (!IsPlaying) return;

		ElapsedTime += delta;

		while(ElapsedTime > CurrentAnimation.FPS)
		{
			ElapsedTime -= CurrentAnimation.FPS;
			CurrentFrameIndex++;
			if (CurrentFrameIndex > CurrentAnimation.Frames.Count - 1)
			{
				CurrentFrameIndex = 0;
				if (!IsRepeat) Stop();

				SpriteAnimationFinishedEventArgs args = new SpriteAnimationFinishedEventArgs();
				args.Animation = CurrentAnimation;
				OnAnimationFinished(args);
			}
		}
	}

	#endregion // Public methods



	#region Protected methods

	protected virtual void OnAnimationFinished(SpriteAnimationFinishedEventArgs e)
	{
		EventHandler<SpriteAnimationFinishedEventArgs>? handler = AnimationFinished;
		handler?.Invoke(this, e);
	}

	#endregion // Protected methods

}

