using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHero : MonoBehaviour
{
	public new Transform camera;

	protected GameObject m_cha;

	protected List<Crazy_Weapon> weapons = new List<Crazy_Weapon>();

	protected GameObject weaponObj;

	private GameObject increaseblood;

	private GameObject invincibleObj;

	private GameObject speedupObj;

	private Dictionary<Material, Color> player_color = new Dictionary<Material, Color>();

	private float fDelta;

	private void Start()
	{
		MyGUIEventListener.Get(base.gameObject).EventHandleOnMoved += OnRotate;
		switch (Crazy_Data.CurData().GetPlayerClass())
		{
		case Crazy_PlayerClass.Fighter:
			m_cha = Crazy_Global.LoadAssetsPrefab("FBX/character/fighter/fighter_pfb");
			break;
		case Crazy_PlayerClass.Knight:
			m_cha = Crazy_Global.LoadAssetsPrefab("FBX/character/knight/ui_knight_pfb");
			break;
		case Crazy_PlayerClass.Warrior:
			m_cha = Crazy_Global.LoadAssetsPrefab("FBX/character/warrior/ui_warrior_pfb");
			break;
		case Crazy_PlayerClass.Rogue:
			m_cha = Crazy_Global.LoadAssetsPrefab("FBX/character/rogue/ui_rogue_pfb");
			break;
		case Crazy_PlayerClass.Paladin:
			m_cha = Crazy_Global.LoadAssetsPrefab("FBX/character/paladin/ui_paladin_pfb");
			break;
		case Crazy_PlayerClass.Mage:
			m_cha = Crazy_Global.LoadAssetsPrefab("FBX/character/mage/ui_mage_pfb");
			break;
		default:
			Debug.LogError("CharError");
			break;
		}
		m_cha.transform.parent = camera;
		m_cha.transform.localPosition = new Vector3(-2.5f, 0.9f, 7.4f);
		m_cha.transform.localEulerAngles = new Vector3(10f, 161.81f, 0f);
		if (Crazy_Data.CurData().GetPlayerClass() == Crazy_PlayerClass.Mage)
		{
			m_cha.GetComponent<Animation>()["Idle_Staff01_celebrate_merge"].wrapMode = WrapMode.Once;
			m_cha.GetComponent<Animation>()["Idle_Staff_UI_merge"].wrapMode = WrapMode.Loop;
			m_cha.GetComponent<Animation>()["Forward01_Staff01_merge"].wrapMode = WrapMode.Once;
			m_cha.GetComponent<Animation>()["Forward01_Staff01_merge"].layer = 1;
			m_cha.GetComponent<Animation>()["Idle_Staff01_celebrate_merge"].layer = 1;
			m_cha.GetComponent<Animation>()["Idle_Staff_UI_merge"].layer = 0;
		}
		else
		{
			m_cha.GetComponent<Animation>()["Idle_Axe_UI_merge"].wrapMode = WrapMode.Loop;
			m_cha.GetComponent<Animation>()["Idle_Sword_UI_merge"].wrapMode = WrapMode.Loop;
			m_cha.GetComponent<Animation>()["Idle_Hammer_UI_merge"].wrapMode = WrapMode.Loop;
			m_cha.GetComponent<Animation>()["Idle_Bow_UI_merge"].wrapMode = WrapMode.Loop;
			m_cha.GetComponent<Animation>()["Idle_Axe_UI_merge"].layer = 0;
			m_cha.GetComponent<Animation>()["Idle_Sword_UI_merge"].layer = 0;
			m_cha.GetComponent<Animation>()["Idle_Hammer_UI_merge"].layer = 0;
			m_cha.GetComponent<Animation>()["Idle_Bow_UI_merge"].layer = 0;
			m_cha.GetComponent<Animation>()["Forward01_Axe01_merge"].wrapMode = WrapMode.Once;
			m_cha.GetComponent<Animation>()["Forward01_Bow01_merge"].wrapMode = WrapMode.Once;
			m_cha.GetComponent<Animation>()["Forward01_Hammer01_merge"].wrapMode = WrapMode.Once;
			m_cha.GetComponent<Animation>()["Forward01_Sword01_merge"].wrapMode = WrapMode.Once;
			m_cha.GetComponent<Animation>()["Forward01_Axe01_merge"].layer = 1;
			m_cha.GetComponent<Animation>()["Forward01_Bow01_merge"].layer = 1;
			m_cha.GetComponent<Animation>()["Forward01_Hammer01_merge"].layer = 1;
			m_cha.GetComponent<Animation>()["Forward01_Sword01_merge"].layer = 1;
			m_cha.GetComponent<Animation>()["Idle_Axe01_celebrate_merge"].wrapMode = WrapMode.Once;
			m_cha.GetComponent<Animation>()["Idle_Sword01_celebrate_merge"].wrapMode = WrapMode.Once;
			m_cha.GetComponent<Animation>()["Idle_Hammer01_celebrate_merge"].wrapMode = WrapMode.Once;
			m_cha.GetComponent<Animation>()["Idle_Bow01_celebrate_merge"].wrapMode = WrapMode.Once;
			m_cha.GetComponent<Animation>()["Idle_Axe01_celebrate_merge"].layer = 1;
			m_cha.GetComponent<Animation>()["Idle_Sword01_celebrate_merge"].layer = 1;
			m_cha.GetComponent<Animation>()["Idle_Hammer01_celebrate_merge"].layer = 1;
			m_cha.GetComponent<Animation>()["Idle_Bow01_celebrate_merge"].layer = 1;
		}
		Transform transform = m_cha.transform.Find("zhujue");
		SkinnedMeshRenderer skinnedMeshRenderer = transform.gameObject.GetComponent("SkinnedMeshRenderer") as SkinnedMeshRenderer;
		Material[] materials = skinnedMeshRenderer.materials;
		for (int i = 0; i < materials.Length; i++)
		{
			player_color.Add(materials[i], materials[i].color);
		}
		LoadEffect(m_cha);
		weapons = Crazy_Weapon.ReadWeaponInfo();
		EquipWeaponOriginal();
	}

	private void LoadEffect(GameObject cha)
	{
		increaseblood = Object.Instantiate(Resources.Load("Prefabs/increase blood/increase blood_pfb")) as GameObject;
		increaseblood.layer = cha.layer;
		for (int i = 0; i < increaseblood.transform.childCount; i++)
		{
			increaseblood.transform.GetChild(i).gameObject.layer = cha.layer;
		}
		increaseblood.transform.parent = cha.transform;
		increaseblood.transform.localPosition = Vector3.zero;
		speedupObj = Object.Instantiate(Resources.Load("Prefabs/speed up/speedup_pfb")) as GameObject;
		speedupObj.layer = cha.layer;
		for (int j = 0; j < speedupObj.transform.childCount; j++)
		{
			speedupObj.transform.GetChild(j).gameObject.layer = cha.layer;
		}
		speedupObj.transform.parent = cha.transform;
		speedupObj.transform.localPosition = new Vector3(0f, 0.2f, 0f);
		invincibleObj = Object.Instantiate(Resources.Load("Prefabs/invincible/invincible_pfb")) as GameObject;
		invincibleObj.layer = cha.layer;
		for (int k = 0; k < invincibleObj.transform.childCount; k++)
		{
			invincibleObj.transform.GetChild(k).gameObject.layer = cha.layer;
		}
		invincibleObj.transform.parent = cha.transform;
		invincibleObj.transform.position = cha.transform.position + new Vector3(0f, 2.5f, 1.8f);
	}

	public void PlayEffect(string name)
	{
		switch (name)
		{
		case "hp1":
		case "hp2":
		{
			Crazy_ParticleSequenceScript crazy_ParticleSequenceScript3 = increaseblood.GetComponent("Crazy_ParticleSequenceScript") as Crazy_ParticleSequenceScript;
			crazy_ParticleSequenceScript3.EmitParticle();
			base.transform.GetComponent<TAudioController>().PlayAudio("Fx_HpUp");
			break;
		}
		case "speed1":
		case "speed2":
		{
			Crazy_ParticleSequenceScript crazy_ParticleSequenceScript2 = speedupObj.GetComponent("Crazy_ParticleSequenceScript") as Crazy_ParticleSequenceScript;
			crazy_ParticleSequenceScript2.EmitParticle();
			PlayRunAni();
			base.transform.GetComponent<TAudioController>().PlayAudio("Fx_SpeedUp");
			break;
		}
		case "wudi1":
		case "wudi2":
		{
			base.transform.GetComponent<TAudioController>().PlayAudio("Fx_DefenseUp");
			Crazy_ParticleSequenceScript crazy_ParticleSequenceScript = invincibleObj.GetComponent("Crazy_ParticleSequenceScript") as Crazy_ParticleSequenceScript;
			crazy_ParticleSequenceScript.EmitParticle();
			StartCoroutine(HeroInvincible());
			break;
		}
		default:
			Debug.LogWarning("unkonw prop");
			break;
		}
	}

	private IEnumerator HeroInvincible()
	{
		float a = Random.Range(0f, 0.4f);
		SetModelColorRate(a, a, 0f);
		yield return new WaitForSeconds(1f);
		SetModelColorOriginal();
	}

	protected void SetModelColorOriginal()
	{
		if (player_color == null)
		{
			return;
		}
		Dictionary<Material, Color>.KeyCollection keys = player_color.Keys;
		foreach (Material item in keys)
		{
			Color value;
			if (player_color.TryGetValue(item, out value))
			{
				item.color = value;
			}
		}
	}

	protected void SetModelColorRate(float rate_r, float rate_g, float rate_b)
	{
		if (player_color == null)
		{
			return;
		}
		Dictionary<Material, Color>.KeyCollection keys = player_color.Keys;
		foreach (Material item in keys)
		{
			Color value;
			if (player_color.TryGetValue(item, out value))
			{
				value = (item.color = new Color(value.r + 1f * rate_r, value.g + 1f * rate_g, value.b + 1f * rate_b, value.a));
			}
		}
	}

	private void EquipWeaponOriginal()
	{
		foreach (Crazy_Weapon weapon in weapons)
		{
			if (weapon.id == Crazy_Data.CurData().GetWeaponId())
			{
				EquipWeaponDemo(weapon);
			}
		}
	}

	private void OnEnable()
	{
		Invoke("InitHero", 0.2f);
	}

	private void Update()
	{
		fDelta += Time.deltaTime;
		if (fDelta > 10f)
		{
			fDelta = 0f;
			InitHero();
		}
	}

	public void InitHero()
	{
		m_cha.transform.localEulerAngles = new Vector3(10f, 161.81f, 0f);
		foreach (Crazy_Weapon weapon in weapons)
		{
			if (weapon.id == Crazy_Data.CurData().GetWeaponId())
			{
				EquipWeaponDemo(weapon);
			}
		}
	}

	private void PlayRunAni()
	{
		foreach (Crazy_Weapon weapon in weapons)
		{
			if (weapon.id == Crazy_Data.CurData().GetWeaponId())
			{
				if (weaponObj != null)
				{
					Object.Destroy(weaponObj);
				}
				weaponObj = Object.Instantiate(Resources.Load(weapon.loadpath)) as GameObject;
				weaponObj.name = "Weapon";
				weaponObj.transform.parent = FindWeaponBone(weapon.type, m_cha).transform;
				weaponObj.transform.localPosition = weapon.modifyPos;
				weaponObj.transform.localEulerAngles = weapon.modifyAngle;
				m_cha.GetComponent<Animation>().Play("Idle_" + weapon.type_name + "_UI_merge");
				m_cha.GetComponent<Animation>().Play("Forward01_" + weapon.type_name + "01_merge");
			}
		}
	}

	private void EquipWeaponDemo(Crazy_Weapon weapon)
	{
		if (weaponObj != null)
		{
			Object.Destroy(weaponObj);
		}
		weaponObj = Object.Instantiate(Resources.Load(weapon.loadpath)) as GameObject;
		weaponObj.name = "Weapon";
		weaponObj.transform.parent = FindWeaponBone(weapon.type, m_cha).transform;
		weaponObj.transform.localPosition = weapon.modifyPos;
		weaponObj.transform.localEulerAngles = weapon.modifyAngle;
		m_cha.GetComponent<Animation>().Play("Idle_" + weapon.type_name + "_UI_merge");
		m_cha.GetComponent<Animation>().Play("Idle_" + weapon.type_name + "01_celebrate_merge");
	}

	private GameObject FindWeaponBone(Crazy_Weapon_Type cwt, GameObject person)
	{
		switch (cwt)
		{
		case Crazy_Weapon_Type.Sword:
		case Crazy_Weapon_Type.Hammer:
		case Crazy_Weapon_Type.Axe:
		case Crazy_Weapon_Type.Staff:
			return person.transform.Find("Bone/Pelvis/Spine/Right_Shoulder/Right_Hand/Weapon").gameObject;
		case Crazy_Weapon_Type.Bow:
			return person.transform.Find("Bone/Pelvis/Spine/Left_Shoulder/Left_Hand/L_Weapon").gameObject;
		default:
			return null;
		}
	}

	private void OnRotate(GameObject go, Vector2 delta)
	{
		if (delta.x < -0.1f)
		{
			m_cha.transform.Rotate(Vector3.up, 16f);
		}
		else if (delta.x > 0.1f)
		{
			m_cha.transform.Rotate(Vector3.up, -16f);
		}
	}
}
