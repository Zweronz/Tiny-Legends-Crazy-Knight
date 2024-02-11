using System.Collections.Generic;
using UnityEngine;

public class Crazy_UtilUIWeaponShopContainer : TUIContainer
{
	protected List<Crazy_Weapon> weapons = new List<Crazy_Weapon>();

	private new void Start()
	{
		weapons = Crazy_Weapon.ReadWeaponInfo();
	}

	private void Update()
	{
	}

	public override void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "Button" && eventType == 3)
		{
			string text = control.transform.parent.gameObject.name;
			foreach (Crazy_Weapon weapon in weapons)
			{
				if (!("Weapon" + weapon.id == text))
				{
					continue;
				}
				OnPlayAudio(weapon);
				List<int> list = new List<int>();
				foreach (int item in Crazy_GlobalData.unlock_weaponid)
				{
					if (item == weapon.id)
					{
						list.Add(item);
					}
				}
				foreach (int item2 in list)
				{
					Crazy_GlobalData.unlock_weaponid.Remove(item2);
				}
				if (CheckWeapon(weapon))
				{
					if (Crazy_Data.CurData().GetWeaponId() == weapon.id)
					{
						PreviewWeapon(weapon);
					}
					else if (Crazy_Data.CurData().GetWeapon()[weapon.id] && weapon.need <= Crazy_Data.CurData().GetLevel())
					{
						EquipWeapon(weapon);
					}
					else if (Crazy_Data.CurData().GetWeapon()[weapon.id] && weapon.need > Crazy_Data.CurData().GetLevel())
					{
						PreviewEquipWeapon(weapon);
					}
					else if (!Crazy_Data.CurData().GetWeapon()[weapon.id] && weapon.need <= Crazy_Data.CurData().GetLevel())
					{
						BuyWeapon(weapon);
					}
					else
					{
						PreviewWeapon(weapon);
					}
				}
			}
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
	}

	private bool CheckWeapon(Crazy_Weapon weapon)
	{
		return (weapon.type == Crazy_Weapon_Type.Staff && Crazy_Data.CurData().GetPlayerClass() == Crazy_PlayerClass.Mage) || (weapon.type != Crazy_Weapon_Type.Staff && Crazy_Data.CurData().GetPlayerClass() != Crazy_PlayerClass.Mage);
	}

	protected void PreviewEquipWeapon(Crazy_Weapon weapon)
	{
		SendMessageUpwards("OnPreviewEquipWeapon", weapon, SendMessageOptions.DontRequireReceiver);
	}

	protected void PreviewWeapon(Crazy_Weapon weapon)
	{
		SendMessageUpwards("OnPreviewWeapon", weapon, SendMessageOptions.DontRequireReceiver);
	}

	protected void BuyWeapon(Crazy_Weapon weapon)
	{
		SendMessageUpwards("OnBuyWeapon", weapon, SendMessageOptions.DontRequireReceiver);
	}

	protected void EquipWeapon(Crazy_Weapon weapon)
	{
		SendMessageUpwards("OnEquipWeapon", weapon, SendMessageOptions.DontRequireReceiver);
	}

	protected void OnPlayAudio(Crazy_Weapon weapon)
	{
		if (weapon.type == Crazy_Weapon_Type.Staff)
		{
			SendMessage("PlayAudio", "UI_Button_EqBow", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			SendMessage("PlayAudio", "UI_Button_Eq" + weapon.type_name.ToUpper(), SendMessageOptions.DontRequireReceiver);
		}
	}
}
