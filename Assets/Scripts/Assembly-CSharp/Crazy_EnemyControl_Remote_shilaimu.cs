using UnityEngine;

public class Crazy_EnemyControl_Remote_shilaimu : Crazy_EnemyControl_Remote
{
	protected GameObject deatheffect;

	public float dieexploderadius = 3f;

	private void Start()
	{
		preStart();
		base.GetComponent<Animation>().wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["idle01_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["forward01_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["death01_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["damage01_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["damage02_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["stun01_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().Play("idle01_merge");
		deatheffect = Object.Instantiate(Resources.Load("Prefabs/silme/silmedeath/silmedeath_pfb")) as GameObject;
		deatheffect.transform.parent = base.transform;
		deatheffect.transform.localPosition = Vector3.zero;
	}

	protected void DieExplode()
	{
		if (deatheffect == null)
		{
			deatheffect = Object.Instantiate(Resources.Load("Prefabs/silme/silmedeath/silmedeath_pfb")) as GameObject;
			deatheffect.transform.parent = base.transform;
			deatheffect.transform.localPosition = Vector3.zero;
		}
		deatheffect.GetComponent<Crazy_ParticleSystemSequenceScript>().EmitParticle();
		Invoke("OnDieExplodeJudgment", 0.9f);
	}

	protected void OnDieExplodeJudgment()
	{
		if (DieExplodeJudgment())
		{
			Crazy_PlayerControl crazy_PlayerControl = target.GetComponent("Crazy_PlayerControl") as Crazy_PlayerControl;
			if (!crazy_PlayerControl.Invincible())
			{
				crazy_PlayerControl.Hurt();
			}
		}
	}

	protected bool DieExplodeJudgment()
	{
		return (target.transform.position - base.transform.position).sqrMagnitude <= dieexploderadius * dieexploderadius;
	}

	protected override void PlayAnimation(Crazy_MonsterStatus toStatus)
	{
		Crazy_PlayerControl crazy_PlayerControl = target.GetComponent("Crazy_PlayerControl") as Crazy_PlayerControl;
		switch (toStatus)
		{
		case Crazy_MonsterStatus.Idle:
			base.GetComponent<Animation>().CrossFade("idle01_merge", 0.1f);
			break;
		case Crazy_MonsterStatus.Move:
			base.GetComponent<Animation>()["forward01_merge"].speed = speedWalk * (1f - skillmoverate) / 3f;
			base.GetComponent<Animation>().CrossFade("forward01_merge", 0.1f);
			break;
		case Crazy_MonsterStatus.Run:
			base.GetComponent<Animation>()["forward01_merge"].speed = speedRun * (1f - skillmoverate) / 3f;
			base.GetComponent<Animation>().CrossFade("forward01_merge", 0.1f);
			break;
		case Crazy_MonsterStatus.Die:
			base.GetComponent<Animation>().Stop();
			base.GetComponent<Animation>().CrossFade("death01_merge", 0.1f);
			DieExplode();
			break;
		case Crazy_MonsterStatus.PreAttack:
			base.GetComponent<Animation>().CrossFade("idle01_merge", 0.1f);
			break;
		case Crazy_MonsterStatus.Attack:
			base.GetComponent<Animation>().CrossFade("attack0" + 1 + "_merge", 0.1f);
			break;
		case Crazy_MonsterStatus.Remote:
			base.GetComponent<Animation>().CrossFade("attack0" + 2 + "_merge", 0.1f);
			break;
		case Crazy_MonsterStatus.HitRecover:
			base.GetComponent<Animation>().CrossFade("stun01_merge", 0.1f);
			break;
		case Crazy_MonsterStatus.Hurt:
		{
			base.GetComponent<Animation>().Stop();
			int num = Random.Range(1, 3);
			base.GetComponent<Animation>().Play("damage0" + num + "_merge");
			break;
		}
		case Crazy_MonsterStatus.EndAttack:
			break;
		}
	}

	public override void SetHurtAnimationEffect(float effect)
	{
		base.GetComponent<Animation>()["death01_merge"].speed = effect;
		base.GetComponent<Animation>()["damage01_merge"].speed = effect;
		base.GetComponent<Animation>()["damage02_merge"].speed = effect;
		base.GetComponent<Animation>()["stun01_merge"].speed = effect;
	}

	public override Renderer[] FindMeshRenderer()
	{
		return new Renderer[1] { base.gameObject.transform.Find("body").gameObject.GetComponent<Renderer>() };
	}

	protected override GameObject[] FindHeadNode()
	{
		return null;
	}
}
