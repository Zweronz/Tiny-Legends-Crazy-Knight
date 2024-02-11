using UnityEngine;

public class Crazy_EnemyControl_Boss_lizard_Net : Crazy_EnemyControl_Boss_Net
{
	protected float objectsamplerate = 1f / 30f;

	private bool isappearend;

	protected override void preStart()
	{
		base.preStart();
		hurteffectposy = 1.2f;
	}

	private void Start()
	{
		preStart();
		ReadStatus("lizard_netskill", "lizard_netstatus");
		base.GetComponent<Animation>().wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["standby_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["attack1_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["skill_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["move_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["dead_merge"].wrapMode = WrapMode.ClampForever;
		AddAnimationEvent();
		Appear();
	}

	protected override void InitBloodSlot(float size)
	{
		bloodSlotHeight = 2f;
		base.InitBloodSlot(size);
	}

	protected void EventAnimation(AnimationClip ani, int frame, string functionname, int intP = 0, float floatP = 0f, string stringP = "", Object objectP = null)
	{
		Crazy_Global.EventAnimation(ani, (float)frame * objectsamplerate, functionname, intP, floatP, stringP, objectP);
	}

	protected void AddAnimationEvent()
	{
		EventAnimation(base.GetComponent<Animation>()["up_merge"].clip, 98, "AppearEnd", 0, 0f, string.Empty);
		EventAnimation(base.GetComponent<Animation>()["attack1_merge"].clip, 25, "OffAttack", 0, 0f, string.Empty);
		EventAnimation(base.GetComponent<Animation>()["skill_merge"].clip, 18, "OffAttack", 0, 0f, string.Empty);
	}

	public void AppearEnd()
	{
		isappearend = true;
		base.GetComponent<Animation>().Play("standby_merge");
	}

	protected void Appear()
	{
		isappearend = false;
		base.GetComponent<Animation>().Play("up_merge");
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
		if ((curStatus != Crazy_MonsterStatus.Move && curStatus != Crazy_MonsterStatus.Run) || (!(Mathf.Abs(curMoveDir.x) > 0.001f) && !(Mathf.Abs(curMoveDir.z) > 0.001f)))
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

	public override void OnAttackJudgment()
	{
		if (AttackJudgment())
		{
			Crazy_PlayerControl crazy_PlayerControl = target.GetComponent("Crazy_PlayerControl") as Crazy_PlayerControl;
			if (!crazy_PlayerControl.Invincible())
			{
				m_hitdata.beatDir = target.transform.position - base.transform.position;
				m_hitdata.beatDir.Normalize();
				m_hitdata.beatSpeed = powerbase * powerrate;
				crazy_PlayerControl.Injury(m_hitdata);
			}
		}
	}

	protected override void Die()
	{
		base.Die();
		foreach (GameObject item in bindobject)
		{
			item.SendMessage("Stop", SendMessageOptions.DontRequireReceiver);
		}
	}

	protected override void Update()
	{
		if (!isappearend)
		{
			updateDrop();
			updateinvincible();
			updateBloodSlot();
		}
		else
		{
			base.Update();
		}
	}

	protected override void PlayAnimation(Crazy_MonsterStatus toStatus)
	{
		switch (toStatus)
		{
		case Crazy_MonsterStatus.Idle:
			base.GetComponent<Animation>().CrossFade("standby_merge", 0.01f);
			break;
		case Crazy_MonsterStatus.Move:
			base.GetComponent<Animation>()["move_merge"].speed = speedWalk * (1f - skillmoverate) * moverate / 5f;
			base.GetComponent<Animation>().CrossFade("move_merge", 0.01f);
			break;
		case Crazy_MonsterStatus.Run:
			base.GetComponent<Animation>()["move_merge"].speed = speedRun * (1f - skillmoverate) * moverate / 5f;
			base.GetComponent<Animation>().CrossFade("move_merge", 0.01f);
			break;
		case Crazy_MonsterStatus.Die:
			base.GetComponent<Animation>().CrossFade("dead_merge", 0.01f);
			break;
		case Crazy_MonsterStatus.PreAttack:
			if (cur_use_skill != null && cur_use_skill.cur_data.preparetime > 0.01f && base.GetComponent<Animation>()[cur_use_skill.cur_data.preanimationname + "_merge"] != null)
			{
				base.GetComponent<Animation>().CrossFade(cur_use_skill.cur_data.preanimationname + "_merge", 0.01f);
			}
			break;
		case Crazy_MonsterStatus.Attack:
			if (cur_use_skill != null && base.GetComponent<Animation>()[cur_use_skill.cur_data.useanimationname + "_merge"] != null)
			{
				base.GetComponent<Animation>().CrossFade(cur_use_skill.cur_data.useanimationname + "_merge", 0.01f);
			}
			break;
		case Crazy_MonsterStatus.EndAttack:
			if (cur_use_skill != null && cur_use_skill.cur_data.endtime > 0.01f && base.GetComponent<Animation>()[cur_use_skill.cur_data.endanimationname + "_merge"] != null)
			{
				base.GetComponent<Animation>().CrossFade(cur_use_skill.cur_data.endanimationname + "_merge", 0.01f);
			}
			break;
		case Crazy_MonsterStatus.HitRecover:
			base.GetComponent<Animation>().CrossFade("standby_merge", 0.01f);
			break;
		case Crazy_MonsterStatus.Hurt:
			base.GetComponent<Animation>().Play("standby_merge");
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
