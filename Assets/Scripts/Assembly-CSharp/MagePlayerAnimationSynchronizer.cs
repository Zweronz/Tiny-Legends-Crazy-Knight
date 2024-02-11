using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagePlayerAnimationSynchronizer : PlayerAnimationSynchronizer
{
	public new float animationspeedmodify = 1f;

	private string m_attackAnimName;

	private GameObject obj;

	private GameObject meteorPlayerEffectobj;

	private GameObject meteorEffectobj;

	private GameObject[] meteorHitEffectobj = new GameObject[5];

	private GameObject mageAttackEffectobj;

	private GameObject blinkStartEffectobj;

	private GameObject blinkEndEffectobj;

	protected override void ModifyAnimation()
	{
		base.GetComponent<Animation>().wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["Forward01_Staff01_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Idle01_Staff01_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Death01_Staff01_merge"].wrapMode = WrapMode.ClampForever;
		base.GetComponent<Animation>()["Idle_Staff01_celebrate01_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Attack01_Staff01_merge"].wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["Skill01_Loop_Staff_merge"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["Forward01_Staff01_merge"].layer = 0;
		base.GetComponent<Animation>()["Idle01_Staff01_merge"].layer = 0;
		base.GetComponent<Animation>()["Attack01_Staff01_merge"].layer = 2;
		base.GetComponent<Animation>()["Skill01_Start_Staff_merge"].layer = 2;
		base.GetComponent<Animation>()["Skill01_Loop_Staff_merge"].layer = 2;
		base.GetComponent<Animation>()["Skill01_End_Staff_merge"].layer = 2;
		base.GetComponent<Animation>()["Blink01_Staff_merge"].layer = 3;
		base.GetComponent<Animation>()["Damage01_Staff01_merge"].layer = 3;
		base.GetComponent<Animation>()["Knockdown_Staff01_merge"].layer = 3;
		base.GetComponent<Animation>()["Death01_Staff01_merge"].layer = 4;
		base.GetComponent<Animation>()["Idle_Staff01_celebrate01_merge"].layer = 5;
		base.GetComponent<Animation>()["Attack01_Staff01_merge"].speed = 1f * animationspeedmodify;
		base.GetComponent<Animation>()["Skill01_Start_Staff_merge"].speed = 1f * animationspeedmodify;
		base.GetComponent<Animation>()["Skill01_Loop_Staff_merge"].speed = 1f * animationspeedmodify;
		base.GetComponent<Animation>()["Skill01_End_Staff_merge"].speed = 1f * animationspeedmodify;
	}

	protected override void InitWeaponEffect()
	{
		attackeffectdic.Clear();
		attackeffectdic = new Dictionary<string, GameObject>();
		if (weapon.type == Crazy_Weapon_Type.Staff)
		{
			blinkStartEffectobj = Crazy_Global.LoadAssetsPrefab("Prefabs/flash/blinkStart_pfb");
			blinkStartEffectobj.layer = LayerMask.NameToLayer("Player");
			blinkStartEffectobj.name = "blinkStart_effect";
			blinkStartEffectobj.transform.parent = RootNode.transform;
			blinkStartEffectobj.transform.position = base.transform.position;
			blinkEndEffectobj = Crazy_Global.LoadAssetsPrefab("Prefabs/flash/blinkEnd_pfb");
			blinkEndEffectobj.layer = LayerMask.NameToLayer("Player");
			blinkEndEffectobj.name = "blinkEnd_effect";
			blinkEndEffectobj.transform.parent = base.gameObject.transform;
			blinkEndEffectobj.transform.localPosition = Vector3.zero;
			meteorPlayerEffectobj = Crazy_Global.LoadAssetsPrefab("Prefabs/staffskilleffect/meteorPlayerEffect_pfb");
			meteorPlayerEffectobj.layer = LayerMask.NameToLayer("Player");
			meteorPlayerEffectobj.name = "staffplayer_effect";
			meteorPlayerEffectobj.transform.parent = base.gameObject.transform;
			meteorPlayerEffectobj.transform.localPosition = Vector3.zero;
			meteorPlayerEffectobj.SetActiveRecursively(false);
			obj = (GameObject)Resources.Load("Prefabs/staffskilleffect/meteorEffect_pfb");
			for (int i = 0; i < 5; i++)
			{
				meteorHitEffectobj[i] = Crazy_Global.LoadAssetsPrefab("Prefabs/staffskilleffect/meteorHitEffect_pfb");
				meteorHitEffectobj[i].layer = LayerMask.NameToLayer("Player");
				meteorHitEffectobj[i].name = "meteor_hit_effect";
				meteorHitEffectobj[i].transform.parent = RootNode.transform;
				meteorHitEffectobj[i].transform.localPosition = Vector3.zero;
			}
			mageAttackEffectobj = Crazy_Global.LoadAssetsPrefab("Prefabs/mageAttackEffect/mobao_pfb");
			mageAttackEffectobj.layer = LayerMask.NameToLayer("Player");
			mageAttackEffectobj.name = "attack_effect";
			mageAttackEffectobj.transform.parent = base.gameObject.transform;
			mageAttackEffectobj.transform.localPosition = Vector3.zero;
		}
	}

	private void PlayPlayerEffect()
	{
		meteorPlayerEffectobj.SetActiveRecursively(true);
		meteorPlayerEffectobj.transform.GetComponentInChildren<ParticleSystem>().Stop(true);
		meteorPlayerEffectobj.transform.GetComponentInChildren<ParticleSystem>().Play(true);
	}

	private void CreateMeteorEffect(Vector3 pos)
	{
		meteorEffectobj = (GameObject)Object.Instantiate(obj, pos, Quaternion.identity);
		meteorEffectobj.layer = LayerMask.NameToLayer("Player");
		meteorEffectobj.name = "meteor_effect";
		meteorEffectobj.transform.parent = RootNode.transform;
	}

	private void PlayMeteorEffect(int i)
	{
		if (Crazy_GlobalData.enemyList == null || Crazy_GlobalData.enemyList.Count == 0)
		{
			CreateMeteorEffect(base.transform.position);
			meteorHitEffectobj[i].transform.localPosition = base.transform.position;
		}
		else
		{
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject value in Crazy_GlobalData.enemyList.Values)
			{
				list.Add(value);
			}
			int index = Random.Range(0, list.Count - 1);
			Vector3 vector = new Vector3(list[index].transform.position.x, 0f, list[index].transform.position.z);
			if (i == 4)
			{
				CreateMeteorEffect(base.transform.position);
				meteorHitEffectobj[i].transform.position = base.transform.position;
			}
			else
			{
				CreateMeteorEffect(vector);
				meteorHitEffectobj[i].transform.position = vector;
			}
		}
		meteorEffectobj.transform.GetComponentInChildren<ParticleSystem>().Play(true);
	}

	private void PlayMeteorHitEffect(int i)
	{
		meteorHitEffectobj[i].transform.GetComponentInChildren<ParticleSystem>().Stop(true);
		meteorHitEffectobj[i].transform.GetComponentInChildren<ParticleSystem>().Play(true);
	}

	public override void ReceiverStatus(Crazy_PlayerStatus status, string attackname)
	{
		switch (status)
		{
		case Crazy_PlayerStatus.Idle:
			base.GetComponent<Animation>().Stop();
			base.GetComponent<Animation>().CrossFade("Idle01_" + weapon.type_name + "01_merge", 0.2f);
			attacking = false;
			break;
		case Crazy_PlayerStatus.Die:
			base.GetComponent<Animation>().Play("Death01_" + weapon.type_name + "01_merge");
			attacking = false;
			break;
		case Crazy_PlayerStatus.Hurt:
			attacking = false;
			OnInvincible(3f);
			base.GetComponent<Animation>().Play("Damage01_" + weapon.type_name + "01_merge");
			break;
		case Crazy_PlayerStatus.Injury:
			attacking = false;
			OnInvincible(3f);
			base.GetComponent<Animation>().Play("Knockdown_" + weapon.type_name + "01_merge");
			break;
		case Crazy_PlayerStatus.Deject:
			attacking = false;
			base.GetComponent<Animation>().Play(weapon.type_name + "01_lose_merge");
			break;
		case Crazy_PlayerStatus.Celebrate:
			attacking = false;
			base.GetComponent<Animation>().Play("Idle_" + weapon.type_name + "01_celebrate01_merge");
			break;
		case Crazy_PlayerStatus.Roll:
			attacking = false;
			base.GetComponent<Animation>().CrossFade("Blink01_Staff_merge", 0.1f);
			blinkStartEffectobj.transform.position = base.transform.position;
			blinkStartEffectobj.transform.GetComponentInChildren<ParticleSystem>().Play();
			StartCoroutine(Blink());
			break;
		case Crazy_PlayerStatus.Shot:
			attacking = false;
			cur_status = attackstatus[attackname];
			AnimationCrossFade(cur_status.attackanimname, 0.1f);
			break;
		case Crazy_PlayerStatus.Attack:
			attacking = true;
			if (attackname == "Attack01")
			{
				attackname = "Attack01_Staff01";
			}
			cur_status = attackstatus[attackname];
			mageAttackEffectobj.transform.GetComponentInChildren<ParticleSystem>().Stop(true);
			mageAttackEffectobj.transform.GetComponentInChildren<ParticleSystem>().Play(true);
			AnimationCrossFade(cur_status.attackanimname, 0.1f);
			lastattackingtime = 0f;
			lastattackjudgmenttime.Clear();
			foreach (Crazy_AttackJudgmentInfo item in cur_status.attackjudgmentinfo)
			{
				lastattackjudgmenttime.Add(item.attackjudgmenttime);
			}
			lastadvattackeffecttime = cur_status.attackadveffectdata.begintime;
			break;
		case Crazy_PlayerStatus.Skill:
			attacking = true;
			cur_status = attackstatus[attackname];
			lastattackingtime = 0f;
			lastattackjudgmenttime.Clear();
			foreach (Crazy_AttackJudgmentInfo item2 in cur_status.attackjudgmentinfo)
			{
				lastattackjudgmenttime.Add(item2.attackjudgmenttime);
			}
			lastadvattackeffecttime = cur_status.attackadveffectdata.begintime;
			if (weapon.type == Crazy_Weapon_Type.Staff)
			{
				PlayPlayerEffect();
				StartCoroutine(PlayMeteorDown());
			}
			break;
		default:
			attacking = false;
			break;
		}
	}

	private IEnumerator PlayMeteorDown()
	{
		AnimationCrossFade("Skill01_Start_" + weapon.type_name + "_merge", 0.1f);
		yield return new WaitForSeconds(1.667f);
		AnimationCrossFade("Skill01_Loop_" + weapon.type_name + "_merge", 0.1f);
		for (int i = 0; i < 5; i++)
		{
			PlayMeteorEffect(i);
			yield return new WaitForSeconds(0.7f);
			PlayMeteorHitEffect(i);
			Object.DestroyObject(meteorEffectobj);
			ExtraAttackMaker(0);
			ExtraAttackEffect();
		}
		meteorPlayerEffectobj.SetActiveRecursively(false);
		yield return null;
	}

	private IEnumerator Blink()
	{
		yield return new WaitForSeconds(0.35f);
		blinkEndEffectobj.transform.GetComponentInChildren<ParticleSystem>().Play();
	}
}
