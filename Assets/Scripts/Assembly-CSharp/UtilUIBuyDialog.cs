using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilUIBuyDialog : MonoBehaviour
{
	public GameObject buybutton;

	public GameObject equipbutton;

	public GameObject crystalbutton;

	protected GameObject item;

	protected Crazy_Weapon buy_weapon;

	private List<Color> namecolor;

	private void Start()
	{
		item = base.transform.Find("Dialog").Find("Item").gameObject;
		namecolor = new List<Color>();
		Color color = new Color(49f / 85f, 43f / 85f, 1f / 51f, 1f);
		namecolor.Add(color);
		color = new Color(0.11764706f, 1f, 0f, 1f);
		namecolor.Add(color);
		color = new Color(0f, 0.44313726f, 74f / 85f, 1f);
		namecolor.Add(color);
		color = new Color(0.64705884f, 16f / 85f, 0.9372549f, 1f);
		namecolor.Add(color);
	}

	private void Update()
	{
	}

	private void OnBuyWeapon(Crazy_Weapon weapon)
	{
		OnUpdateDialog(weapon);
		buy_weapon = weapon;
		if (CheckMoney())
		{
			buybutton.GetComponent<TUIButtonClick>().Reset();
			buybutton.GetComponent<TUIButtonClick>().SetDisabled(false);
			buybutton.transform.localPosition = new Vector3(68f, -38f, buybutton.transform.localPosition.z);
			equipbutton.transform.localPosition = new Vector3(1000f, 0f, equipbutton.transform.localPosition.z);
			crystalbutton.transform.localPosition = new Vector3(1000f, 0f, crystalbutton.transform.localPosition.z);
		}
		else
		{
			crystalbutton.transform.localPosition = new Vector3(68f, -38f, crystalbutton.transform.localPosition.z);
			buybutton.transform.localPosition = new Vector3(1000f, 0f, buybutton.transform.localPosition.z);
			equipbutton.transform.localPosition = new Vector3(1000f, 0f, equipbutton.transform.localPosition.z);
		}
	}

	private void OnEquipWeapon(Crazy_Weapon weapon)
	{
		OnUpdateDialog(weapon);
		buy_weapon = weapon;
		equipbutton.GetComponent<TUIButtonClick>().Reset();
		equipbutton.GetComponent<TUIButtonClick>().SetDisabled(false);
		equipbutton.transform.localPosition = new Vector3(68f, -38f, equipbutton.transform.localPosition.z);
		buybutton.transform.localPosition = new Vector3(1000f, 0f, buybutton.transform.localPosition.z);
		crystalbutton.transform.localPosition = new Vector3(1000f, 0f, crystalbutton.transform.localPosition.z);
	}

	private void OnPreviewEquipWeapon(Crazy_Weapon weapon)
	{
		OnUpdateDialog(weapon);
		buy_weapon = weapon;
		equipbutton.GetComponent<TUIButtonClick>().Reset();
		equipbutton.GetComponent<TUIButtonClick>().SetDisabled(true);
		equipbutton.transform.localPosition = new Vector3(68f, -38f, equipbutton.transform.localPosition.z);
		buybutton.transform.localPosition = new Vector3(1000f, 0f, buybutton.transform.localPosition.z);
		crystalbutton.transform.localPosition = new Vector3(1000f, 0f, crystalbutton.transform.localPosition.z);
	}

	private void OnPreviewWeapon(Crazy_Weapon weapon)
	{
		OnUpdateDialog(weapon);
		buy_weapon = weapon;
		buybutton.transform.localPosition = new Vector3(1000f, 0f, buybutton.transform.localPosition.z);
		equipbutton.transform.localPosition = new Vector3(1000f, 0f, equipbutton.transform.localPosition.z);
		crystalbutton.transform.localPosition = new Vector3(1000f, 0f, crystalbutton.transform.localPosition.z);
	}

	private void OnBuyButtonClick()
	{
		if (PayMoney())
		{
			BuyWeaponSuccess();
		}
	}

	private void OnEquipButtonClick()
	{
		EquipWeaponSuccess();
	}

	private bool CheckMoney()
	{
		switch (buy_weapon.price_type)
		{
		case Crazy_Price_Type.Crystal:
			if (buy_weapon.price <= Crazy_Data.CurData().GetCrystal())
			{
				return true;
			}
			return false;
		case Crazy_Price_Type.Gold:
			if (buy_weapon.price <= Crazy_Data.CurData().GetMoney())
			{
				return true;
			}
			return false;
		default:
			return false;
		}
	}

	private bool PayMoney()
	{
		switch (buy_weapon.price_type)
		{
		case Crazy_Price_Type.Crystal:
			if (buy_weapon.price <= Crazy_Data.CurData().GetCrystal())
			{
				Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() - buy_weapon.price);
				return true;
			}
			return false;
		case Crazy_Price_Type.Gold:
			if (buy_weapon.price <= Crazy_Data.CurData().GetMoney())
			{
				Crazy_Data.CurData().SetMoney(Crazy_Data.CurData().GetMoney() - buy_weapon.price);
				Crazy_GlobalData.cur_used_money += buy_weapon.price;
				return true;
			}
			return false;
		default:
			return false;
		}
	}

	private void EquipWeaponSuccess()
	{
		Crazy_Data.CurData().SetWeaponId(buy_weapon.id);
		Crazy_Data.SaveData();
		SendMessageUpwards("UpdateWeaponState", SendMessageOptions.DontRequireReceiver);
	}

	private void BuyWeaponSuccess()
	{
		if (buy_weapon.price_type == Crazy_Price_Type.Crystal)
		{
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task20, 0, 0f);
		}
		Crazy_Data.CurData().SetWeapon(buy_weapon.id, true);
		Crazy_Data.CurData().SetWeaponId(buy_weapon.id);
		Crazy_Data.SaveData();
		Hashtable hashtable = new Hashtable();
		hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
		hashtable.Add("WeaponID", buy_weapon.id);
		FlurryPlugin.logEvent("Buy_Weapon", hashtable);
		SendMessageUpwards("UpdateWeaponState", SendMessageOptions.DontRequireReceiver);
	}

	private void OnUpdateDialog(Crazy_Weapon weapon)
	{
		TUIMeshText component = item.transform.Find("TextName").GetComponent<TUIMeshText>();
		component.text = weapon.name;
		component.color = namecolor[weapon.level];
		component.UpdateMesh();
		TUIMeshSprite component2 = item.transform.Find("Icon").GetComponent<TUIMeshSprite>();
		component2.frameName = weapon.iconname;
		component2.UpdateMesh();
		TUIMeshText component3 = item.transform.Find("Level").Find("TextLevelData").GetComponent<TUIMeshText>();
		component3.text = weapon.need.ToString();
		component3.UpdateMesh();
		TUIMeshText component4 = item.transform.Find("Damage").Find("TextDamageData").GetComponent<TUIMeshText>();
		component4.text = weapon.damage.ToString();
		component4.UpdateMesh();
		TUIMeshText component5 = item.transform.Find("Speed").Find("TextSpeedData").GetComponent<TUIMeshText>();
		component5.text = ((int)(weapon.move * 10f)).ToString();
		component5.UpdateMesh();
		TUIMeshText component6 = item.transform.Find("Money").Find("MoneyData").GetComponent<TUIMeshText>();
		if (Crazy_Data.CurData().GetWeapon()[weapon.id])
		{
			component6.text = string.Empty;
		}
		else
		{
			component6.text = weapon.price.ToString();
		}
		component6.UpdateMesh();
		TUIMeshSprite component7 = item.transform.Find("Money").Find("MoneyType").GetComponent<TUIMeshSprite>();
		if (Crazy_Data.CurData().GetWeapon()[weapon.id])
		{
			component7.frameName = string.Empty;
		}
		else if (weapon.price_type == Crazy_Price_Type.Crystal)
		{
			component7.frameName = "CrystalBig";
		}
		else
		{
			component7.frameName = "GoldBig";
		}
		component7.UpdateMesh();
		UtilUISkillBoard component8 = item.transform.Find("SkillBoard").GetComponent<UtilUISkillBoard>();
		component8.UpdateSkill(weapon.skill);
	}
}
