using UnityEngine;

public class UVAnimation : MonoBehaviour
{
	public bool loop;

	public int animationrate;

	public int uCount = 1;

	public int vCount = 1;

	private float uCell;

	private float vCell;

	private int cellIndex;

	public int[] frameList = new int[1];

	private int lastframe;

	private int fCount;

	private float lasttime;

	private float timefactor = 1f;

	private bool ispause;

	private void Start()
	{
		if (uCount < 1)
		{
			uCount = 1;
		}
		if (vCount < 1)
		{
			vCount = 1;
		}
		if (frameList.Length < uCount * vCount)
		{
			Debug.LogError("The Frame List length is not correct!!");
		}
		cellIndex = 0;
		fCount = frameList[cellIndex];
		uCell = 1f / (float)uCount;
		vCell = 1f / (float)vCount;
		base.gameObject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(uCell, vCell);
		base.gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f, 1f - ((float)cellIndex + 1f) % (float)vCount * vCell);
		base.gameObject.active = false;
	}

	private void Update()
	{
		if (ispause)
		{
			return;
		}
		lasttime += Time.deltaTime * timefactor;
		int num = (int)(lasttime * (float)animationrate);
		if (lastframe < num)
		{
			if (fCount <= 0)
			{
				cellIndex++;
				if (cellIndex >= uCount * vCount)
				{
					cellIndex = 0;
					if (!loop)
					{
						base.gameObject.active = false;
					}
				}
				fCount = frameList[cellIndex];
			}
			fCount--;
			lastframe++;
		}
		float x = (float)(cellIndex % uCount) * uCell;
		float y = 1f - ((float)cellIndex + 1f) % (float)vCount * vCell;
		base.gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(x, y);
	}

	public void OnUVAnimation(float time)
	{
		lasttime = time;
		lastframe = 0;
		cellIndex = 0;
		fCount = frameList[cellIndex] - 1;
		base.gameObject.active = true;
	}

	public void OnUVAnimation()
	{
		lasttime = 0f;
		lastframe = 0;
		cellIndex = 0;
		fCount = frameList[cellIndex] - 1;
		base.gameObject.active = true;
	}

	public void Pause()
	{
		ispause = true;
	}

	public void Continue()
	{
		ispause = false;
	}

	public void SetTimeFactor(float factor)
	{
		timefactor = factor;
	}
}
