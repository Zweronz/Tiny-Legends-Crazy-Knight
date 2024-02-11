using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TAudioController))]
public class Crazy_EnemyControl : Crazy_ObjectControl
{
	protected Renderer[] meshrenderer;

	private Crazy_HitRecover hitrecover;

	protected GameObject target;

	protected GameObject reftarget;

	protected GameObject shadowObject;

	protected GameObject bloodObject;

	protected GameObject bloodSlotObject;

	protected Crazy_MonsterStatus curStatus = Crazy_MonsterStatus.Idle;

	protected float dyingtime;

	protected float speedWalk = 1f;

	protected float speedRun = 2f;

	protected float speedAround = 0.5f;

	protected Vector3 curMoveDir;

	protected Vector3 curForwardDir;

	protected Vector3 attackForward;

	protected float attackrange = 2.5f;

	protected float attackintervaltime = 1f;

	protected float lastattacktime = 10f;

	protected float preattacktime = 1f;

	protected float preattacklasttime;

	protected float endattacktime;

	protected float endattacklasttime;

	protected float MaxHp = 10f;

	protected float curHp = 1f;

	protected float attackjudgmentrange = 2.75f;

	protected Vector3 attackjudgmentpoint;

	protected float attackjudgmentangle = 30f;

	protected float pointlength;

	protected float pointangle;

	protected float rotateangle = 40f;

	protected float lasthitrecovertime;

	protected float beatspeed;

	protected float alertrange = 50f;

	protected bool isalert;

	protected float lastalerttime;

	protected float alerttime = 10f;

	protected float alertrecovertime = 2f;

	protected Vector3 outalertMoveDir = new Vector3(1f, 0f, 0f);

	protected bool outalertmove;

	protected float bodytype = 1f;

	protected float _interval_update;

	protected float _Interval_Update = 0.2f;

	protected Vector3 hurt_vec3;

	protected float hitbox = 0.7f;

	protected float keepToWalls = 1f;

	protected float keepToPlayers = 0.5f;

	protected bool btouchground;

	protected float lastinvincibletime;

	protected bool invincible;

	protected float invincibletime;

	protected Dictionary<Material, Color> mat_color = new Dictionary<Material, Color>();

	protected GameObject[] headg;

	protected float last_pause_time;

	protected bool is_pause;

	protected float pause_time;

	protected Crazy_MonsterDeathType dtype;

	protected GameObject RootNode;

	protected bool useeffect = true;

	protected bool usehint;

	protected float recycletime = 2f;

	protected int m_lv;

	protected int m_exp;

	protected int m_gold = -1;

	protected int m_item = -1;

	protected int m_id;

	protected int m_type;

	protected float skillmoverate;

	protected float skillattackrate;

	protected float activetime;

	protected Crazy_Point prange;

	protected float rangeout = 30f;

	protected float hurteffectposy = 1.5f;

	protected List<GameObject> bindobject = new List<GameObject>();

	protected float lastreactiontime;

	protected float reactiontime = 5f;

	protected bool hurt_active;

	protected Crazy_SkillPoint sp = new Crazy_SkillPoint();

	public void SetRange(Crazy_Point range, float rout)
	{
		prange = range;
		rangeout = rout;
	}

	public virtual void InitData(Crazy_Monster_Template template, Crazy_Monster_Level level, int item, float speedmodify, float preattacktimemodify)
	{
		InitData(template.id, template.size, template.movespeed * speedmodify, template.rotatespeed, template.hitboxsize, template.preattackrange, template.preattacktime * preattacktimemodify, template.pointlength, template.pointangle, template.judgmentrange, template.judgmentangle, level.hp * template.hpmodify, level.lv, level.gold, level.exp, item, template.type);
	}

	public virtual void InitData(Crazy_Monster_Template template, Crazy_Monster_Level level, int item)
	{
		InitData(template.id, template.size, template.movespeed, template.rotatespeed, template.hitboxsize, template.preattackrange, template.preattacktime, template.pointlength, template.pointangle, template.judgmentrange, template.judgmentangle, level.hp * template.hpmodify, level.lv, level.gold, level.exp, item, template.type);
	}

