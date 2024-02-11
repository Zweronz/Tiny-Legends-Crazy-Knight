using UnityEngine;

public class FloatRegion
{
	public float minf;

	public float maxf;

	public FloatRegion(float min, float max)
	{
		minf = min;
		maxf = max;
	}

	public float RandomFloat()
	{
		return Random.Range(minf, maxf);
	}
}
