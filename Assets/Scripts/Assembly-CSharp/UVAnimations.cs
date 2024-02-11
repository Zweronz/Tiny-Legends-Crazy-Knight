using System.Collections.Generic;
using UnityEngine;

public class UVAnimations : MonoBehaviour
{
	public GameObject[] uvs;

	public int[] bf;

	private List<UVAnimation> uvp = new List<UVAnimation>();

	public int animationrate;

	private float lasttime;

	private int frame = -1;

	private int cur_uvp;

	private bool ispause;

	private float timefactor = 1f;

	private void Start()
	{
		for (int i = 0; i < uvs.GetLength(0); i++)
		{
			UVAnimation item = uvs[i].GetComponent("UVAnimation") as UVAnimation;
			uvp.Add(item);
		}
	}

	private void Update()
	{
		if (ispause)
		{
			return;
		}
		lasttime += Time.deltaTime * timefactor;
		if (frame == -1)
		{
			return;
		}
		int num = (int)(lasttime * (float)animationrate);
		while (frame <= bf[cur_uvp] && bf[cur_uvp] <= num)
		{
			uvp[cur_uvp].OnUVAnimation(lasttime - (float)bf[cur_uvp] / (float)animationrate);
			cur_uvp++;
			if (cur_uvp >= bf.GetLength(0))
			{
				cur_uvp = 0;
				frame = -1;
				return;
			}
		}
		frame = num;
	}

	public void OnUVAnimation()
	{
		lasttime = 0f;
		frame = 0;
		cur_uvp = 0;
	}

	public void Pause()
	{
		ispause = true;
		for (int i = 0; i < uvp.ToArray().GetLength(0); i++)
		{
			uvp[i].Pause();
		}
	}

	public void Continue()
	{
		ispause = false;
		for (int i = 0; i < uvp.ToArray().GetLength(0); i++)
		{
			uvp[i].Continue();
		}
	}

	public void SetTimeFactor(float factor)
	{
		timefactor = factor;
		for (int i = 0; i < uvp.ToArray().GetLength(0); i++)
		{
			uvp[i].SetTimeFactor(factor);
		}
	}
}