	public virtual void InitData(int id, float size, float movespeed, float rotatespeed, float hitboxsize, float pattackrange, float pattacktime, float aipl, float aipa, float ajr, float aja, float hp, int lv, int gold, int exp, int item, int type)
	{
		m_id = id;
		bodytype = base.gameObject.transform.localScale.x;
		alertrange *= size;
		speedWalk *= movespeed;
		speedRun *= movespeed;
		rotateangle *= rotatespeed;
		hitbox = hitboxsize;
		attackrange = pattackrange;
		preattacktime = pattacktime;
		pointlength = aipl;
		pointangle = aipa;
		attackjudgmentrange = ajr;
		attackjudgmentangle = aja;
		sp.length = pointlength;
		sp.angle = pointangle;
		sp.rangelength = attackjudgmentrange;
		sp.rangeangle = attackjudgmentangle;
		MaxHp = hp;
		curHp = MaxHp;
		m_lv = lv;
		m_gold = gold;
		m_exp = exp;
		m_item = item;
		m_type = type;
		InitShadow(2f * (bodytype + 1f));
		InitBlood(2f * (bodytype + 1f));
		InitBloodSlot(2f * (bodytype + 1f));
		curHp = MaxHp;
		hitrecover = new Crazy_HitRecover();
		hitrecover.InitializeEff(0f, 0f, 1f, 1f, 0f);
		meshrenderer = FindMeshRenderer();
		InitModelColor();
		InitTexture(m_lv);
		headg = FindHeadNode();
		hurt_active = true;
	}

	public bool GetActive()
	{
		return hurt_active;
	}

	protected virtual void InitTexture(int lv)
	{
		Renderer[] array = meshrenderer;
		foreach (Renderer renderer in array)
		{
			Material[] materials = renderer.materials;
			for (int j = 0; j < materials.Length; j++)
			{
				string text = materials[j].mainTexture.name;
				if (lv < 20 && lv >= 8)
				{
					Material material = Resources.Load("Textures/Monster/" + text + "_01_M") as Material;
					materials[j] = material;
				}
				else if (lv >= 20)
				{
					Material material2 = Resources.Load("Textures/Monster/" + text + "_02_M") as Material;
					materials[j] = material2;
				}
			}
			renderer.materials = materials;
		}
	}

	protected virtual void InitBloodSlot(float size)
	{
	}

	protected virtual void updateBloodSlot()
	{
	}

	public virtual void SetWeaponSkillEffect(float move, float speed)
	{
		skillmoverate = move;
		skillattackrate = speed;
	}

	public float GetHPPercent()
	{
		return curHp / MaxHp;
	}

	public float GetActiveTime()
	{
		return activetime;
	}

	protected virtual GameObject[] FindHeadNode()
	{
		return null;
	}

