using UnityEngine;

public class Crazy_EnemyControl_Boss_guaiwu : Crazy_EnemyControl_MiddleBoss
{
	private void Start()
	{
		preStart();
		base.GetComponent<Animation>().wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["idle01_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["forward01_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["adeath01_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["adeath02_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["bdeath01_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["bdeath02_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["cdeath01_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["damage01_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["damage02_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["stun01_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().Play("idle01_merge");
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
			if (crazy_PlayerControl.UseWeaponType() == Crazy_Weapon_Type.Hammer)
			{
				switch (crazy_PlayerControl.UseAttackType())
				{
				case Crazy_AttackType.Attack01:
				case Crazy_AttackType.Attack02:
				case Crazy_AttackType.Attack03:
					base.GetComponent<Animation>().CrossFade("bdeath02_merge", 0.1f);
					DieAway();
					break;
				case Crazy_AttackType.Attack04:
				case Crazy_AttackType.Skill:
					if ((crazy_PlayerControl.UseExplodePosition() - base.transform.position).magnitude + GetHitBox() <= 3f)
					{
						base.GetComponent<Animation>().CrossFade("bdeath01_merge", 0.1f);
						break;
					}
					base.GetComponent<Animation>().CrossFade("bdeath02_merge", 0.1f);
					DieAway();
					break;
				}
			}
			else if (crazy_PlayerControl.UseAttackType() == Crazy_AttackType.Skill)
			{
				int num3 = Random.Range(1, 3);
				base.GetComponent<Animation>().CrossFade("adeath0" + num3 + "_merge", 0.1f);
				DieAway3();
			}
			else if (crazy_PlayerControl.UseWeaponType() == Crazy_Weapon_Type.Bow)
			{
				base.GetComponent<Animation>().CrossFade("bdeath02_merge", 0.1f);
				DieAway4();
			}
			else
			{
				base.GetComponent<Animation>().CrossFade("cdeath0" + 1 + "_merge", 0.1f);
				DieAway2();
			}
			break;
		case Crazy_MonsterStatus.PreAttack:
			base.GetComponent<Animation>().CrossFade("idle01_merge", 0.1f);
			break;
		case Crazy_MonsterStatus.Attack:
		{
			int num2 = Random.Range(1, 3);
			base.GetComponent<Animation>().CrossFade("attack0" + num2 + "_merge", 0.1f);
			break;
		}
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
		case Crazy_MonsterStatus.Remote:
			break;
		}
	}

	public override void SetHurtAnimationEffect(float effect)
	{
		base.GetComponent<Animation>()["adeath01_merge"].speed = effect;
		base.GetComponent<Animation>()["adeath02_merge"].speed = effect;
		base.GetComponent<Animation>()["bdeath01_merge"].speed = effect;
		base.GetComponent<Animation>()["bdeath02_merge"].speed = effect;
		base.GetComponent<Animation>()["cdeath01_merge"].speed = effect;
		base.GetComponent<Animation>()["damage01_merge"].speed = effect;
		base.GetComponent<Animation>()["damage02_merge"].speed = effect;
		base.GetComponent<Animation>()["stun01_merge"].speed = effect;
	}

	public override Renderer[] FindMeshRenderer()
	{
		return new Renderer[1] { base.gameObject.transform.Find("monster").gameObject.GetComponent<Renderer>() };
	}

	protected override GameObject[] FindHeadNode()
	{
		return new GameObject[2]
		{
			base.gameObject.transform.Find("Bone01/Dummy02/Dummy01").gameObject,
			base.gameObject.transform.Find("Bone01/Dummy02/Dummy03").gameObject
		};
	}
}
