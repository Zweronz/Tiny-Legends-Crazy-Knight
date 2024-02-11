using System;
using UnityEngine;

public class EventOnHomeKey : MonoBehaviour
{
	private void Awake()
	{
		Application.runInBackground = false;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		if (Crazy_Data.CurData().GetLastLoginTime() != string.Empty)
		{
			TimeSpan timeSpan = DateTime.Now - DateTime.Parse(Crazy_Data.CurData().GetLastLoginTime());
			if (timeSpan.TotalHours >= 24.0 && timeSpan.TotalHours <= 48.0)
			{
				Crazy_GlobalData.g_bActiveUser = true;
			}
			else
			{
				Crazy_GlobalData.g_bActiveUser = false;
			}
		}
		else
		{
			Crazy_GlobalData.g_bActiveUser = false;
		}
		CommunicationServerTime.Instance.callback = OnGetServerTime;
		CommunicationServerTime.Instance.SentData();
	}

	private void OnApplicationPause(bool pause)
	{
		Debug.Log("OnApplicationPause:" + pause);
		if (pause)
		{
			Crazy_Data.CurData().SetLastLoginTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			Crazy_Data.SaveData();
			return;
		}
		if (Crazy_Data.CurData().GetLastLoginTime() != string.Empty)
		{
			TimeSpan timeSpan = DateTime.Now - DateTime.Parse(Crazy_Data.CurData().GetLastLoginTime());
			if (timeSpan.TotalHours >= 24.0 && timeSpan.TotalHours <= 48.0)
			{
				Crazy_GlobalData.g_bActiveUser = true;
			}
			else
			{
				Crazy_GlobalData.g_bActiveUser = false;
			}
		}
		else
		{
			Crazy_GlobalData.g_bActiveUser = false;
		}
		CommunicationServerTime.Instance.callback = OnGetServerTime;
		CommunicationServerTime.Instance.SentData();
	}

	private void OnGetServerTime(string time)
	{
		Crazy_GlobalData.m_bShowDailyReward = true;
		Crazy_Global.SetServerTime(time);
		if (Crazy_Data.CurData().GetLastDailyAwardTime() != string.Empty)
		{
			TimeSpan timeSpan = DateTime.Parse(time) - DateTime.Parse(Crazy_Data.CurData().GetLastDailyAwardTime());
			if (timeSpan.TotalHours > 0.0 && timeSpan.TotalHours < 24.0)
			{
				Crazy_GlobalData.m_bShowDailyReward = false;
			}
		}
	}

	private void OnApplicationQuit()
	{
		Crazy_Data.CurData().SetLastLoginTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
		Crazy_Data.SaveData();
		Crazy_Data.CurData().SetLastLeaveTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
	}
}