	protected void InitModelColor()
	{
		Renderer[] array = meshrenderer;
		foreach (Renderer renderer in array)
		{
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				mat_color.Add(material, material.color);
			}
		}
	}

	protected virtual void Awake()
	{
		target = GameObject.Find("Player");
		reftarget = target;
		RootNode = GameObject.Find("Scene");
	}

	protected virtual void preStart()
	{
	}

	public void SetEffect(bool effect)
	{
		useeffect = effect;
	}

	public virtual void SetHint(bool hint)
	{
		usehint = hint;
	}

	protected void InitShadow(float size)
	{
		shadowObject = new GameObject("shadow", typeof(MeshFilter), typeof(MeshRenderer));
		shadowObject.GetComponent<Renderer>().material = Resources.Load("Textures/character_shadow_M") as Material;
		MeshFilter meshFilter = shadowObject.GetComponent("MeshFilter") as MeshFilter;
		Vector3[] array = new Vector3[4];
		Vector2[] array2 = new Vector2[4];
		int[] array3 = new int[6];
		array[0] = new Vector3(0.5f * size, 0.1f, -0.3f * size);
		array[1] = new Vector3(0.5f * size, 0.1f, 0.3f * size);
		array[2] = new Vector3(-0.5f * size, 0.1f, -0.3f * size);
		array[3] = new Vector3(-0.5f * size, 0.1f, 0.3f * size);
		array2[0] = new Vector2(0f, 1f);
		array2[1] = new Vector2(0f, 0f);
		array2[2] = new Vector2(1f, 1f);
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
		shadowObject.transform.parent = base.transform;
		shadowObject.transform.localPosition = Vector3.zero;
	}

	protected void InitBlood(float size)
	{
		bloodObject = Crazy_Global.LoadAssetsPrefab("Textures/blood" + Random.Range(1, 3).ToString("D02"));
		bloodObject.transform.parent = base.transform;
		bloodObject.transform.localPosition = new Vector3(0f, 0.1f, 0f);
		bloodObject.active = false;
		Crazy_PlayAnimation crazy_PlayAnimation = bloodObject.GetComponent("Crazy_PlayAnimation") as Crazy_PlayAnimation;
		crazy_PlayAnimation.Hide();
	}

	public bool IsHurt()
	{
		return curStatus == Crazy_MonsterStatus.Hurt;
	}

	public bool IsDie()
	{
		return curStatus == Crazy_MonsterStatus.Die;
	}

	public bool IsHitRecover()
	{
		return curStatus == Crazy_MonsterStatus.HitRecover;
	}

	protected void PlayEnchantEffect()
	{
		GameObject enchantEffect = target.GetComponent<Crazy_EffectManagement>().GetEnchantEffect();
		if (enchantEffect != null)
		{
			enchantEffect.transform.localPosition = base.transform.localPosition + new Vector3(0f, hurteffectposy, 0f);
			Crazy_ParticleSequenceScript crazy_ParticleSequenceScript = enchantEffect.GetComponent("Crazy_ParticleSequenceScript") as Crazy_ParticleSequenceScript;
			crazy_ParticleSequenceScript.EmitParticle();
			Crazy_PlayTAudio crazy_PlayTAudio = enchantEffect.GetComponent("Crazy_PlayTAudio") as Crazy_PlayTAudio;
			crazy_PlayTAudio.Play();
		}
	}

	protected void PlayHurtEffectEx()
	{
		GameObject bloodEffect = target.GetComponent<Crazy_EffectManagement>().GetBloodEffect();
		if (bloodEffect != null)
		{
			bloodEffect.transform.localPosition = base.transform.localPosition + new Vector3(0f, hurteffectposy, 0f);
			Crazy_ParticleSequenceScript crazy_ParticleSequenceScript = bloodEffect.GetComponent("Crazy_ParticleSequenceScript") as Crazy_ParticleSequenceScript;
			crazy_ParticleSequenceScript.EmitParticle();
		}
	}

	protected virtual void PlayHurtEffect(Crazy_Weapon_Type type)
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
				attackEffect.transform.localEulerAngles = new Vector3(Random.Range(220f, 270f), 90f, -90f);
				crazy_PlayAnimation.Play();
			}
			Crazy_PlayTAudio crazy_PlayTAudio = attackEffect.GetComponent("Crazy_PlayTAudio") as Crazy_PlayTAudio;
			switch (type)
			{
			case Crazy_Weapon_Type.Sword:
				crazy_PlayTAudio.audioname = "Multi_SwordHitFlesh01";
				break;
			case Crazy_Weapon_Type.Hammer:
				crazy_PlayTAudio.audioname = "Fx_HammerHitFlesh";
				break;
			case Crazy_Weapon_Type.Axe:
				crazy_PlayTAudio.audioname = "Multi_AxeHitFlesh01";
				break;
			case Crazy_Weapon_Type.Bow:
				crazy_PlayTAudio.audioname = "Ani_BowHit_Flesh01";
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

	protected bool checkCollideToGround(Vector3 distance)
	{
		Vector3 vector = distance;
		vector.Normalize();
		Vector3 origin = base.transform.position + Vector3.up * 0.5f * GetHitBox() + vector * keepToWalls + distance;
		Ray ray = new Ray(origin, Vector3.down);
		int layerMask = 1 << LayerMask.NameToLayer("Ground");
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, GetHitBox(), layerMask) && hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			return true;
		}
		return false;
	}

	protected bool checkCollideToWall(Vector3 dir, ref Vector3 fixdir)
	{
		Ray ray = new Ray(base.transform.position + Vector3.up + dir * (keepToWalls - 0.1f), dir);
		int layerMask = 1 << LayerMask.NameToLayer("Wall");
		RaycastHit hitInfo;
		if (btouchground && Physics.Raycast(ray, out hitInfo, keepToWalls + GetHitBox(), layerMask))
		{
			Vector3 vector = new Vector3(hitInfo.normal.z, 0f, 0f - hitInfo.normal.x);
			fixdir = vector * Vector3.Dot(ray.direction, vector);
			return true;
		}
		fixdir = Vector3.zero;
		return false;
	}

	protected bool checkCollideToPlayer(Vector3 dir, ref Vector3 fixdir)
	{
		Ray ray = new Ray(base.transform.position + Vector3.up, dir);
		int layerMask = 1 << LayerMask.NameToLayer("Player");
		RaycastHit hitInfo;
		if (btouchground && Physics.Raycast(ray, out hitInfo, keepToPlayers + GetHitBox() / 2f, layerMask))
		{
			Vector3 vector = new Vector3(hitInfo.normal.z, 0f, 0f - hitInfo.normal.x);
			fixdir = vector * Vector3.Dot(ray.direction, vector);
			return true;
		}
		fixdir = Vector3.zero;
		return false;
	}

	protected virtual void Move(Vector3 dis)
	{
		Vector3 vector = dis;
		vector.Normalize();
		float magnitude = dis.magnitude;
		btouchground = checkCollideToGround(dis);
		Vector3 fixdir = default(Vector3);
		if (checkCollideToWall(vector, ref fixdir))
		{
			Vector3 fixdir2 = default(Vector3);
			if (checkCollideToPlayer(fixdir, ref fixdir2))
			{
				base.transform.position += fixdir2 * magnitude;
			}
			else
			{
				base.transform.position += fixdir * magnitude;
			}
		}
		else if (btouchground)
		{
			if (checkCollideToPlayer(vector, ref fixdir))
			{
				base.transform.position += fixdir * magnitude;
			}
			else
			{
				base.transform.position += vector * magnitude;
			}
		}
	}

	protected virtual void MoveReduce(Vector3 dis)
	{
		dis = Vector3.Lerp(base.transform.position, dis, Mathf.Clamp01(beatspeed / 400f)) - base.transform.position;
		beatspeed *= 0.6f;
		Move(dis);
	}

	public virtual bool Hurt(float damage, Crazy_HitData chd, Crazy_Weapon_Type type, bool isskill)
	{
		if (curHp <= damage)
		{
			OffAttack();
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
		if (hitrecover.IsHitRecover() && damage / MaxHp > 0.2f)
		{
			OffAttack();
			Hitted(chd);
			switchMonsterStatus(Crazy_MonsterStatus.Hurt);
			hurt_vec3 = base.transform.position + hitrecover.getBeatMoveDistance();
			beatspeed = hitrecover.getBeatSpeed();
		}
		return false;
	}

	protected void OnInvincible(float time)
	{
		lastinvincibletime = 0f;
		invincibletime = time;
		invincible = true;
		SetModelColorRate(0.4f, 0.4f, 0.4f);
	}

	public void Pause(float time)
	{
		last_pause_time = 0f;
		is_pause = true;
		pause_time = time;
		SetHurtAnimationEffect(0f);
	}

	protected void OffPause()
	{
		is_pause = false;
		SetHurtAnimationEffect(1f);
		switch (dtype)
		{
		case Crazy_MonsterDeathType.DeathType01:
			DieAway();
			break;
		case Crazy_MonsterDeathType.DeathType02:
			DieAway2();
			break;
		case Crazy_MonsterDeathType.DeathType03:
			DieAway3();
			break;
		case Crazy_MonsterDeathType.DeathType04:
			DieAway4();
			break;
		}
		dtype = Crazy_MonsterDeathType.None;
	}

	protected void updateinvincible()
	{
		if (invincible)
		{
			lastinvincibletime += Time.deltaTime;
			if (lastinvincibletime >= invincibletime)
			{
				invincible = false;
				SetModelColorOriginal();
			}
		}
	}

	public virtual void SetHurtAnimationEffect(float effect)
	{
	}

	protected virtual void Hitted(Crazy_HitData chd)
	{
		hitrecover.ApplyBeat(chd);
		hitrecover.ApplyEff(curForwardDir, speedWalk * (1f - skillmoverate));
		lasthitrecovertime = 0f;
	}

	protected void updateAlert()
	{
		lastalerttime += Time.deltaTime;
	}

	private void UpdateAttackLogic(float deltatime)
	{
	}

	protected virtual void Update()
	{
		updateinvincible();
		updateBloodSlot();
		if (Crazy_GlobalData.cur_game_state == Crazy_GameState.Game)
		{
			activetime += Time.deltaTime;
			if (is_pause)
			{
				last_pause_time += Time.deltaTime;
				if (last_pause_time >= pause_time)
				{
					OffPause();
				}
				return;
			}
			_interval_update += Time.deltaTime;
			if (_interval_update >= _Interval_Update)
			{
				updateData(_interval_update);
				_interval_update = 0f;
			}
			switch (curStatus)
			{
			case Crazy_MonsterStatus.Die:
				updateDie(Time.deltaTime);
				break;
			case Crazy_MonsterStatus.Hurt:
				updateHurt(Time.deltaTime);
				break;
			case Crazy_MonsterStatus.HitRecover:
				updateHitRecover(Time.deltaTime);
				break;
			case Crazy_MonsterStatus.Move:
			case Crazy_MonsterStatus.Run:
				updateMove(Time.deltaTime);
				updateTurnRound();
				break;
			case Crazy_MonsterStatus.PreAttack:
			case Crazy_MonsterStatus.Attack:
				updateAttack(Time.deltaTime);
				updateMove(Time.deltaTime);
				break;
			case Crazy_MonsterStatus.EndAttack:
				updateEndAttack(Time.deltaTime);
				break;
			}
			relive();
			reaction(Time.deltaTime);
			updateAlert();
		}
		else
		{
			base.GetComponent<Animation>().Stop();
		}
	}

	protected void updateHurt(float deltatime)
	{
		lasthitrecovertime += deltatime;
		if (lasthitrecovertime <= hitrecover.getBeatTime())
		{
			MoveReduce(hurt_vec3);
		}
		else
		{
			switchMonsterStatus(Crazy_MonsterStatus.HitRecover);
		}
	}

	protected void updateHitRecover(float deltatime)
	{
		lasthitrecovertime += deltatime;
		if (lasthitrecovertime >= hitrecover.getHitRecoverTime())
		{
			switchMonsterStatus(Crazy_MonsterStatus.Idle);
		}
	}

	protected void updatePreAttack(float deltatime)
	{
		preattacklasttime += deltatime;
	}

	protected void OnPreAttack()
	{
		preattacklasttime = 0f;
	}

	protected void ResetPreAttack(float time)
	{
		preattacklasttime = time;
	}

	protected virtual bool IsPreAttack()
	{
		return preattacklasttime < preattacktime * (1f + skillattackrate);
	}

	protected void updateAttack(float deltatime)
	{
		if (curStatus == Crazy_MonsterStatus.PreAttack)
		{
			updatePreAttack(deltatime);
			if (!IsPreAttack())
			{
				ResetPreAttack(0f);
				switchMonsterStatus(Crazy_MonsterStatus.Attack);
			}
		}
	}

	protected virtual void updateEndAttack(float deltatime)
	{
		endattacklasttime += deltatime;
		if (endattacklasttime > endattacktime)
		{
			switchMonsterStatus(Crazy_MonsterStatus.Idle);
			updateTurnRound();
		}
	}

	protected virtual void OnAttack()
	{
		attackForward = curForwardDir;
	}

	public virtual void OnAttackJudgment()
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

	protected virtual bool AttackJudgment()
	{
		Vector2 original = new Vector2(base.transform.position.x, base.transform.position.z);
		Vector2 forward = new Vector2(base.transform.forward.x, base.transform.forward.z);
		Vector2 vector = Crazy_Global.RotatebyAngle(original, forward, pointangle, pointlength);
		attackjudgmentpoint = new Vector3(vector.x, 0f, vector.y);
		Vector3 vector2 = attackjudgmentpoint - target.transform.position;
		float sqrMagnitude = vector2.sqrMagnitude;
		if (sqrMagnitude < attackjudgmentrange * attackjudgmentrange)
		{
			float num = Vector3.Angle(-vector2, base.transform.forward);
			if (num < attackjudgmentangle)
			{
				return true;
			}
		}
		return false;
	}

	public virtual void OffAttack()
	{
		endattacklasttime = 0f;
		switchMonsterStatus(Crazy_MonsterStatus.EndAttack);
	}

	protected virtual void updateTurnRound()
	{
		if (curStatus != Crazy_MonsterStatus.PreAttack && curStatus != Crazy_MonsterStatus.Attack)
		{
			base.transform.forward = curMoveDir;
		}
	}

	protected virtual void updateMove(float deltatime)
	{
		if ((curStatus != Crazy_MonsterStatus.Move && curStatus != Crazy_MonsterStatus.Run) || (!(Mathf.Abs(curMoveDir.x) > 0.001f) && !(Mathf.Abs(curMoveDir.z) > 0.001f)))
		{
			return;
		}
		if (!isalert)
		{
			if (outalertmove)
			{
				outalertmove = false;
				outalertMoveDir = new Vector3(Random.Range(-100, 100), 0f, Random.Range(-100, 100));
				outalertMoveDir.Normalize();
			}
			curMoveDir = outalertMoveDir;
			Move(new Vector3(curMoveDir.x * deltatime * speedWalk * (1f - skillmoverate), 0f, curMoveDir.z * deltatime * speedWalk * (1f - skillmoverate)));
		}
		else
		{
			curMoveDir.Normalize();
			Move(new Vector3(curMoveDir.x * deltatime * speedRun * (1f - skillmoverate), 0f, curMoveDir.z * deltatime * speedRun * (1f - skillmoverate)));
		}
	}

	protected void Idle()
	{
		switchMonsterStatus(Crazy_MonsterStatus.Idle);
	}

	protected virtual void updateData(float deltatime)
	{
		curMoveDir = new Vector3(reftarget.transform.position.x - base.transform.position.x, 0f, reftarget.transform.position.z - base.transform.position.z);
		curForwardDir = new Vector3(reftarget.transform.position.x - base.transform.position.x, 0f, reftarget.transform.position.z - base.transform.position.z);
		if (IsinAlertRange(reftarget))
		{
			if (!isalert && lastalerttime >= alertrecovertime)
			{
				if (Crazy_EnemyManagement.AddActiveNumber())
				{
					isalert = true;
					lastalerttime = 0f;
				}
			}
			else if (isalert && lastalerttime >= alerttime && curStatus == Crazy_MonsterStatus.Run && Crazy_EnemyManagement.RemoveActiveNumber())
			{
				isalert = false;
				lastalerttime = 0f;
			}
		}
		else if (isalert && Crazy_EnemyManagement.RemoveActiveNumber())
		{
			isalert = false;
			lastalerttime = 0f;
		}
		if (curStatus == Crazy_MonsterStatus.Attack || curStatus == Crazy_MonsterStatus.EndAttack)
		{
			return;
		}
		if (IsinAttackRange(reftarget))
		{
			switchMonsterStatus(Crazy_MonsterStatus.PreAttack);
		}
		else if (isalert)
		{
			switchMonsterStatus(Crazy_MonsterStatus.Run);
		}
		else
		{
			if (isalert)
			{
				return;
			}
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
	}

	protected void Dying()
	{
		shadowObject.active = false;
		base.gameObject.layer = LayerMask.NameToLayer("Dead");
		PlayBlood();
		OnDropItem();
	}

	protected virtual void Die()
	{
		dyingtime = 0f;
		Crazy_PlayerControl crazy_PlayerControl = target.GetComponent("Crazy_PlayerControl") as Crazy_PlayerControl;
		crazy_PlayerControl.AddGold(m_gold);
		crazy_PlayerControl.AddExp(m_exp);
		Crazy_GlobalData.cur_kill_number++;
		Crazy_Statistics.AddMonsterKillNumber(Crazy_MonsterType.Normal, m_id);
		Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task06, m_type - 1, 0f);
	}

	protected float GetRandomFloat3(float amin, float amax, float apercent, float bmin, float bmax, float bpercent, float cmin, float cmax, float cpercent)
	{
		List<FloatRegionPercent> list = new List<FloatRegionPercent>();
		FloatRegionPercent item = new FloatRegionPercent(amin, amax, apercent);
		list.Add(item);
		item = new FloatRegionPercent(bmin, bmax, bpercent);
		list.Add(item);
		item = new FloatRegionPercent(cmin, cmax, cpercent);
		list.Add(item);
		return Crazy_Global.FloatRegionRandom(list);
	}

	protected int RandomType()
	{
		return Mathf.FloorToInt(GetRandomFloat3(0f, 0.5f, 0.7f, 1f, 1.5f, 0.25f, 2f, 2.5f, 0.05f));
	}

	protected void DieAway()
	{
		if (is_pause)
		{
			dtype = Crazy_MonsterDeathType.DeathType01;
			return;
		}
		Vector3 vector = base.gameObject.transform.position - target.transform.position;
		vector.y = 0f;
		vector.Normalize();
		float y = 0f;
		float num = 0f;
		switch (RandomType())
		{
		case 0:
			y = Random.Range(2f, 3f);
			num = Random.Range(3f, 4f);
			break;
		case 1:
			y = Random.Range(3f, 6f);
			num = Random.Range(4f, 4.5f);
			break;
		case 2:
			y = Random.Range(6f, 8f);
			num = Random.Range(4.5f, 5f);
			break;
		}
		vector *= Random.Range(8f, 10f);
		vector.y = y;
		Devil_S_Parabola_Arrow devil_S_Parabola_Arrow = base.gameObject.AddComponent<Devil_S_Parabola_Arrow>() as Devil_S_Parabola_Arrow;
		devil_S_Parabola_Arrow.dir2D = new Vector2(vector.x, vector.z);
		devil_S_Parabola_Arrow.dir2D.Normalize();
		devil_S_Parabola_Arrow.dir2D = Crazy_Global.Rotate(devil_S_Parabola_Arrow.dir2D, Random.Range(30f, -30f));
		devil_S_Parabola_Arrow.vel2D = 10f * num;
		devil_S_Parabola_Arrow.velY = vector.y * 6f;
		Crazy_S_Auto_Rotate crazy_S_Auto_Rotate = base.gameObject.AddComponent<Crazy_S_Auto_Rotate>() as Crazy_S_Auto_Rotate;
		crazy_S_Auto_Rotate.rotatespeed = Random.Range(300f, 500f);
		crazy_S_Auto_Rotate.rotatedir = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		crazy_S_Auto_Rotate.rotatedir.Normalize();
	}

	protected void DieAway2()
	{
		if (is_pause)
		{
			dtype = Crazy_MonsterDeathType.DeathType02;
			return;
		}
		float vel2D = 0f;
		float vel2D2 = 0f;
		float velY = 0f;
		float maxrotatetime = 0f;
		switch (RandomType())
		{
		case 0:
			vel2D = Random.Range(4f, 5f);
			vel2D2 = Random.Range(3f, 4f);
			velY = Random.Range(6f, 7f);
			maxrotatetime = Random.Range(10f, 20f);
			break;
		case 1:
			vel2D = Random.Range(7f, 7.5f);
			vel2D2 = Random.Range(6f, 10f);
			velY = Random.Range(9f, 11f);
			maxrotatetime = Random.Range(0.025f, 0.033f);
			break;
		case 2:
			vel2D = Random.Range(7.5f, 8f);
			vel2D2 = Random.Range(15f, 20f);
			velY = Random.Range(14f, 15f);
			maxrotatetime = Random.Range(0.02f, 0.025f);
			break;
		}
		Devil_S_Parabola_Arrow devil_S_Parabola_Arrow = base.gameObject.AddComponent<Devil_S_Parabola_Arrow>() as Devil_S_Parabola_Arrow;
		Vector3 vector = base.gameObject.transform.position - target.transform.position;
		devil_S_Parabola_Arrow.dir2D = new Vector2(vector.x, vector.z);
		devil_S_Parabola_Arrow.vel2D = vel2D;
		if (headg != null)
		{
			for (int i = 0; i < headg.GetLength(0); i++)
			{
				headg[i].transform.parent = base.gameObject.transform.parent;
				Crazy_S_Parabola_Arrow crazy_S_Parabola_Arrow = headg[i].AddComponent<Crazy_S_Parabola_Arrow>() as Crazy_S_Parabola_Arrow;
				crazy_S_Parabola_Arrow.dir2D = new Vector2(vector.x, vector.z);
				crazy_S_Parabola_Arrow.dir2D.Normalize();
				crazy_S_Parabola_Arrow.dir2D = Crazy_Global.Rotate(crazy_S_Parabola_Arrow.dir2D, Random.Range(30f, -30f));
				crazy_S_Parabola_Arrow.vel2D = vel2D2;
				crazy_S_Parabola_Arrow.velY = velY;
				crazy_S_Parabola_Arrow.maxrotatetime = maxrotatetime;
			}
		}
	}

	protected void DieAway3()
	{
		if (is_pause)
		{
			dtype = Crazy_MonsterDeathType.DeathType03;
			return;
		}
		Devil_S_Parabola_Arrow devil_S_Parabola_Arrow = base.gameObject.AddComponent<Devil_S_Parabola_Arrow>() as Devil_S_Parabola_Arrow;
		Vector3 vector = base.gameObject.transform.position - target.transform.position;
		devil_S_Parabola_Arrow.dir2D = new Vector2(vector.x, vector.z);
		devil_S_Parabola_Arrow.vel2D = GetRandomFloat3(1f, 1.2f, 0.7f, 1.4f, 1.6f, 0.25f, 1.8f, 2f, 0.05f);
	}

	protected void DieAway4()
	{
		if (is_pause)
		{
			dtype = Crazy_MonsterDeathType.DeathType04;
			return;
		}
		Vector3 vector = base.gameObject.transform.position - target.transform.position;
		vector.y = 0f;
		vector.Normalize();
		vector *= Random.Range(6f, 8f);
		vector.y = 10f;
		Devil_S_Parabola_Arrow devil_S_Parabola_Arrow = base.gameObject.AddComponent<Devil_S_Parabola_Arrow>() as Devil_S_Parabola_Arrow;
		devil_S_Parabola_Arrow.dir2D = new Vector2(vector.x, vector.z);
		devil_S_Parabola_Arrow.dir2D.Normalize();
		devil_S_Parabola_Arrow.dir2D = Crazy_Global.Rotate(devil_S_Parabola_Arrow.dir2D, Random.Range(15f, -15f));
		devil_S_Parabola_Arrow.vel2D = 10f;
		devil_S_Parabola_Arrow.velY = vector.y;
	}

	protected void OnDropItem()
	{
		switch (m_item)
		{
		case 1:
		{
			GameObject gameObject2 = Crazy_ItemManager.CreateItem("FBX/item/heal/heal_000_pfb", ColliderMessage.HealUp);
			if (gameObject2 != null)
			{
				gameObject2.transform.position = new Vector3(bloodObject.transform.position.x, 0.1f, bloodObject.transform.position.z);
				gameObject2.transform.parent = RootNode.transform;
			}
			break;
		}
		case 2:
		{
			GameObject gameObject3 = Crazy_ItemManager.CreateItem("FBX/item/speed/speed_000_pfb", ColliderMessage.SpeedUp);
			if (gameObject3 != null)
			{
				gameObject3.transform.position = new Vector3(bloodObject.transform.position.x, 0.1f, bloodObject.transform.position.z);
				gameObject3.transform.parent = RootNode.transform;
			}
			break;
		}
		case 3:
		{
			GameObject gameObject = Crazy_ItemManager.CreateItem("FBX/item/shield/shield_000_pfb", ColliderMessage.Invincible);
			if (gameObject != null)
			{
				gameObject.transform.position = new Vector3(bloodObject.transform.position.x, 0.1f, bloodObject.transform.position.z);
				gameObject.transform.parent = RootNode.transform;
			}
			break;
		}
		}
	}

	protected void PlayBlood()
	{
		if (bloodObject != null)
		{
			bloodObject.active = true;
			bloodObject.transform.parent = RootNode.transform;
			bloodObject.transform.position = new Vector3(bloodObject.transform.position.x, 0.1f, bloodObject.transform.position.z);
			Crazy_PlayAnimation crazy_PlayAnimation = bloodObject.GetComponent("Crazy_PlayAnimation") as Crazy_PlayAnimation;
			crazy_PlayAnimation.Play();
		}
	}

	protected void updateDie(float deltatime)
	{
		dyingtime += deltatime;
		if (!(dyingtime > recycletime) || Crazy_GlobalData.enemyList == null)
		{
			return;
		}
		Dictionary<int, GameObject>.KeyCollection keys = Crazy_GlobalData.enemyList.Keys;
		foreach (int item in keys)
		{
			GameObject value;
			if (!Crazy_GlobalData.enemyList.TryGetValue(item, out value) || !(value == base.gameObject))
			{
				continue;
			}
			Crazy_GlobalData.enemyList.Remove(item);
			if (isalert)
			{
				Crazy_EnemyManagement.RemoveActiveNumber();
			}
			foreach (GameObject item2 in bindobject)
			{
				Object.Destroy(item2);
			}
			Object.Destroy(base.gameObject);
			if (headg != null)
			{
				for (int i = 0; i < headg.GetLength(0); i++)
				{
					Object.Destroy(headg[i]);
				}
			}
			break;
		}
	}

	protected virtual void switchMonsterStatus(Crazy_MonsterStatus toStatus)
	{
		if ((toStatus == Crazy_MonsterStatus.Idle || toStatus == Crazy_MonsterStatus.HitRecover || curStatus <= toStatus) && curStatus != toStatus)
		{
			ResetPreAttack(0f);
			switch (toStatus)
			{
			case Crazy_MonsterStatus.PreAttack:
				updateTurnRound();
				break;
			case Crazy_MonsterStatus.Attack:
				OnAttack();
				break;
			case Crazy_MonsterStatus.Die:
				Die();
				break;
			}
			PlayAnimation(toStatus);
			curStatus = toStatus;
		}
	}

	protected virtual bool IsinAttackRange(GameObject tar)
	{
		return (tar.transform.position - base.transform.position).sqrMagnitude < (attackrange + GetHitBox()) * (attackrange + GetHitBox());
	}

	protected virtual bool IsinAlertRange(GameObject tar)
	{
		return (tar.transform.position - base.transform.position).sqrMagnitude < (alertrange + GetHitBox()) * (alertrange + GetHitBox());
	}

	protected virtual void PlayAnimation(Crazy_MonsterStatus toStatus)
	{
	}

	protected virtual void reaction(float time)
	{
		if (curStatus == Crazy_MonsterStatus.Attack)
		{
			lastreactiontime += time;
			if (lastreactiontime >= reactiontime)
			{
				OffAttack();
			}
		}
		else
		{
			lastreactiontime = 0f;
		}
	}

	protected void relive()
	{
		if (!IsDie() && (base.gameObject.transform.position.y < -1f || base.gameObject.transform.position.x >= rangeout || base.gameObject.transform.position.x <= 0f - rangeout || base.gameObject.transform.position.z >= rangeout || base.gameObject.transform.position.z <= 0f - rangeout))
		{
			base.gameObject.transform.position = Crazy_Global.FindMonsterAppPosition(prange);
		}
	}

	public virtual Renderer[] FindMeshRenderer()
	{
		return null;
	}

	public Renderer[] GetMeshRenderer()
	{
		return meshrenderer;
	}

	public float GetHitBox()
	{
		return hitbox;
	}

	protected void SetModelColor(Color color)
	{
		if (mat_color == null)
		{
			return;
		}
		Dictionary<Material, Color>.KeyCollection keys = mat_color.Keys;
		foreach (Material item in keys)
		{
			item.color = color;
		}
	}

	protected void SetModelColorAlpha(float a)
	{
		if (mat_color == null)
		{
			return;
		}
		Dictionary<Material, Color>.KeyCollection keys = mat_color.Keys;
		foreach (Material item in keys)
		{
			Color color = new Color(item.color.r, item.color.g, item.color.b, a);
			item.color = color;
		}
	}

	protected void SetModelColorRate(float rate_r, float rate_g, float rate_b)
	{
		if (mat_color == null)
		{
			return;
		}
		Dictionary<Material, Color>.KeyCollection keys = mat_color.Keys;
		foreach (Material item in keys)
		{
			Color value;
			if (mat_color.TryGetValue(item, out value))
			{
				value = (item.color = new Color(value.r + 1f * rate_r, value.g + 1f * rate_g, value.b + 1f * rate_b, value.a));
			}
		}
	}

	protected void SetModelColorOriginal()
	{
		if (mat_color == null)
		{
			return;
		}
		Dictionary<Material, Color>.KeyCollection keys = mat_color.Keys;
		foreach (Material item in keys)
		{
			Color value;
			if (mat_color.TryGetValue(item, out value))
			{
				item.color = value;
			}
		}
	}
}
