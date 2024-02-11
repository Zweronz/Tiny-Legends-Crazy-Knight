using System;
using System.Collections;
using UnityEngine;

public class Crazy_UpdateMonster_Net : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("BossID", Crazy_GlobalData_Net.Instance.bossID);
		hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
		FlurryPlugin.logEvent("Fight_Online", hashtable, true);
		Invoke("UpdateMonster", 3f);
	}

	private void UpdateMonster()
	{
		try
		{
			if (TNetManager.Connection == null || TNetManager.Connection.CurRoom == null)
			{
				Invoke("UpdateMonster", 3f);
			}
			else if (Crazy_SceneManager.GetInstance().GetScene().IsGameBegin())
			{
				if (!Crazy_GlobalData_Net.Instance.bossGenerate)
				{
					if (Crazy_Global_Net.IsRoomHost(TNetManager.Connection.CurRoom, TNetManager.Connection.Myself.Id))
					{
						int randomMonsterId = Crazy_Global_Net.GetRandomMonsterId();
						float seed = UnityEngine.Random.Range(0f, 100f);
						NetworkMonsterManager.Instance.CreateCopy(randomMonsterId, Crazy_GlobalData_Net.Instance.bossID, seed);
						NetworkManager.Instance.SendMonsterUpdate(new MonsterUpdateInfo(Crazy_GlobalData_Net.Instance.bossID, randomMonsterId, seed));
						Crazy_GlobalData_Net.Instance.bossGenerate = true;
					}
					else
					{
						Invoke("UpdateMonster", 3f);
					}
				}
				else
				{
					Debug.Log("boss already update end by other");
				}
			}
			else
			{
				Invoke("UpdateMonster", 3f);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
			Invoke("UpdateMonster", 3f);
		}
	}

	private void Update()
	{
	}
}
