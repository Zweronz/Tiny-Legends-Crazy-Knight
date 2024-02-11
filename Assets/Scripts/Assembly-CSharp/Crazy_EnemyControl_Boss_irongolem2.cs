using UnityEngine;

public class Crazy_EnemyControl_Boss_irongolem2 : Crazy_EnemyControl_Boss
{
	protected float skill3time = 6f;

	protected float skill3lasttime;

	protected float skill3speedrate = 1.5f;

	protected GameObject skill2effect1;

	protected GameObject skill2effect2;

	private void Start()
	{
		preStart();
		ReadStatus("irongolemskill2", "irongolemstatus2");
		base.GetComponent<Animation>().wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["Idle01_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Attack03_01_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["Attack03_02_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Attack03_03_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["Forward01_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Death01_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>().Play("Idle01_merge");
		skill2effect1 = Object.Instantiate(Resources.Load("Prefabs/BossSkill/SkillEffect/SkillEffectAnimation")) as GameObject;
		skill2effect1.transform.parent = base.gameObject.transform;
		skill2effect1.transform.localPosition = Vector3.zero;
		skill2effect1.transform.localEulerAngles = new Vector3(270f, 0f, 0f);
		Crazy_PlayAnimation crazy_PlayAnimation = skill2effect1.GetComponent("Crazy_PlayAnimation") as Crazy_PlayAnimation;
		crazy_PlayAnimation.Hide();
		skill2effect2 = Object.Instantiate(Resources.Load("Prefabs/BossSkill/SkillEffect/SkillEffectP")) as GameObject;
		skill2effect2.transform.parent = base.gameObject.transform;
		skill2effect2.transform.localPosition = Vector3.zero;
		skill2effect2.transform.localEulerAngles = Vector3.zero;
		bindobject.Add(skill2effect1);
		bindobject.Add(skill2effect2);
		Appear();
	}

	protected void Appear()
	{
		Crazy_PlayTAudio crazy_PlayTAudio = base.gameObject.GetComponent("Crazy_PlayTAudio") as Crazy_PlayTAudio;
		crazy_PlayTAudio.Play();
	}

	protected override void PlayHurtEffect(Crazy_Weapon_Type type)
	{
		if (!useeffect)
		{
			return;
		}
		GameObject attackEffect = target.GetComponent<Crazy_EffectManagement>().GetAttackEffect();
		if (attackEffect != null)
		{
			attackEffect.transform.localPosition = base.transform.localPosition + new Vector3(0f, hurteffectposy, 0f);
			Crazy_ParticleSequenceScript crazy_ParticleSequenceScript = attackEffect.GetComponent("Crazy_ParticleSequenceScript") as Crazy_ParticleSequenceScript;
			if (crazy_ParticleSequenceScript != null)
			{
				crazy_ParticleSequenceScript.EmitParticle();
			}
			else if (type == Crazy_Weapon_Type.Staff)
			{
				attackEffect.GetComponentInChildren<ParticleSystem>().Play();
			}
			else
			{
				Crazy_PlayAnimation crazy_PlayAnimation = attackEffect.GetComponent("Crazy_PlayAnimation") as Crazy_PlayAnimation;
				crazy_PlayAnimation.Play();
			}
			Crazy_PlayTAudio crazy_PlayTAudio = attackEffect.GetComponent("Crazy_PlayTAudio") as Crazy_PlayTAudio;
			switch (type)
			{
			case Crazy_Weapon_Type.Sword:
				crazy_PlayTAudio.audioname = "Multi_SwordHitMetal01";
				break;
			case Crazy_Weapon_Type.Hammer:
				crazy_PlayTAudio.audioname = "Fx_HammerHitMetal";
				break;
			case Crazy_Weapon_Type.Axe:
				crazy_PlayTAudio.audioname = "Multi_AxeHitMetal01";
				break;
			case Crazy_Weapon_Type.Bow:
				crazy_PlayTAudio.audioname = "Ani_BowHit_Metal01";
				break;
			case Crazy_Weapon_Type.Staff:
				crazy_PlayTAudio.audioname = "MobaoHit01";
				break;
			}
			crazy_PlayTAudio.Play();
		}
		PlayEnchantEffect();
		PlayHurtEffectEx();
	}

	protected override void updateMove(float deltatime)
	{
		if (curStatus == Crazy_MonsterStatus.Move || curStatus == Crazy_MonsterStatus.Run)
		{
			if (!(Mathf.Abs(curMoveDir.x) > 0.001f) && !(Mathf.Abs(curMoveDir.z) > 0.001f))
			{
				return;
			}
			if (cur_use_skill == null)
			{
				if (outalertmove)
				{
					outalertmove = false;
					outalertMoveDir = new Vector3(Random.Range(-100, 100), 0f, Random.Range(-100, 100));
					outalertMoveDir.Normalize();
				}
				curMoveDir = outalertMoveDir;
				Move(new Vector3(curMoveDir.x * deltatime * speedWalk * (1f - skillmoverate) * moverate, 0f, curMoveDir.z * deltatime * speedWalk * (1f - skillmoverate) * moverate));
			}
			else
			{
				curMoveDir.Normalize();
				Move(new Vector3(curMoveDir.x * deltatime * speedRun * (1f - skillmoverate) * moverate, 0f, curMoveDir.z * deltatime * speedRun * (1f - skillmoverate) * moverate));
			}
		}
		else if (cur_use_skill != null && cur_use_skill.cur_data.id == 2 && curStatus == Crazy_MonsterStatus.Attack)
		{
			Vector2 vector = new Vector2((target.transform.position - base.transform.position).x, (target.transform.position - base.transform.position).z);
			if (vector.SqrMagnitude() != 0f)
			{
				vector.Normalize();
			}
			Vector3 vector2 = new Vector3(vector.x, 0f, vector.y);
			Move(vector2 * deltatime * speedWalk * (1f - skillmoverate) * moverate * skill3speedrate);
		}
	}

	protected void UpdateSkill3()
	{
		if (cur_use_skill != null && cur_use_skill.cur_data.id == 2 && cur_use_skill.cur_process == Crazy_Boss_Skill_Process.Use)
		{
			skill3lasttime += Time.deltaTime;
			if (skill3lasttime >= skill3time)
			{
				skill3lasttime = 0f;
				OffAttack();
				Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task29, 0, 0f);
			}
		}
	}

	public override void OnAttackJudgment()
	{
		if (!AttackJudgment())
		{
			return;
		}
		Crazy_PlayerControl crazy_PlayerControl = target.GetComponent("Crazy_PlayerControl") as Crazy_PlayerControl;
		if (!crazy_PlayerControl.Invincible())
		{
			m_hitdata.beatDir = target.transform.position - base.transform.position;
			m_hitdata.beatDir.Normalize();
			m_hitdata.beatSpeed = powerbase * powerrate;
			crazy_PlayerControl.Injury(m_hitdata);
			if (cur_use_skill != null && cur_use_skill.cur_data.id == 2)
			{
				OffAttack();
			}
		}
	}

	public void PlaySkill1Effect(string name)
	{
	}

	public void PlaySkill2Effect(int id)
	{
		if (useeffect)
		{
			switch (id)
			{
			case 1:
			{
				Crazy_PlayAnimation crazy_PlayAnimation = skill2effect1.GetComponent("Crazy_PlayAnimation") as Crazy_PlayAnimation;
				crazy_PlayAnimation.Play();
				break;
			}
			case 2:
			{
				Crazy_ParticleSequenceScript crazy_ParticleSequenceScript = skill2effect2.GetComponent("Crazy_ParticleSequenceScript") as Crazy_ParticleSequenceScript;
				crazy_ParticleSequenceScript.EmitParticle();
				break;
			}
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		UpdateSkill3();
	}

	protected override void PlayAnimation(Crazy_MonsterStatus toStatus)
	{
		switch (toStatus)
		{
		case Crazy_MonsterStatus.Idle:
			base.GetComponent<Animation>().CrossFade("Idle01_merge", 0.01f);
			break;
		case Crazy_MonsterStatus.Move:
			base.GetComponent<Animation>()["Forward01_merge"].speed = speedWalk * (1f - skillmoverate) * moverate / 5f;
			base.GetComponent<Animation>().CrossFade("Forward01_merge", 0.01f);
			break;
		case Crazy_MonsterStatus.Run:
			base.GetComponent<Animation>()["Forward01_merge"].speed = speedRun * (1f - skillmoverate) * moverate / 5f;
			base.GetComponent<Animation>().CrossFade("Forward01_merge", 0.01f);
			break;
		case Crazy_MonsterStatus.Die:
			base.GetComponent<Animation>().CrossFade("Death01_merge", 0.01f);
			break;
		case Crazy_MonsterStatus.PreAttack:
			if (cur_use_skill.cur_data.preparetime > 0.01f && base.GetComponent<Animation>()[cur_use_skill.cur_data.preanimationname + "_merge"] != null)
			{
				base.GetComponent<Animation>().CrossFade(cur_use_skill.cur_data.preanimationname + "_merge", 0.01f);
			}
			break;
		case Crazy_MonsterStatus.Attack:
			if (base.GetComponent<Animation>()[cur_use_skill.cur_data.useanimationname + "_merge"] != null)
			{
				base.GetComponent<Animation>().CrossFade(cur_use_skill.cur_data.useanimationname + "_merge", 0.01f);
			}
			break;
		case Crazy_MonsterStatus.EndAttack:
			if (cur_use_skill.cur_data.endtime > 0.01f && base.GetComponent<Animation>()[cur_use_skill.cur_data.endanimationname + "_merge"] != null)
			{
				base.GetComponent<Animation>().CrossFade(cur_use_skill.cur_data.endanimationname + "_merge", 0.01f);
			}
			break;
		case Crazy_MonsterStatus.HitRecover:
			base.GetComponent<Animation>().CrossFade("Idle01_merge", 0.01f);
			break;
		case Crazy_MonsterStatus.Hurt:
			base.GetComponent<Animation>().Play("Idle01_merge");
			break;
		case Crazy_MonsterStatus.Remote:
			break;
		}
	}

	public override void SetHurtAnimationEffect(float effect)
	{
	}

	public override Renderer[] FindMeshRenderer()
	{
		return new Renderer[1] { base.gameObject.transform.Find("body").gameObject.GetComponent<Renderer>() };
	}
}
