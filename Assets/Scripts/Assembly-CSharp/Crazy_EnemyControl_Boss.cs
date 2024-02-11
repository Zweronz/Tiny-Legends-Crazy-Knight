using System.Collections.Generic;
using UnityEngine;

public class Crazy_EnemyControl_Boss : Crazy_EnemyControl
{
	protected List<Crazy_Boss_Status> m_status = new List<Crazy_Boss_Status>();

	protected Crazy_Boss_Skill cur_use_skill;

	protected Crazy_Boss_Status cur_status;

	protected Dictionary<int, Crazy_Boss_Skill> m_skill = new Dictionary<int, Crazy_Boss_Skill>();

	protected Crazy_HitData m_hitdata;

	protected float moverate = 1f;

	protected float powerrate = 1f;

	protected float powerbase = 10f;

	protected float bloodSlotHeight = 3.5f;

	private int line_count;

	public override void InitData(Crazy_Monster_Template template, Crazy_Monster_Level level, int item, float speedmodify, float power)
	{
		InitData(template.id, template.size, template.movespeed * speedmodify, template.rotatespeed, template.hitboxsize, template.preattackrange, template.preattacktime, template.pointlength, template.pointangle, template.judgmentrange, template.judgmentangle, level.hp, level.lv, level.gold, level.exp, item, template.type);
		powerrate = power;
	}

	protected override void InitTexture(int lv)
	{
	}

	protected override void preStart()
	{
		hurteffectposy = 2.5f;
		recycletime = 5f;
		reactiontime = 15f;
		base.preStart();
		InitHitData();
	}

	protected virtual void InitHitData()
	{
		m_hitdata.beatDir = Vector3.zero;
		m_hitdata.beatSpeed = powerbase * powerrate;
		m_hitdata.beatTime = 1f / 3f;
		m_hitdata.hitrecovertime = 1.6666667f;
		m_hitdata.crazy_fightedtype = Crazy_FightedType.BeatBack;
		m_hitdata.hitrecoverPower = 1f;
	}

	protected virtual void ReadStatus(string skillpath, string statuspath)
	{
		ReadSkillXml(skillpath);
		ReadStatusXml(statuspath);
	}

	protected virtual void ReadSkillXml(string path)
	{
		m_skill = Crazy_BossSkillInfo.GetCrazyBossSkill(path);
	}

	protected virtual void ReadStatusXml(string path)
	{
		m_status = Crazy_BossStatusInfo.GetCrazyBossStatus(path, m_skill);
	}

	protected void InitBloodSlotBk(float size, GameObject slot)
	{
		GameObject gameObject = new GameObject("bloodslotbk", typeof(MeshFilter), typeof(MeshRenderer));
		gameObject.GetComponent<Renderer>().material = Resources.Load("Textures/boss_blood_slot_M") as Material;
		MeshFilter meshFilter = gameObject.GetComponent("MeshFilter") as MeshFilter;
		Vector3[] array = new Vector3[4];
		Vector2[] array2 = new Vector2[4];
		int[] array3 = new int[6];
		array[0] = new Vector3(0.024f * size, 0.027f * size, -0.01f);
		array[1] = new Vector3(-0.63f * size, 0.027f * size, -0.01f);
		array[2] = new Vector3(0.024f * size, -0.069f * size, -0.01f);
		array[3] = new Vector3(-0.63f * size, -0.069f * size, -0.01f);
		array2[0] = new Vector2(0f, 1f);
		array2[1] = new Vector2(1f, 1f);
		array2[2] = new Vector2(0f, 0f);
		array2[3] = new Vector2(1f, 0f);
		array3[0] = 0;
		array3[1] = 2;
		array3[2] = 1;
		array3[3] = 1;
		array3[4] = 2;
		array3[5] = 3;
		meshFilter.mesh.vertices = array;
		meshFilter.mesh.uv = array2;
		meshFilter.mesh.triangles = array3;
		gameObject.transform.parent = slot.transform.parent;
		gameObject.transform.localPosition = new Vector3(1.515f, 0f, 0f);
	}

