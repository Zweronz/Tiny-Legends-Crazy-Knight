using UnityEngine;

public class Crazy_PlayAnimationStart : MonoBehaviour, ICrazyEffectEvent
{
	public float timeTosleep = 1f;

	public string animationname;

	private float lasttimetosleep;

	public float speed = 1f;

	public GameObject[] objs;

	private void Start()
	{
		lasttimetosleep = timeTosleep;
		Play();
	}

	private void Update()
	{
		lasttimetosleep -= Time.deltaTime;
		if (lasttimetosleep < 0f)
		{
			base.gameObject.active = false;
			GameObject[] array = objs;
			foreach (GameObject gameObject in array)
			{
				gameObject.active = false;
			}
		}
	}

	public void Stop()
	{
		if (base.gameObject.active)
		{
			base.gameObject.GetComponent<Animation>()[animationname].speed = speed;
			base.gameObject.GetComponent<Animation>().Stop(animationname);
		}
	}

	public void Play()
	{
		base.gameObject.active = true;
		GameObject[] array = objs;
		foreach (GameObject gameObject in array)
		{
			gameObject.active = true;
		}
		base.gameObject.GetComponent<Animation>()[animationname].speed = speed;
		base.gameObject.GetComponent<Animation>().Play(animationname);
		lasttimetosleep = timeTosleep;
	}

	public void Play(float time)
	{
		base.gameObject.active = true;
		GameObject[] array = objs;
		foreach (GameObject gameObject in array)
		{
			gameObject.active = true;
		}
		base.gameObject.GetComponent<Animation>()[animationname].speed = speed;
		base.gameObject.GetComponent<Animation>().Play(animationname);
		lasttimetosleep = time;
	}

	public void Hide()
	{
		base.gameObject.active = false;
		GameObject[] array = objs;
		foreach (GameObject gameObject in array)
		{
			gameObject.active = false;
		}
	}

	public void Trigger()
	{
		Play();
	}
}
