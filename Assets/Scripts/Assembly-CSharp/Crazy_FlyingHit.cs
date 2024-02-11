using UnityEngine;

public class Crazy_FlyingHit : MonoBehaviour
{
	public float radius;

	protected GameObject target;

	private void Start()
	{
	}

	public void SetTarget(GameObject tar)
	{
		target = tar;
	}

	public void OnAttackJudgment()
	{
		if (AttackJudgment())
		{
			Crazy_PlayerControl crazy_PlayerControl = target.GetComponent("Crazy_PlayerControl") as Crazy_PlayerControl;
			if (!crazy_PlayerControl.Invincible())
			{
				crazy_PlayerControl.Hurt();
			}
		}
	}

	protected bool AttackJudgment()
	{
		return (target.transform.position - base.transform.position).sqrMagnitude <= radius * radius;
	}

	private void Update()
	{
	}
}
