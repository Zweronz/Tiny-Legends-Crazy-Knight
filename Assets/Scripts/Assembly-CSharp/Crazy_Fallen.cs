using UnityEngine;

public class Crazy_Fallen : MonoBehaviour
{
	public float fallentime = 0.5f;

	protected float lastfallentime;

	protected bool flying;

	public GameObject determine;

	public GameObject aim;

	protected GameObject target;

	protected bool isdetermine;

	private void Start()
	{
	}

	public void SetTarget(GameObject tar)
	{
		target = tar;
	}

	public void OnFly()
	{
		Aim();
	}

	private void Update()
	{
		lastfallentime += Time.deltaTime;
		if (lastfallentime >= fallentime)
		{
			Determine();
		}
	}

	private void Aim()
	{
		if (aim != null)
		{
			GameObject gameObject = Object.Instantiate(aim) as GameObject;
			gameObject.transform.position = base.transform.position + new Vector3(0f, 0.1f, 0f);
			gameObject.transform.parent = base.transform.parent;
			gameObject.SendMessage("Trigger", SendMessageOptions.DontRequireReceiver);
		}
	}

	private void Determine()
	{
		if (!isdetermine)
		{
			Crazy_PlayTAudio component = base.gameObject.GetComponent<Crazy_PlayTAudio>();
			if (component != null)
			{
				component.Play();
			}
			isdetermine = true;
			GameObject gameObject = Object.Instantiate(determine) as GameObject;
			gameObject.transform.parent = base.transform.parent;
			gameObject.transform.localPosition = base.transform.localPosition;
			gameObject.SendMessage("SetTarget", target, SendMessageOptions.DontRequireReceiver);
			gameObject.SendMessage("OnAttackJudgment", SendMessageOptions.DontRequireReceiver);
		}
	}
}
