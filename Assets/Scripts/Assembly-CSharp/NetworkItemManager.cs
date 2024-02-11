using System.Collections.Generic;
using TNetSdk;
using UnityEngine;

public class NetworkItemManager : MonoBehaviour
{
	private static NetworkItemManager instance;

	public Dictionary<int, GameObject> item = new Dictionary<int, GameObject>();

	private Dictionary<int, Crazy_Package_Item> cur_droplist = new Dictionary<int, Crazy_Package_Item>();

	public GameObject itemfx;

	public static NetworkItemManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("NetworkItemManager").AddComponent<NetworkItemManager>();
			}
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		item.Clear();
	}

	private void Start()
	{
	}

	protected GameObject WeaponDrop(int id, ref string audioname)
	{
		if (Crazy_Data.CurData().GetWeapon()[id])
		{
			return null;
		}
		Crazy_Weapon crazy_Weapon = Crazy_Weapon.FindWeaponByID(Crazy_Weapon.ReadWeaponInfo(), id);
		GameObject gameObject = Object.Instantiate(Resources.Load(crazy_Weapon.loadpath)) as GameObject;
		AnimationClip clip = Object.Instantiate(Resources.Load("Anim/float_w")) as AnimationClip;
		Animation animation = gameObject.AddComponent<Animation>();
		animation.AddClip(clip, "Float");
		animation.Stop();
		GameObject gameObject2 = new GameObject("Item");
		gameObject2.transform.position = new Vector3(5000f, 0f, 5000f);
		gameObject.transform.parent = gameObject2.transform;
		gameObject.transform.localPosition = new Vector3(0f, 1.45f, 0f);
		gameObject.transform.localEulerAngles = new Vector3(-17f, -12f, 50f);
		gameObject2.AddComponent<Crazy_ItemTrigger>();
		audioname = "UI_Button_Eq" + crazy_Weapon.type_name;
		Crazy_Weapon_Type type = crazy_Weapon.type;
		GameObject gameObject3 = ((type != Crazy_Weapon_Type.Bow) ? (Object.Instantiate(Resources.Load("Prefabs/ItemEffect/Item_W")) as GameObject) : (Object.Instantiate(Resources.Load("Prefabs/ItemEffect/Item_B")) as GameObject));
		gameObject3.transform.parent = gameObject2.transform;
		gameObject3.transform.localPosition = Vector3.zero;
		return gameObject2;
	}

	protected GameObject GoldDrop()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("FBX/item/gold/gold_pfb")) as GameObject;
		AnimationClip clip = Object.Instantiate(Resources.Load("Anim/float")) as AnimationClip;
		Animation animation = gameObject.AddComponent<Animation>();
		animation.AddClip(clip, "Float");
		animation.Stop();
		GameObject gameObject2 = new GameObject("Item");
		gameObject2.transform.position = new Vector3(5000f, 0f, 5000f);
		gameObject.transform.parent = gameObject2.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject2.AddComponent<Crazy_ItemTrigger>();
		GameObject gameObject3 = Object.Instantiate(Resources.Load("Prefabs/ItemEffect/Item_G")) as GameObject;
		gameObject3.transform.parent = gameObject2.transform;
		gameObject3.transform.localPosition = Vector3.zero;
		return gameObject2;
	}

	protected GameObject GoldBigDrop()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("FBX/item/gold/gold_000_pfb")) as GameObject;
		AnimationClip clip = Object.Instantiate(Resources.Load("Anim/float")) as AnimationClip;
		Animation animation = gameObject.AddComponent<Animation>();
		animation.AddClip(clip, "Float");
		animation.Stop();
		GameObject gameObject2 = new GameObject("Item");
		gameObject2.transform.position = new Vector3(5000f, 0f, 5000f);
		gameObject.transform.parent = gameObject2.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject2.AddComponent<Crazy_ItemTrigger>();
		GameObject gameObject3 = Object.Instantiate(Resources.Load("Prefabs/ItemEffect/Item_GB")) as GameObject;
		gameObject3.transform.parent = gameObject2.transform;
		gameObject3.transform.localPosition = Vector3.zero;
		return gameObject2;
	}

	protected GameObject CrystalSmallDrop()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("FBX/item/crystal/crystal_000_pfb")) as GameObject;
		AnimationClip clip = Object.Instantiate(Resources.Load("Anim/float")) as AnimationClip;
		Animation animation = gameObject.AddComponent<Animation>();
		animation.AddClip(clip, "Float");
		animation.Stop();
		GameObject gameObject2 = new GameObject("Item");
		gameObject2.transform.position = new Vector3(5000f, 0f, 5000f);
		gameObject.transform.parent = gameObject2.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject2.AddComponent<Crazy_ItemTrigger>();
		GameObject gameObject3 = Object.Instantiate(Resources.Load("Prefabs/ItemEffect/Item_C")) as GameObject;
		gameObject3.transform.parent = gameObject2.transform;
		gameObject3.transform.localPosition = Vector3.zero;
		return gameObject2;
	}

	protected GameObject CrystalBigDrop()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load("FBX/item/crystal/crystal_001_pfb")) as GameObject;
		AnimationClip clip = Object.Instantiate(Resources.Load("Anim/float")) as AnimationClip;
		Animation animation = gameObject.AddComponent<Animation>();
		animation.AddClip(clip, "Float");
		animation.Stop();
		GameObject gameObject2 = new GameObject("Item");
		gameObject2.transform.position = new Vector3(5000f, 0f, 5000f);
		gameObject.transform.parent = gameObject2.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject2.AddComponent<Crazy_ItemTrigger>();
		GameObject gameObject3 = Object.Instantiate(Resources.Load("Prefabs/ItemEffect/Item_C")) as GameObject;
		gameObject3.transform.parent = gameObject2.transform;
		gameObject3.transform.localPosition = Vector3.zero;
		return gameObject2;
	}

	private void PlayFx(GameObject citem)
	{
		if (itemfx == null)
		{
			itemfx = new GameObject("ItemFx");
			itemfx.AddComponent<Crazy_PlayTAudio>();
		}
		itemfx.transform.position = citem.transform.position;
		Crazy_PlayTAudio component = itemfx.GetComponent<Crazy_PlayTAudio>();
		component.audioname = citem.GetComponent<Crazy_ItemInfo>().audioname;
		component.Play();
	}

	public void SendColliderItem(GameObject citem)
	{
		PlayFx(citem);
		Crazy_ItemInfo component = citem.GetComponent<Crazy_ItemInfo>();
		GameObject value = null;
		if (item.TryGetValue(component.netid, out value))
		{
			item.Remove(component.netid);
		}
		Object.Destroy(citem);
		NetworkManager.Instance.SendLock(component.netid);
	}

	public void UpdateGetItem(TNetUser user, ItemInfo info)
	{
		AddItemNote(user, info.id);
		GameObject value = null;
		if (item.TryGetValue(info.id, out value))
		{
			item.Remove(info.id);
			if (value != null)
			{
				Object.Destroy(value);
			}
		}
	}

	public void AddItemNote(TNetUser user, int id)
	{
		List<int> value;
		if (Crazy_GlobalData_Net.Instance.netitem.TryGetValue(user, out value))
		{
			value.Add(cur_droplist[id].awardid);
			return;
		}
		value = new List<int>();
		value.Add(cur_droplist[id].awardid);
		Crazy_GlobalData_Net.Instance.netitem.Add(user, value);
	}

	public void GetItemSuccess(TNetUser user, int id)
	{
		DoItem(id);
		AddItemNote(user, id);
		NetworkManager.Instance.SendGetItem(new ItemInfo(id));
	}

	protected void DoItem(int id)
	{
		Crazy_Global.Prize(cur_droplist[id].awardid);
	}

	public void GetItemFailed(TNetUser user, int id)
	{
	}

	public List<GameObject> InitDrop(Dictionary<int, Crazy_Package_Item> cur_drop)
	{
		cur_droplist.Clear();
		foreach (int key in cur_drop.Keys)
		{
			cur_droplist.Add(key, cur_drop[key]);
		}
		List<GameObject> list = new List<GameObject>();
		foreach (int key2 in cur_drop.Keys)
		{
			switch (cur_drop[key2].type)
			{
			case Crazy_Award_Item_Type.Currency:
				switch (cur_drop[key2].typeid)
				{
				case 0:
				{
					GameObject gameObject5 = GoldDrop();
					Crazy_ItemInfo crazy_ItemInfo5 = gameObject5.AddComponent<Crazy_ItemInfo>();
					crazy_ItemInfo5.awardid = cur_drop[key2].awardid;
					crazy_ItemInfo5.netid = key2;
					crazy_ItemInfo5.audioname = "FX_Coin_pickup01";
					list.Add(gameObject5);
					item.Add(key2, gameObject5);
					break;
				}
				case 1:
				{
					GameObject gameObject4 = CrystalSmallDrop();
					Crazy_ItemInfo crazy_ItemInfo4 = gameObject4.AddComponent<Crazy_ItemInfo>();
					crazy_ItemInfo4.awardid = cur_drop[key2].awardid;
					crazy_ItemInfo4.netid = key2;
					crazy_ItemInfo4.audioname = "FX_Crystal_pickup01";
					list.Add(gameObject4);
					item.Add(key2, gameObject4);
					break;
				}
				case 2:
				{
					GameObject gameObject3 = CrystalBigDrop();
					Crazy_ItemInfo crazy_ItemInfo3 = gameObject3.AddComponent<Crazy_ItemInfo>();
					crazy_ItemInfo3.awardid = cur_drop[key2].awardid;
					crazy_ItemInfo3.netid = key2;
					crazy_ItemInfo3.audioname = "FX_Crystal_pickup02";
					list.Add(gameObject3);
					item.Add(key2, gameObject3);
					break;
				}
				case 3:
				{
					GameObject gameObject2 = GoldBigDrop();
					Crazy_ItemInfo crazy_ItemInfo2 = gameObject2.AddComponent<Crazy_ItemInfo>();
					crazy_ItemInfo2.awardid = cur_drop[key2].awardid;
					crazy_ItemInfo2.netid = key2;
					crazy_ItemInfo2.audioname = "FX_Coin_pickup02";
					list.Add(gameObject2);
					item.Add(key2, gameObject2);
					break;
				}
				}
				break;
			case Crazy_Award_Item_Type.Weapon:
			{
				string audioname = string.Empty;
				GameObject gameObject = WeaponDrop(cur_drop[key2].typeid, ref audioname);
				if (gameObject == null)
				{
					list.Add(gameObject);
					item.Add(key2, gameObject);
					break;
				}
				Crazy_ItemInfo crazy_ItemInfo = gameObject.AddComponent<Crazy_ItemInfo>();
				crazy_ItemInfo.awardid = cur_drop[key2].awardid;
				crazy_ItemInfo.netid = key2;
				crazy_ItemInfo.audioname = audioname;
				list.Add(gameObject);
				item.Add(key2, gameObject);
				break;
			}
			}
		}
		return list;
	}
}
