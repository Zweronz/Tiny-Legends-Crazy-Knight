using System.Collections.Generic;
using UnityEngine;

public class UtilUIStateBoard : MonoBehaviour
{
	protected TUIMeshText lv;

	protected UtilUIBloodBoard blood;

	protected TUIMeshText damage;

	protected TUIMeshText speed;

	private float class_damage_rate = 1f;

	private float class_speed_rate;

	private List<Crazy_Weapon> weapons;

	private void Start()
	{
		lv = base.transform.Find("LV/Text").GetComponent<TUIMeshText>();
		blood = base.transform.Find("BloodBoard").GetComponent("UtilUIBloodBoard") as UtilUIBloodBoard;
		damage = base.transform.Find("State/Damage/TextDamageData").GetComponent<TUIMeshText>();
		speed = base.transform.Find("State/Speed/TextSpeedData").GetComponent<TUIMeshText>();
		weapons = Crazy_Weapon.ReadWeaponInfo();
	}

	private void updateLv()
	{
		lv.text = "L" + Crazy_Data.CurData().GetLevel();
		lv.UpdateMesh();
	}

	private void updateClassFactor(Crazy_Weapon cur_weapon)
	{
		switch (Crazy_Data.CurData().GetPlayerClass())
		{
		case Crazy_PlayerClass.Fighter:
			switch (cur_weapon.type)
			{
			case Crazy_Weapon_Type.Sword:
				class_damage_rate = 1.1f;
				class_speed_rate = 0.5f;
				break;
			case Crazy_Weapon_Type.Hammer:
			case Crazy_Weapon_Type.Axe:
				class_damage_rate = 1f;
				class_speed_rate = -0.5f;
				break;
			}
			break;
		case Crazy_PlayerClass.Knight:
			switch (cur_weapon.type)
			{
			case Crazy_Weapon_Type.Sword:
				class_damage_rate = 0.9f;
				class_speed_rate = 0f;
				break;
			case Crazy_Weapon_Type.Axe:
				class_damage_rate = 1.1f;
				class_speed_rate = 0f;
				break;
			case Crazy_Weapon_Type.Hammer:
				class_damage_rate = 1f;
				class_speed_rate = 0f;
				break;
			}
			break;
		case Crazy_PlayerClass.Warrior:
			switch (cur_weapon.type)
			{
			case Crazy_Weapon_Type.Sword:
				class_damage_rate = 0.8f;
				class_speed_rate = 0f;
				break;
			case Crazy_Weapon_Type.Axe:
				class_damage_rate = 1f;
				class_speed_rate = 0f;
				break;
			case Crazy_Weapon_Type.Hammer:
				class_damage_rate = 1.1f;
				class_speed_rate = 0.5f;
				break;
			}
			break;
		case Crazy_PlayerClass.Rogue:
			switch (cur_weapon.type)
			{
			case Crazy_Weapon_Type.Sword:
			case Crazy_Weapon_Type.Hammer:
			case Crazy_Weapon_Type.Axe:
				class_damage_rate = 0.95f;
				break;
			case Crazy_Weapon_Type.Bow:
				class_damage_rate = 1.1f;
				break;
			}
			break;
		case Crazy_PlayerClass.Paladin:
			class_damage_rate = 1.1f;
			class_speed_rate = 0f;
			break;
		case Crazy_PlayerClass.Mage:
			class_damage_rate = 1f;
			class_speed_rate = 0f;
			break;
		default:
			Debug.LogError("ClassError");
			break;
		}
	}

	private Crazy_Weapon GetCurWeaponInfo()
	{
		return Crazy_Weapon.FindWeaponByID(weapons, Crazy_Data.CurData().GetWeaponId());
	}

	private void updateStateData(Crazy_Weapon weapon)
	{
		updateClassFactor(weapon);
		int num = (int)((float)(weapon.damage + Crazy_PlayerClass_Level.GetPlayerLevelinfo(Crazy_Data.CurData().GetLevel()).damage) * class_damage_rate);
		int num2 = (int)((weapon.move + class_speed_rate) * 10f);
		Crazy_Weapon curWeaponInfo = GetCurWeaponInfo();
		updateClassFactor(curWeaponInfo);
		int num3 = (int)((float)(curWeaponInfo.damage + Crazy_PlayerClass_Level.GetPlayerLevelinfo(Crazy_Data.CurData().GetLevel()).damage) * class_damage_rate);
		int num4 = (int)((curWeaponInfo.move + class_speed_rate) * 10f);
		damage.text = num + " (" + ((num <= num3) ? string.Empty : "+") + (num - num3) + ")";
		speed.text = num2 + " (" + ((num2 <= num4) ? string.Empty : "+") + (num2 - num4) + ")";
		if (num > num3)
		{
			damage.fontName = "SFSCR";
		}
		else if (num < num3)
		{
			damage.fontName = "SFSCG";
		}
		else
		{
			damage.fontName = "SFSC";
		}
		if (num2 > num4)
		{
			speed.fontName = "SFSCR";
		}
		else if (num2 < num4)
		{
			speed.fontName = "SFSCG";
		}
		else
		{
			speed.fontName = "SFSC";
		}
		damage.UpdateMesh();
		speed.UpdateMesh();
	}

	private void updateState(Crazy_Weapon weapon)
	{
		updateStateData(weapon);
		if (weapon.skill != null)
		{
			foreach (Crazy_WeaponSkill item in weapon.skill)
			{
				if (item.GetType() == Crazy_WeaponSkillType.HealUp)
				{
					blood.UpdateBlood((int)item.GetParam() + 3);
					return;
				}
			}
		}
		blood.UpdateBlood(3);
	}

	private void Update()
	{
		updateLv();
	}
}
