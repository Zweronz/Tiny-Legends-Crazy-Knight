using System.Collections.Generic;
using TNetSdk;
using UnityEngine;

public class UtilUIReward : MonoBehaviour
{
	private GameObject item;

	private void Start()
	{
		item = base.transform.Find("ItemPrefab").gameObject;
		UpdateRewardInfo();
	}

	protected void UpdateRewardInfo()
	{
		Dictionary<TNetUser, List<int>> netitem = Crazy_GlobalData_Net.Instance.netitem;
		int num = 0;
		List<TNetUser> list = new List<TNetUser>();
		foreach (TNetUser key in netitem.Keys)
		{
			if (TNetManager.Connection != null && key == TNetManager.Connection.Myself)
			{
				list.Insert(0, key);
			}
			else
			{
				list.Add(key);
			}
		}
		foreach (TNetUser item in list)
		{
			GameObject gameObject = Object.Instantiate(this.item) as GameObject;
			gameObject.name = "Info" + num;
			gameObject.transform.parent = this.item.transform.parent;
			gameObject.transform.localPosition = new Vector3(0f, -18 - num * 40, -1f);
			TUIMeshSprite component = gameObject.transform.Find("HeroClass").GetComponent<TUIMeshSprite>();
			if (Crazy_GlobalData_Net.Instance.userlocalinfo.ContainsKey(item))
			{
				component.frameName = Crazy_GlobalData_Net.Instance.userlocalinfo[item].type.ToString();
			}
			else
			{
				component.frameName = Crazy_Data.CurData().GetPlayerClass().ToString();
			}
			component.UpdateMesh();
			TUIMeshText component2 = gameObject.transform.Find("HeroName").GetComponent<TUIMeshText>();
			if (Crazy_GlobalData_Net.Instance.userlocalinfo.ContainsKey(item))
			{
				component2.text = Crazy_GlobalData_Net.Instance.userlocalinfo[item].name;
			}
			else
			{
				component2.text = item.Name;
			}
			component2.UpdateMesh();
			TUIMeshText component3 = gameObject.transform.Find("GoldText").GetComponent<TUIMeshText>();
			component3.text = GetGold(netitem[item]).ToString();
			component3.UpdateMesh();
			TUIMeshText component4 = gameObject.transform.Find("CrystalText").GetComponent<TUIMeshText>();
			component4.text = GetCrystal(netitem[item]).ToString();
			component4.UpdateMesh();
			GameObject gameObject2 = gameObject.transform.Find("WeaponIcons/WeaponItemPrefab").gameObject;
			List<int> weapons = GetWeapons(netitem[item]);
			int num2 = 0;
			foreach (int item2 in weapons)
			{
				GameObject gameObject3 = Object.Instantiate(gameObject2) as GameObject;
				gameObject3.name = "Weapon" + num2;
				gameObject3.transform.parent = gameObject2.transform.parent;
				gameObject3.transform.localPosition = new Vector3(26 * num2, 0f, 0f);
				TUIMeshSprite component5 = gameObject3.GetComponent<TUIMeshSprite>();
				component5.frameName = Crazy_Weapon.FindWeaponByID(Crazy_Weapon.ReadWeaponInfo(), item2).iconname;
				component5.UpdateMesh();
				num2++;
				if (num2 == 4)
				{
					break;
				}
			}
			num++;
		}
		netitem.Clear();
		TNetManager.ManualDisconnect();
	}

	protected List<int> GetWeapons(List<int> t)
	{
		List<int> list = new List<int>();
		foreach (int item in t)
		{
			Crazy_Award awardInfo = Crazy_Award.GetAwardInfo(item);
			foreach (Crazy_Award_Item item2 in awardInfo.item)
			{
				if (item2.type == Crazy_Award_Item_Type.Weapon)
				{
					list.Add(item2.id);
				}
			}
		}
		return list;
	}

	protected int GetGold(List<int> t)
	{
		int num = 0;
		foreach (int item in t)
		{
			Crazy_Award awardInfo = Crazy_Award.GetAwardInfo(item);
			foreach (Crazy_Award_Item item2 in awardInfo.item)
			{
				if (item2.type == Crazy_Award_Item_Type.Currency && item2.id == 0)
				{
					num += item2.count;
				}
			}
		}
		return num;
	}

	protected int GetCrystal(List<int> t)
	{
		int num = 0;
		foreach (int item in t)
		{
			Crazy_Award awardInfo = Crazy_Award.GetAwardInfo(item);
			foreach (Crazy_Award_Item item2 in awardInfo.item)
			{
				if (item2.type == Crazy_Award_Item_Type.Currency && item2.id == 1)
				{
					num += item2.count;
				}
			}
		}
		return num;
	}

	private void Update()
	{
	}
}
