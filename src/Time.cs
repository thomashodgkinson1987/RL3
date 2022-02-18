public class Time
{

	#region Properties

	public static float FixedTimeStep { get; private set; }
	public static float Delta { get; private set; }
	public static float Elapsed { get; private set; }

	#endregion // Properties



	#region Constructors

	public Time (float fixedTimeStep)
	{
		FixedTimeStep = fixedTimeStep;
		Delta = 0;
		Elapsed = 0;
	}

	#endregion // Constructors



	#region Public methods

	public void SetFixedDelta(float delta)
	{
		FixedTimeStep = delta;
	}

	public void SetDelta(float delta)
	{
		Delta = delta;
	}

	public void SetElapsed(float elapsed)
	{
		Elapsed = elapsed;
	}

	#endregion // Public methods

}

