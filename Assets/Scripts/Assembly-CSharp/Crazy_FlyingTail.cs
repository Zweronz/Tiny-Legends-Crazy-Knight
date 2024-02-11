using UnityEngine;

public class Crazy_FlyingTail : MonoBehaviour
{
	public float gravity = -9.8f;

	public float time = 1f;

	protected Vector3 beginposition;

	protected Vector3 endposition;

	protected bool flying;

	protected float speedxz;

	protected float speedy;

	protected Vector2 vxz;

	public GameObject convert;

	public GameObject determine;

	public GameObject aim;

	protected GameObject target;

	public float destroytime;

	private void Start()
	{
	}

	public void SetTarget(GameObject tar)
	{
		target = tar;
	}

	public void OnFly(Vector3 des)
	{
		beginposition = base.gameObject.transform.position;
		endposition = des;
		flying = true;
		Vector2 vector = new Vector2(endposition.x - beginposition.x, endposition.z - beginposition.z);
		speedxz = vector.magnitude / time;
		vxz = vector.normalized;
		speedy = (endposition.y - beginposition.y) / time - 0.5f * gravity * time;
		flying = true;
		Aim();
	}

	private void Update()
	{
		if (flying)
		{
			Vector3 vector = new Vector3(vxz.x * speedxz * Time.deltaTime, speedy * Time.deltaTime + 0.5f * gravity * Time.deltaTime * Time.deltaTime, vxz.y * speedxz * Time.deltaTime);
			speedy += gravity * Time.deltaTime;
			base.gameObject.transform.position += vector;
			if ((endposition - base.gameObject.transform.position).sqrMagnitude < 0.1f || (base.gameObject.transform.position.y < 0.1f && speedy < 0f))
			{
				base.gameObject.transform.position = endposition;
				flying = false;
				Determine();
				Convert();
			}
		}
	}

	private void Aim()
	{
		if (aim != null)
		{
			GameObject gameObject = Object.Instantiate(aim) as GameObject;
			gameObject.transform.position = endposition + new Vector3(0f, 0.1f, 0f);
			gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, Random.Range(0f, 360f), gameObject.transform.eulerAngles.z);
			gameObject.transform.parent = base.transform.parent;
			gameObject.SendMessage("Play", SendMessageOptions.DontRequireReceiver);
		}
	}

	private void Convert()
	{
		GameObject gameObject = Object.Instantiate(convert) as GameObject;
		gameObject.transform.parent = base.transform.parent;
		gameObject.transform.localPosition = base.transform.localPosition;
		gameObject.SendMessage("EmitParticle", SendMessageOptions.DontRequireReceiver);
		base.gameObject.SendMessage("Stop", SendMessageOptions.DontRequireReceiver);
		base.gameObject.AddComponent<Crazy_AutoDestroy>().timeToDestroy = destroytime;
	}

	private void Determine()
	{
		GameObject gameObject = Object.Instantiate(determine) as GameObject;
		gameObject.transform.parent = base.transform.parent;
		gameObject.transform.localPosition = base.transform.localPosition;
		gameObject.SendMessage("SetTarget", target, SendMessageOptions.DontRequireReceiver);
		gameObject.SendMessage("OnAttackJudgment", SendMessageOptions.DontRequireReceiver);
	}
}
