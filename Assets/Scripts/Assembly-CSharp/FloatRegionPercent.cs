public class FloatRegionPercent
{
	public FloatRegion fr;

	public float percent;

	public FloatRegionPercent(float min, float max, float per)
	{
		fr = new FloatRegion(min, max);
		percent = per;
	}
}
