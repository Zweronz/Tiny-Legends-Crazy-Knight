using System.Collections.Generic;
using UnityEngine;

public class UtilUIHeroBoard : MonoBehaviour
{
	public List<GameObject> heros;

	public List<int> weaponidlist;

	protected GameObject sample;

	public Crazy_UIButtonSelectGroup container;

	public Vector3[] itemposition;

	protected List<Crazy_Weapon> weapons = new List<Crazy_Weapon>();

	private void Start()
	{
		weapons = Crazy_Weapon.ReadWeaponInfo();
		sample = base.transform.Find("StateBoardSample").gameObject;
		for (int i = 1; i <= itemposition.Length; i++)
		{
			GameObject gameObject = Object.Instantiate(sample) as GameObject;
			gameObject.name = string.Format("StateBoard{0:D02}", i);
			gameObject.transform.parent = sample.transform.parent;
			gameObject.transform.localPosition = itemposition[i - 1];
			TUIButtonSelect component = gameObject.transform.Find("Button").GetComponent<TUIButtonSelect>();
			container.AddControl(component);
			Crazy_PlayerClass crazy_PlayerClass = (Crazy_PlayerClass)(i - 1);
			gameObject.SendMessage("updateClassState", crazy_PlayerClass, SendMessageOptions.DontRequireReceiver);
			EquipWeapon(weaponidlist[i - 1], heros[i - 1]);
			Crazy_Weapon crazy_Weapon = FindWeapon(weaponidlist[i - 1]);
			if (Crazy_Data.CurData().GetPlayerClass() == crazy_PlayerClass)
			{
				gameObject.transform.Find("Button").GetComponent<TUIButtonSelect>().SetSelected(true);
			}
			if (!Crazy_Data.CurData().GetUnlock(crazy_PlayerClass))
			{
				gameObject.transform.localPosition = new Vector3(1000f, 1000f, 0f);
				Transform transform = heros[i - 1].transform.Find("zhujue");
				SkinnedMeshRenderer skinnedMeshRenderer = transform.gameObject.GetComponent("SkinnedMeshRenderer") as SkinnedMeshRenderer;
				Material[] materials = skinnedMeshRenderer.materials;
				for (int j = 0; j < materials.Length; j++)
				{
					materials[j] = Object.Instantiate(Resources.Load("Textures/Character/Crazy" + crazy_PlayerClass.ToString() + "-Material #" + (j + 1))) as Material;
				}
				skinnedMeshRenderer.materials = materials;
				SkinnedMeshRenderer component2 = FindWeaponBoneRenderer(crazy_Weapon.type, heros[i - 1]).GetComponent<SkinnedMeshRenderer>();
				component2.material = Object.Instantiate(Resources.Load("Textures/Weapon/Crazy" + crazy_PlayerClass.ToString() + "-WeaponMaterial")) as Material;
			}
		}
	}

	private Crazy_Weapon FindWeapon(int id)
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

	private void EquipWeapon(int weaponid, GameObject _char)
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

	private GameObject FindWeaponBoneRenderer(Crazy_Weapon_Type cwt, GameObject person)
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

	private void Update()
	{
	}
}
