using System.Collections.Generic;
using UnityEngine;

public class NetworkMonsterManager : MonoBehaviour
{
	private static NetworkMonsterManager instance;

	private Dictionary<int, GameObject> m_monster_copy = new Dictionary<int, GameObject>();

	public static NetworkMonsterManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("NetworkMonsterManager").AddComponent<NetworkMonsterManager>();
			}
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		Crazy_GlobalData.enemyList = new Dictionary<int, GameObject>();
	}

	public Dictionary<int, GameObject> GetMonster()
	{
		return m_monster_copy;
	}

	private void Start()
	{
	}

	public void MonsterUpdate(MonsterUpdateInfo info)
	{
		if (!m_monster_copy.ContainsKey(info.monsterid))
		{
			Crazy_GlobalData_Net.Instance.bossGenerate = true;
			Crazy_Global_Net.SynchronizeMonsterId(info.monsterid);
			CreateCopy(info.monsterid, info.monstertemplateid, info.seed);
		}
	}

	public void GetController()
	{
		foreach (int key in m_monster_copy.Keys)
		{
			m_monster_copy[key].SendMessage("OnController", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void CreateCopy(int id, int templateid, float seed)
	{
		if (!m_monster_copy.ContainsKey(id))
		{
			m_monster_copy.Add(id, CreateBoss(templateid, id, seed));
		}
	}

	private GameObject CreateBoss(int templateid, int networkid, float seed)
	{
		Crazy_Monster_Template monsterTemplate = Crazy_Monster_Template_Manager.GetMonsterTemplate(templateid);
		Crazy_NetBoss netBossInfo = Crazy_NetBoss.GetNetBossInfo(templateid);
		Crazy_Monster_Level crazy_Monster_Level = new Crazy_Monster_Level(netBossInfo.lv);
		crazy_Monster_Level.exp = 0;
		crazy_Monster_Level.gold = 0;
		crazy_Monster_Level.hp = netBossInfo.hp;
		GameObject gameObject = Object.Instantiate(Resources.Load(monsterTemplate.path)) as GameObject;
		gameObject.name = monsterTemplate.name;
		Crazy_EnemyControl_Boss_Net crazy_EnemyControl_Boss_Net = gameObject.GetComponent("Crazy_EnemyControl_Boss_Net") as Crazy_EnemyControl_Boss_Net;
		if ((bool)crazy_EnemyControl_Boss_Net)
		{
			crazy_EnemyControl_Boss_Net.InitData(monsterTemplate, crazy_Monster_Level, 0, netBossInfo.move, 1f);
			crazy_EnemyControl_Boss_Net.SetEffect(Crazy_SceneManager.GetInstance().GetScene().monstereffect);
			crazy_EnemyControl_Boss_Net.SetHint(Crazy_SceneManager.GetInstance().GetScene().monsterhint);
			crazy_EnemyControl_Boss_Net.InitNetworkId(networkid);
			crazy_EnemyControl_Boss_Net.InitSeed(seed);
		}
		else
		{
			Crazy_EnemyControl_MidBoss_Net crazy_EnemyControl_MidBoss_Net = gameObject.GetComponent("Crazy_EnemyControl_MidBoss_Net") as Crazy_EnemyControl_MidBoss_Net;
			crazy_EnemyControl_MidBoss_Net.InitData(monsterTemplate, crazy_Monster_Level, 0, netBossInfo.move, 1f);
			crazy_EnemyControl_MidBoss_Net.SetEffect(Crazy_SceneManager.GetInstance().GetScene().monstereffect);
			crazy_EnemyControl_MidBoss_Net.SetHint(Crazy_SceneManager.GetInstance().GetScene().monsterhint);
			crazy_EnemyControl_MidBoss_Net.InitNetworkId(networkid);
			crazy_EnemyControl_MidBoss_Net.InitSeed(seed);
		}
		gameObject.layer = LayerMask.NameToLayer("Enemy");
		gameObject.transform.parent = base.gameObject.transform;
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		Crazy_GlobalData.enemyList.Add(networkid, gameObject);
		return gameObject;
	}

	public bool ContainCopy(int id)
	{
		return m_monster_copy.ContainsKey(id);
	}

	public void UpdateBossSkill(BossSkillInfo bsi)
	{
		GameObject value = null;
		if (m_monster_copy.TryGetValue(int.Parse(bsi.objectid), out value))
		{
			value.SendMessage("OnBossSkillReceiver", bsi, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void UpdateBossStatus(BossStatusInfo bsi)
	{
		GameObject value = null;
		if (m_monster_copy.TryGetValue(int.Parse(bsi.objectid), out value))
		{
			value.SendMessage("OnBossStatusReceiver", bsi, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void UpdateMonsterStatus(MonsterStatusInfo msi)
	{
		GameObject value = null;
		if (m_monster_copy.TryGetValue(int.Parse(msi.objectid), out value))
		{
			value.SendMessage("OnMonsterStatusReceiver", msi, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void UpdateNetworkTrans(int id, NetworkTransform net)
	{
		GameObject value = null;
		if (m_monster_copy.TryGetValue(id, out value))
		{
			NetworkTransformReceiver component = value.GetComponent<NetworkTransformReceiver>();
			if (component != null)
			{
				component.ReceiveTransform(net);
			}
		}
	}

	public void UpdateMonsterHurt(MonsterHurtInfo mhi)
	{
		GameObject value = null;
		if (m_monster_copy.TryGetValue(int.Parse(mhi.objectid), out value))
		{
			value.SendMessage("OnMonsterHurtReceiver", mhi, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void DeleteCopy(int id)
	{
		if (m_monster_copy.ContainsKey(id))
		{
			Object.Destroy(m_monster_copy[id]);
			m_monster_copy.Remove(id);
		}
	}

	public void OnDestroy()
	{
		Crazy_GlobalData.enemyList = null;
	}
}
