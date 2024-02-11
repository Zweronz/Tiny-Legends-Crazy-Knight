using UnityEngine;

public class UtilUIMoveY : MonoBehaviour
{
	private float Ymax;

	private float Ymin;

	private float Ycurrent;

	private void SetYMax(float max)
	{
		Ymax = max;
	}

	private void SetYMin(float min)
	{
		Ymin = min;
	}

	private void Move(float delta)
	{
		float num = Mathf.Clamp(Ycurrent + delta, Ymin, Ymax);
		base.gameObject.transform.position += new Vector3(0f, num - Ycurrent, 0f);
		Ycurrent = num;
	}
}