	protected override void InitBloodSlot(float size)
	{
		GameObject gameObject = new GameObject("bloodslotparent");
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = new Vector3(0f, bloodSlotHeight, 0f);
		bloodSlotObject = new GameObject("bloodslot", typeof(MeshFilter), typeof(MeshRenderer));
		bloodSlotObject.GetComponent<Renderer>().material = Resources.Load("Textures/boss_blood_M") as Material;
		MeshFilter meshFilter = bloodSlotObject.GetComponent("MeshFilter") as MeshFilter;
		Vector3[] array = new Vector3[4];
		Vector2[] array2 = new Vector2[4];
		int[] array3 = new int[6];
		array[0] = new Vector3(0f, 0f, 0f);
		array[1] = new Vector3(-0.606f * size, 0f, 0f);
		array[2] = new Vector3(0f, -0.042f * size, 0f);
		array[3] = new Vector3(-0.606f * size, -0.042f * size, 0f);
		array2[0] = new Vector2(0f, 1f);
		array2[1] = new Vector2(1f, 1f);
		array2[2] = new Vector2(0f, 0f);
		array2[3] = new Vector2(1f, 0f);
		array3[0] = 0;
		array3[1] = 2;
		array3[2] = 1;
		array3[3] = 1;
		array3[4] = 2;
		array3[5] = 3;
		meshFilter.mesh.vertices = array;
		meshFilter.mesh.uv = array2;
		meshFilter.mesh.triangles = array3;
		bloodSlotObject.transform.parent = gameObject.transform;
		bloodSlotObject.transform.localPosition = new Vector3(1.515f, 0f, 0f);
		InitBloodSlotBk(size, bloodSlotObject);
	}

