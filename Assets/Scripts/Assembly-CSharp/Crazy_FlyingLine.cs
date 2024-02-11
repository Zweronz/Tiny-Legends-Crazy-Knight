using System.Collections.Generic;
using UnityEngine;

public class Crazy_FlyingLine : MonoBehaviour
{
	public GameObject FlyingObject;

	public Vector3 moveDir;

	public float Speed;

	public float Radius;

	public List<GameObject> TargetList = new List<GameObject>();

	public float survive;

	private float interval;

	private float time;

	private void Awake()
	{
		interval = Radius * 2f / Speed;
		time = 0f;
	}

	private void Start()
	{
		GameObject gameObject = Object.Instantiate(FlyingObject) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localEulerAngles = Vector3.zero;
		gameObject.SendMessage("Trigger", SendMessageOptions.DontRequireReceiver);
		gameObject.SendMessage("Play", SendMessageOptions.DontRequireReceiver);
	}

	private void Update()
	{
		time += Time.deltaTime;
		base.transform.position += moveDir * Speed * Time.deltaTime;
		if (time >= interval)
		{
			OnAttackJudgment();
			time = 0f;
			survive -= interval;
			if (survive <= 0f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	private void OnAttackJudgment()
	{
		foreach (GameObject target in TargetList)
		{
			OnAttackJudgment(target);
		}
	}

	public void OnAttackJudgment(GameObject target)
	{
		if (AttackJudgment(target))
		{
			Crazy_PlayerControl crazy_PlayerControl = target.GetComponent("Crazy_PlayerControl") as Crazy_PlayerControl;
			if (!crazy_PlayerControl.Invincible())
			{
				crazy_PlayerControl.Hurt();
			}
		}
	}

	protected bool AttackJudgment(GameObject target)
	{
		return (target.transform.position - base.transform.position).sqrMagnitude <= Radius * Radius;
	}
}
