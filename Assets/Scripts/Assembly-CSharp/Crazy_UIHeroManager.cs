using System.Collections.Generic;
using UnityEngine;

public class Crazy_UIHeroManager : MonoBehaviour
{
	public GameObject hero;

	public int weaponid;

	public Crazy_PlayerClass cpc;

	protected List<Crazy_Weapon> weapons = new List<Crazy_Weapon>();

	private Material[] ownmats;

	private Material weaponmat;

	private Crazy_Weapon weapon;

	private void Start()
	{
		weapons = Crazy_Weapon.ReadWeaponInfo();
		weapon = FindWeapon(weaponid);
		EquipWeapon(weaponid, hero);
		GetHeroStateInfo();
		UpdateHeroState(Crazy_Data.CurData().GetUnlock(cpc));
	}

	public void UpdateHero()
	{
		UpdateHeroState(Crazy_Data.CurData().GetUnlock(cpc));
	}

	protected void GetHeroStateInfo()
	{
		Transform transform = hero.transform.Find("zhujue");
		SkinnedMeshRenderer skinnedMeshRenderer = transform.gameObject.GetComponent("SkinnedMeshRenderer") as SkinnedMeshRenderer;
		ownmats = skinnedMeshRenderer.materials;
		SkinnedMeshRenderer component = FindWeaponBoneRenderer(weapon.type, hero).GetComponent<SkinnedMeshRenderer>();
		weaponmat = component.material;
	}

	protected void UpdateHeroState(bool up)
	{
		if (!up)
		{
			Transform transform = hero.transform.Find("zhujue");
			SkinnedMeshRenderer skinnedMeshRenderer = transform.gameObject.GetComponent("SkinnedMeshRenderer") as SkinnedMeshRenderer;
			Material[] materials = skinnedMeshRenderer.materials;
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i] = Object.Instantiate(Resources.Load("Textures/Character/Crazy" + cpc.ToString() + "-Material #" + (i + 1))) as Material;
			}
			skinnedMeshRenderer.materials = materials;
			SkinnedMeshRenderer component = FindWeaponBoneRenderer(weapon.type, hero).GetComponent<SkinnedMeshRenderer>();
			component.material = Object.Instantiate(Resources.Load("Textures/Weapon/Crazy" + cpc.ToString() + "-WeaponMaterial")) as Material;
		}
		else
		{
			Transform transform2 = hero.transform.Find("zhujue");
			SkinnedMeshRenderer skinnedMeshRenderer2 = transform2.gameObject.GetComponent("SkinnedMeshRenderer") as SkinnedMeshRenderer;
			skinnedMeshRenderer2.materials = ownmats;
			SkinnedMeshRenderer component2 = FindWeaponBoneRenderer(weapon.type, hero).GetComponent<SkinnedMeshRenderer>();
			component2.material = weaponmat;
		}
	}

	protected Crazy_Weapon FindWeapon(int id)
	{
		foreach (Crazy_Weapon weapon in weapons)
		{
			if (weapon.id == id)
			{
				return weapon;
			}
		}
		return null;
	}

	protected void EquipWeapon(int weaponid, GameObject _char)
	{
		Crazy_Weapon crazy_Weapon = FindWeapon(weaponid);
		GameObject gameObject = Object.Instantiate(Resources.Load(crazy_Weapon.loadpath)) as GameObject;
		gameObject.name = "Weapon";
		gameObject.transform.parent = FindWeaponBone(crazy_Weapon.type, _char).transform;
		gameObject.transform.localPosition = crazy_Weapon.modifyPos;
		gameObject.transform.localEulerAngles = crazy_Weapon.modifyAngle;
		_char.GetComponent<Animation>()["Idle_" + crazy_Weapon.type_name + "_UI_merge"].wrapMode = WrapMode.Loop;
		_char.GetComponent<Animation>().Play("Idle_" + crazy_Weapon.type_name + "_UI_merge");
	}

	protected GameObject FindWeaponBoneRenderer(Crazy_Weapon_Type cwt, GameObject person)
	{
		GameObject gameObject = FindWeaponBone(cwt, person);
		if (gameObject == null)
		{
			return null;
		}
		switch (cwt)
		{
		case Crazy_Weapon_Type.Sword:
		case Crazy_Weapon_Type.Hammer:
		case Crazy_Weapon_Type.Axe:
		case Crazy_Weapon_Type.Staff:
			return gameObject.transform.Find("Weapon/Bone01").gameObject;
		case Crazy_Weapon_Type.Bow:
			return gameObject.transform.Find("Weapon/Bone").gameObject;
		default:
			return null;
		}
	}

	protected GameObject FindWeaponBone(Crazy_Weapon_Type cwt, GameObject person)
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

	private void Update()
	{
	}
}