	protected override void updateBloodSlot()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
		Vector3 vector = gameObject.transform.position - bloodSlotObject.transform.parent.position;
		vector.y = 0f;
		bloodSlotObject.transform.parent.forward = new Vector3(0f, 0f, 1f);
		bloodSlotObject.transform.localScale = new Vector3(GetHPPercent(), 1f, 1f);
	}

	protected virtual void updateStatus()
	{
		foreach (Crazy_Boss_Status item in m_status)
		{
			if (IsInStatus(item) && (cur_status == null || cur_status.priority > item.priority))
			{
				cur_status = item;
				updateStatusData();
			}
		}
	}

	protected void updateStatusData()
	{
		if (cur_status == null)
		{
			return;
		}
		moverate = cur_status.data.moverate;
		if (cur_use_skill != null)
		{
			preattacktime = cur_use_skill.cur_data.preparetime;
			endattacktime = cur_use_skill.cur_data.endtime;
			if (cur_use_skill.cur_data.id == 0 && base.GetComponent<Animation>()[cur_use_skill.cur_data.useanimationname + "_merge"] != null)
			{
				base.GetComponent<Animation>()[cur_use_skill.cur_data.useanimationname + "_merge"].speed = 1f * cur_status.data.preattacktimerate / (1f + skillattackrate);
			}
		}
		cur_status.updateSkillPriority();
		if (cur_status.priority == 0)
		{
			Crazy_SceneManager.GetInstance().GetScene().SendHintMessage("#BADASS BOSS MODE$");
		}
	}

	protected bool IsInStatus(Crazy_Boss_Status status)
	{
		switch (status.condition)
		{
		case Crazy_Boss_Status_ConditionType.HP:
			return GetHPPercent() <= status.conditionparam;
		case Crazy_Boss_Status_ConditionType.Time:
			return GetActiveTime() >= status.conditionparam;
		default:
			return false;
		}
	}

	protected void updateSkillCoolDown(float deltatime)
	{
		if (cur_status == null)
		{
			return;
		}
		foreach (Crazy_Boss_Skill key in cur_status.data.m_skill.Keys)
		{
			if (key.cur_status == Crazy_Boss_Skill_Status.CoolDown)
			{
				key.lastcooldowntime += deltatime;
				if (key.lastcooldowntime >= key.cur_data.cooldowntime)
				{
					key.lastcooldowntime = 0f;
					key.cur_status = Crazy_Boss_Skill_Status.Active;
				}
			}
		}
	}

	protected virtual void updateSkill()
	{
		if (cur_use_skill == null)
		{
			Crazy_Boss_Skill prioritySkill = cur_status.GetPrioritySkill();
			if (prioritySkill != null)
			{
				OnSkill(prioritySkill);
			}
		}
		else if (cur_use_skill.cur_process == Crazy_Boss_Skill_Process.Begin && cur_use_skill.cur_data.isDelay)
		{
			Crazy_Boss_Skill prioritySkill2 = cur_status.GetPrioritySkill();
			if (prioritySkill2 != null && cur_use_skill.cur_data.priority > prioritySkill2.cur_data.priority)
			{
				OnSkill(prioritySkill2);
			}
		}
	}

	protected virtual void OnSkill(Crazy_Boss_Skill skill)
	{
		if (cur_use_skill != null)
		{
			cur_use_skill.cur_process = Crazy_Boss_Skill_Process.NotUse;
		}
		skill.cur_process = Crazy_Boss_Skill_Process.Begin;
		cur_use_skill = skill;
		updateSkillData();
	}

	protected void updateSkillData()
	{
		attackrange = cur_use_skill.cur_data.begintopreparerange;
		preattacktime = cur_use_skill.cur_data.preparetime;
		endattacktime = cur_use_skill.cur_data.endtime;
		if (cur_status != null && cur_use_skill.cur_data.id == 0 && base.GetComponent<Animation>()[cur_use_skill.cur_data.useanimationname + "_merge"] != null)
		{
			base.GetComponent<Animation>()[cur_use_skill.cur_data.useanimationname + "_merge"].speed = 1f * cur_status.data.preattacktimerate / (1f + skillattackrate);
		}
		cur_use_skill.cur_data.seed = Random.Range(0f, 100f);
	}

	protected override void updateData(float deltatime)
	{
		updateStatus();
		updateSkillCoolDown(deltatime);
		updateSkill();
		if (reftarget == null)
		{
			return;
		}
		curMoveDir = new Vector3(reftarget.transform.position.x - base.transform.position.x, 0f, reftarget.transform.position.z - base.transform.position.z);
		curForwardDir = new Vector3(reftarget.transform.position.x - base.transform.position.x, 0f, reftarget.transform.position.z - base.transform.position.z);
		if (IsinAlertRange(reftarget))
		{
			isalert = true;
		}
		if (curStatus == Crazy_MonsterStatus.Attack)
		{
			return;
		}
		if (cur_use_skill != null && IsinAttackRange(reftarget))
		{
			switchMonsterStatus(Crazy_MonsterStatus.PreAttack);
		}
		else if (cur_use_skill != null && isalert)
		{
			switchMonsterStatus(Crazy_MonsterStatus.Run);
		}
		else if (!isalert)
		{
			if (curStatus == Crazy_MonsterStatus.Move || curStatus == Crazy_MonsterStatus.Run)
			{
				float num = Random.Range(0f, 1f);
				if (num >= 0.95f)
				{
					switchMonsterStatus(Crazy_MonsterStatus.Idle);
				}
			}
			else if (curStatus == Crazy_MonsterStatus.Idle)
			{
				float num2 = Random.Range(0f, 1f);
				if (num2 >= 0.8f)
				{
					outalertmove = true;
					switchMonsterStatus(Crazy_MonsterStatus.Move);
				}
			}
		}
		else if (cur_use_skill == null && (curStatus == Crazy_MonsterStatus.Move || curStatus == Crazy_MonsterStatus.Run))
		{
			switchMonsterStatus(Crazy_MonsterStatus.Idle);
		}
	}

	public override bool Hurt(float damage, Crazy_HitData chd, Crazy_Weapon_Type type, bool isskill)
	{
		if (curHp <= damage)
		{
			if (isskill)
			{
				Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task28, 0, 0f);
			}
			curHp = 0f;
			Crazy_FightedType crazy_fightedtype = chd.crazy_fightedtype;
			if (crazy_fightedtype == Crazy_FightedType.BeatBack)
			{
				PlayHurtEffect(type);
				Dying();
				switchMonsterStatus(Crazy_MonsterStatus.Die);
			}
			return true;
		}
		PlayHurtEffect(type);
		curHp -= damage;
		OnInvincible(0.1f);
		return false;
	}

	protected void OffSkill()
	{
		if (cur_use_skill != null)
		{
			cur_use_skill.cur_process = Crazy_Boss_Skill_Process.NotUse;
			cur_use_skill.cur_status = Crazy_Boss_Skill_Status.CoolDown;
			cur_use_skill.lastcooldowntime = 0f;
			cur_use_skill = null;
		}
	}

	protected override void updateEndAttack(float deltatime)
	{
		endattacklasttime += deltatime;
		if (endattacklasttime > endattacktime)
		{
			switchMonsterStatus(Crazy_MonsterStatus.Idle);
			updateTurnRound();
			OffSkill();
		}
	}

	protected override void switchMonsterStatus(Crazy_MonsterStatus toStatus)
	{
		if ((toStatus == Crazy_MonsterStatus.Idle || toStatus == Crazy_MonsterStatus.HitRecover || curStatus <= toStatus) && curStatus != toStatus)
		{
			ResetPreAttack(0f);
			switch (toStatus)
			{
			case Crazy_MonsterStatus.PreAttack:
				cur_use_skill.cur_process = Crazy_Boss_Skill_Process.Prepare;
				updateTurnRound();
				break;
			case Crazy_MonsterStatus.Attack:
				cur_use_skill.cur_process = Crazy_Boss_Skill_Process.Use;
				OnAttack();
				break;
			case Crazy_MonsterStatus.EndAttack:
				cur_use_skill.cur_process = Crazy_Boss_Skill_Process.End;
				break;
			case Crazy_MonsterStatus.Die:
				Die();
				break;
			}
			PlayAnimation(toStatus);
			curStatus = toStatus;
		}
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
				m_hitdata.beatSpeed = powerbase * powerrate;
				crazy_PlayerControl.Injury(m_hitdata);
			}
		}
	}

	protected override bool IsPreAttack()
	{
		return preattacklasttime < preattacktime;
	}

	protected override void OnAttack()
	{
		base.OnAttack();
		if (cur_use_skill == null)
		{
			return;
		}
		Vector2 original = new Vector2(base.transform.position.x, base.transform.position.z);
		Vector2 forward = new Vector2(base.transform.forward.x, base.transform.forward.z);
		ResetLine();
		foreach (Crazy_SkillPoint item in cur_use_skill.cur_data.damagejudgment)
		{
			Vector2 original2 = Crazy_Global.RotatebyAngle(original, forward, item.angle, item.length);
			Vector3 vector = new Vector3(original2.x, 0f, original2.y);
			DrawLine(base.transform.position, vector, new Color(1f, 0f, 0f, 0.2f));
			Vector2 forward2 = Crazy_Global.Rotate(forward, item.fixangle);
			for (float num = 0f; num <= item.rangeangle; num += 1f)
			{
				Vector2 vector2 = Crazy_Global.RotatebyAngle(original2, forward2, num, item.rangelength);
				Vector3 end = new Vector3(vector2.x, 0f, vector2.y);
				Vector2 vector3 = Crazy_Global.RotatebyAngle(original2, forward2, 0f - num, item.rangelength);
				Vector3 end2 = new Vector3(vector3.x, 0f, vector3.y);
				DrawLine(vector, end, new Color(1f, 0f, 0f, 0.2f));
				DrawLine(vector, end2, new Color(1f, 0f, 0f, 0.2f));
			}
		}
	}

	public override void SetHint(bool hint)
	{
		base.SetHint(hint);
		ResetLine();
	}

	public void DrawLine(Vector3 start, Vector3 end, Color color)
	{
		if (usehint)
		{
			LineRenderer lineRenderer = base.gameObject.GetComponent("LineRenderer") as LineRenderer;
			if (lineRenderer == null)
			{
				lineRenderer = base.gameObject.AddComponent<LineRenderer>() as LineRenderer;
				lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
			}
			lineRenderer.SetWidth(0.1f, 0.1f);
			lineRenderer.SetColors(color, color);
			line_count += 2;
			lineRenderer.SetVertexCount(line_count);
			lineRenderer.SetPosition(line_count - 2, start);
			lineRenderer.SetPosition(line_count - 1, end);
		}
	}

	public void ResetLine()
	{
		LineRenderer lineRenderer = base.gameObject.GetComponent("LineRenderer") as LineRenderer;
		if (lineRenderer != null)
		{
			line_count = 0;
			lineRenderer.SetVertexCount(line_count);
		}
	}

	protected override bool AttackJudgment()
	{
		if (cur_use_skill == null)
		{
			return false;
		}
		Vector2 original = new Vector2(base.transform.position.x, base.transform.position.z);
		Vector2 forward = new Vector2(base.transform.forward.x, base.transform.forward.z);
		foreach (Crazy_SkillPoint item in cur_use_skill.cur_data.damagejudgment)
		{
			Vector2 vector = Crazy_Global.RotatebyAngle(original, forward, item.angle, item.length);
			Vector3 vector2 = new Vector3(vector.x, 0f, vector.y);
			Vector3 vector3 = vector2 - target.transform.position;
			float sqrMagnitude = vector3.sqrMagnitude;
			if (sqrMagnitude < item.rangelength * item.rangelength)
			{
				Vector2 vector4 = Crazy_Global.Rotate(forward, item.fixangle);
				float num = Vector3.Angle(-vector3, new Vector3(vector4.x, 0f, vector4.y));
				if (num < item.rangeangle)
				{
					return true;
				}
			}
		}
		return false;
	}

	protected override void Die()
	{
		dyingtime = 0f;
		Crazy_PlayerControl crazy_PlayerControl = target.GetComponent("Crazy_PlayerControl") as Crazy_PlayerControl;
		crazy_PlayerControl.AddGold(m_gold);
		crazy_PlayerControl.AddExp(m_exp);
		Crazy_GlobalData.cur_kill_number++;
		Crazy_Statistics.AddMonsterKillNumber(Crazy_MonsterType.Boss, m_id);
		Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task06, m_type - 1, 0f);
		if (crazy_PlayerControl.GetCurHealth() == 1)
		{
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task18, 0, 0f);
		}
		if (cur_status != null && cur_status.priority == 0)
		{
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task19, 0, 0f);
		}
		bool flag = false;
		foreach (GameObject value in Crazy_GlobalData.enemyList.Values)
		{
			if (value != base.gameObject && !value.GetComponent<Crazy_EnemyControl>().IsDie())
			{
				flag = true;
			}
		}
		if (flag)
		{
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task27, 0, 0f);
		}
	}
}
