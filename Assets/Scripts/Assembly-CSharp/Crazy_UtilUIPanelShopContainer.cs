using Assets.Scripts.Assembly_CSharp.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Crazy_UtilUIPanelShopContainer : TUIContainer
{
	[Serializable]
	public class DialogInfo
	{
		public GameObject dialog;

		public Vector3 showPosition;

		public Vector3 hidePosition;

		public void Show()
		{
			dialog.transform.localPosition = showPosition;
		}

		public void Hide()
		{
			dialog.transform.localPosition = hidePosition;
		}
	}

	public UtilUIStateBoard state;

	protected GameObject weaponObj;

	protected GameObject m_char;

	protected List<Crazy_Weapon> weapons = new List<Crazy_Weapon>();

	public TUIButtonBase[] button;

	protected int show_weapon_id = -1;

	public DialogInfo dialog;

	public List<GameObject> weaponstate;

	public GameObject cannotdialog;

	private new void Start()
	{
		switch (Crazy_Data.CurData().GetPlayerClass())
		{
		case Crazy_PlayerClass.Fighter:
			m_char = GameObject.Find("UICrazyFighter");
			break;
		case Crazy_PlayerClass.Knight:
			m_char = GameObject.Find("UICrazyKnight");
			break;
		case Crazy_PlayerClass.Warrior:
			m_char = GameObject.Find("UICrazyWarrior");
			break;
		case Crazy_PlayerClass.Rogue:
			m_char = GameObject.Find("UICrazyRogue");
			break;
		case Crazy_PlayerClass.Paladin:
			m_char = GameObject.Find("UICrazyPaladin");
			break;
		case Crazy_PlayerClass.Mage:
			m_char = GameObject.Find("UICrazyMage");
			break;
		default:
			Debug.LogError("CharError");
			break;
		}
		m_char.transform.localPosition = new Vector3(-4.14f, -1f, 7.14f);
		m_char.transform.localEulerAngles = new Vector3(10f, 161.81f, 0f);
		if (Crazy_Data.CurData().GetPlayerClass() == Crazy_PlayerClass.Mage)
		{
			m_char.GetComponent<Animation>()["Idle_Staff_UI_merge"].wrapMode = WrapMode.Loop;
			m_char.GetComponent<Animation>()["Idle_Staff_UI_merge"].layer = 0;
			m_char.GetComponent<Animation>()["Idle_Staff01_celebrate_merge"].wrapMode = WrapMode.Once;
			m_char.GetComponent<Animation>()["Idle_Staff01_celebrate_merge"].layer = 1;
		}
		else
		{
			m_char.GetComponent<Animation>()["Idle_Axe_UI_merge"].wrapMode = WrapMode.Loop;
			m_char.GetComponent<Animation>()["Idle_Sword_UI_merge"].wrapMode = WrapMode.Loop;
			m_char.GetComponent<Animation>()["Idle_Hammer_UI_merge"].wrapMode = WrapMode.Loop;
			m_char.GetComponent<Animation>()["Idle_Bow_UI_merge"].wrapMode = WrapMode.Loop;
			m_char.GetComponent<Animation>()["Idle_Axe_UI_merge"].layer = 0;
			m_char.GetComponent<Animation>()["Idle_Sword_UI_merge"].layer = 0;
			m_char.GetComponent<Animation>()["Idle_Hammer_UI_merge"].layer = 0;
			m_char.GetComponent<Animation>()["Idle_Bow_UI_merge"].layer = 0;
			m_char.GetComponent<Animation>()["Idle_Axe01_celebrate_merge"].wrapMode = WrapMode.Once;
			m_char.GetComponent<Animation>()["Idle_Sword01_celebrate_merge"].wrapMode = WrapMode.Once;
			m_char.GetComponent<Animation>()["Idle_Hammer01_celebrate_merge"].wrapMode = WrapMode.Once;
			m_char.GetComponent<Animation>()["Idle_Bow01_celebrate_merge"].wrapMode = WrapMode.Once;
			m_char.GetComponent<Animation>()["Idle_Axe01_celebrate_merge"].layer = 1;
			m_char.GetComponent<Animation>()["Idle_Sword01_celebrate_merge"].layer = 1;
			m_char.GetComponent<Animation>()["Idle_Hammer01_celebrate_merge"].layer = 1;
			m_char.GetComponent<Animation>()["Idle_Bow01_celebrate_merge"].layer = 1;
		}
		weapons = Crazy_Weapon.ReadWeaponInfo();
		EquipWeaponOriginal();
		dialog.Hide();
	}

	private void Update()
	{
	}

	protected void ShowDialog(string str)
	{
		cannotdialog.transform.localPosition = new Vector3(0f, 0f, cannotdialog.transform.localPosition.z);
		TUIMeshText component = cannotdialog.transform.Find("Text").GetComponent<TUIMeshText>();
		component.text = str;
		component.UpdateMesh();
	}

	protected void HideDialog()
	{
		cannotdialog.transform.localPosition = new Vector3(1000f, 1000f, cannotdialog.transform.localPosition.z);
	}

	public override void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "Button" && eventType == 3)
		{
			string text = control.transform.parent.gameObject.name;
			string str = string.Empty;
			foreach (Crazy_Weapon weapon in weapons)
			{
				if ("Weapon" + weapon.id == text)
				{
					if (CheckWeapon(weapon, ref str))
					{
						EquipWeaponDemo(weapon);
					}
					else
					{
						ShowDialog(str);
					}
				}
			}
		}
		if (control.name == "MoveTrack" && eventType == 2)
		{
			if (wparam < -0.1f)
			{
				m_char.transform.Rotate(Vector3.up, 16f);
			}
			else if (wparam > 0.1f)
			{
				m_char.transform.Rotate(Vector3.up, -16f);
			}
		}
		if (control.name == "BuyButton" && eventType == 3)
		{
			if (dialog != null)
			{
				dialog.Hide();
			}
			EquipWeaponOriginal();
			OnEndBuy();
		}
		if (control.name == "EquipButton" && eventType == 3)
		{
			if (dialog != null)
			{
				dialog.Hide();
			}
			EquipWeaponOriginal();
			OnEndBuy();
		}
		if (control.name == "CancelButton" && eventType == 3)
		{
			if (dialog != null)
			{
				dialog.Hide();
			}
			EquipWeaponOriginal();
			OnEndBuy();
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
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

	private bool CheckWeapon(Crazy_Weapon weapon, ref string str)
	{
		if (weapon.type != Crazy_Weapon_Type.Staff && Crazy_Data.CurData().GetPlayerClass() == Crazy_PlayerClass.Mage)
		{
			str = "The mage can only equip staffs!";
			return false;
		}
		if (weapon.type == Crazy_Weapon_Type.Staff && Crazy_Data.CurData().GetPlayerClass() != Crazy_PlayerClass.Mage)
		{
			str = "Only Merrill can equip staffs!";
			return false;
		}
		return true;
	}

	private void EquipWeaponDemo(Crazy_Weapon weapon)
	{
		if (weaponObj != null)
		{
			UnityEngine.Object.Destroy(weaponObj);
		}
		weaponObj = GameUtils.InstantiateAsGameObject<GameObject>(Resources.Load(weapon.loadpath)) as GameObject;
		weaponObj.name = "Weapon";
		weaponObj.transform.parent = FindWeaponBone(weapon.type, m_char).transform;
		weaponObj.transform.localPosition = weapon.modifyPos;
		weaponObj.transform.localEulerAngles = weapon.modifyAngle;
		m_char.GetComponent<Animation>().Play("Idle_" + weapon.type_name + "_UI_merge");
		if (show_weapon_id != weapon.id)
		{
			m_char.GetComponent<Animation>().Play("Idle_" + weapon.type_name + "01_celebrate_merge");
			show_weapon_id = weapon.id;
		}
		state.SendMessage("updateState", weapon, SendMessageOptions.DontRequireReceiver);
		if (weaponstate == null)
		{
			return;
		}
		foreach (GameObject item in weaponstate)
		{
			item.SendMessage("UpdateWeaponInfo", SendMessageOptions.DontRequireReceiver);
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

	private void OnEndBuy()
	{
		TUIButtonBase[] array = button;
		foreach (TUIButtonBase tUIButtonBase in array)
		{
			tUIButtonBase.SetDisabled(false);
		}
	}

	private void OnBeginBuy()
	{
		TUIButtonBase[] array = button;
		foreach (TUIButtonBase tUIButtonBase in array)
		{
			tUIButtonBase.SetDisabled(true);
		}
	}

	private void OnBuyWeapon(Crazy_Weapon weapon)
	{
		if (dialog != null)
		{
			dialog.Show();
			dialog.dialog.SendMessage("OnBuyWeapon", weapon, SendMessageOptions.DontRequireReceiver);
		}
		OnBeginBuy();
	}

	private void OnEquipWeapon(Crazy_Weapon weapon)
	{
		if (dialog != null)
		{
			dialog.Show();
			dialog.dialog.SendMessage("OnEquipWeapon", weapon, SendMessageOptions.DontRequireReceiver);
		}
		OnBeginBuy();
	}

	private void OnPreviewEquipWeapon(Crazy_Weapon weapon)
	{
		if (dialog != null)
		{
			dialog.Show();
			dialog.dialog.SendMessage("OnPreviewEquipWeapon", weapon, SendMessageOptions.DontRequireReceiver);
		}
		OnBeginBuy();
	}

	private void OnPreviewWeapon(Crazy_Weapon weapon)
	{
		if (dialog != null)
		{
			dialog.Show();
			dialog.dialog.SendMessage("OnPreviewWeapon", weapon, SendMessageOptions.DontRequireReceiver);
		}
		OnBeginBuy();
	}

	private void UpdateWeaponState()
	{
		EquipWeaponOriginal();
	}
}
