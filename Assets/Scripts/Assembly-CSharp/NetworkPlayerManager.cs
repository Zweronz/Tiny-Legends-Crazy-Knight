using System.Collections.Generic;
using TNetSdk;
using UnityEngine;

public class NetworkPlayerManager : MonoBehaviour
{
	private static NetworkPlayerManager instance;

	private Dictionary<TNetUser, GameObject> m_player_copy = new Dictionary<TNetUser, GameObject>();

	private List<Crazy_Weapon> weaponlist;

	public static NetworkPlayerManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("NetworkPlayerManager").AddComponent<NetworkPlayerManager>();
			}
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		weaponlist = Crazy_Weapon.ReadWeaponInfo();
	}

	public Dictionary<TNetUser, GameObject> GetPlayer()
	{
		return m_player_copy;
	}

	private void Start()
	{
		if (TNetManager.Connection != null && !Crazy_GlobalData_Net.Instance.netitem.ContainsKey(TNetManager.Connection.Myself))
		{
			Crazy_GlobalData_Net.Instance.netitem.Add(TNetManager.Connection.Myself, new List<int>());
		}
	}

	public void CreateCopy(TNetUser user, PlayerSettingInfo info)
	{
		if (info == null || !info.ingame)
		{
			return;
		}
		if (!Crazy_GlobalData_Net.Instance.userlocalinfo.ContainsKey(user))
		{
			Crazy_GlobalData_Net.Instance.userlocalinfo.Add(user, new UserLocalInfo(user.Name, info.cpc));
		}
		if (!Crazy_GlobalData_Net.Instance.netitem.ContainsKey(user))
		{
			Crazy_GlobalData_Net.Instance.netitem.Add(user, new List<int>());
		}
		if (!m_player_copy.ContainsKey(user))
		{
			Crazy_Weapon crazy_Weapon = Crazy_Weapon.FindWeaponByID(weaponlist, info.weapon);
			GameObject gameObject = null;
			switch (info.cpc)
			{
			case Crazy_PlayerClass.Fighter:
				gameObject = Crazy_Global.LoadAssetsPrefab("FBX/character/fighter/fighter_pfb");
				break;
			case Crazy_PlayerClass.Knight:
				gameObject = Crazy_Global.LoadAssetsPrefab("FBX/character/knight/knight_pfb");
				break;
			case Crazy_PlayerClass.Warrior:
				gameObject = Crazy_Global.LoadAssetsPrefab("FBX/character/warrior/warrior_pfb");
				break;
			case Crazy_PlayerClass.Rogue:
				gameObject = Crazy_Global.LoadAssetsPrefab("FBX/character/rogue/rogue_pfb");
				break;
			case Crazy_PlayerClass.Paladin:
				gameObject = Crazy_Global.LoadAssetsPrefab("FBX/character/paladin/paladin_pfb");
				break;
			case Crazy_PlayerClass.Mage:
				gameObject = Crazy_Global.LoadAssetsPrefab("FBX/character/mage/mage_pfb");
				break;
			}
			gameObject.name = "PlayerCopy";
			gameObject.transform.parent = base.transform;
			gameObject.transform.position = new Vector3(0f, 0.1f, 6f);
			gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			gameObject.layer = LayerMask.NameToLayer("Player");
			PlayerAnimationSynchronizer playerAnimationSynchronizer = ((info.cpc != Crazy_PlayerClass.Mage) ? (gameObject.AddComponent<PlayerAnimationSynchronizer>() as PlayerAnimationSynchronizer) : (gameObject.AddComponent<MagePlayerAnimationSynchronizer>() as PlayerAnimationSynchronizer));
			playerAnimationSynchronizer.SetWeapon(crazy_Weapon);
			gameObject.AddComponent<NetworkTransformInterpolation>();
			gameObject.AddComponent<NetworkTransformReceiver>();
			gameObject.AddComponent<NetworkStatusReceiver>();
			Crazy_Name crazy_Name = gameObject.AddComponent<Crazy_Name>();
			crazy_Name.InitName(user.Name);
			GameObject gameObject2 = Object.Instantiate(Resources.Load(crazy_Weapon.loadpath)) as GameObject;
			gameObject2.name = "Weapon";
			gameObject2.transform.parent = FindWeaponBone(crazy_Weapon.type, gameObject).transform;
			gameObject2.transform.localPosition = crazy_Weapon.modifyPos;
			gameObject2.transform.localEulerAngles = crazy_Weapon.modifyAngle;
			gameObject2.layer = LayerMask.NameToLayer("Player");
			m_player_copy.Add(user, gameObject);
		}
	}

	public bool ContainCopy(TNetUser user)
	{
		return m_player_copy.ContainsKey(user);
	}

	public void UpdateNetworkTrans(TNetUser user, NetworkTransform net)
	{
		GameObject value;
		if (m_player_copy.TryGetValue(user, out value))
		{
			value.GetComponent<NetworkTransformReceiver>().ReceiveTransform(net);
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

	public void UpdateNetworkStatus(TNetUser user, PlayerStatusInfo info)
	{
		GameObject value;
		if (m_player_copy.TryGetValue(user, out value))
		{
			value.GetComponent<NetworkStatusReceiver>().ReceiveStatus(info);
		}
	}

	public void DeleteCopy(TNetUser user)
	{
		if (m_player_copy.ContainsKey(user))
		{
			Object.Destroy(m_player_copy[user]);
			m_player_copy.Remove(user);
		}
	}
}
