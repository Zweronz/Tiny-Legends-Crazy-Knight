using UnityEngine;

public class UtilUIJewel : MonoBehaviour
{
	public GameObject[] Jewels;

	public float Interval;

	private float interval;

	private void Start()
	{
	}

	private void Update()
	{
		interval -= Time.deltaTime;
		if (interval <= 0f)
		{
			int num = Jewels.Length;
			int num2 = Random.Range(0, num);
			for (int i = 0; i < num; i++)
			{
				Jewels[i].active = true;
			}
			Jewels[num2].active = false;
			interval = Interval;
		}
	}
}
