using System.Collections;
using LitJson;
using UnityEngine;

public class CommunicationServerTime : MonoBehaviour
{
	public OnResponse_GetSysTime callback;

	public OnRequestTimeout_GetSysTime callback_error;

	private static CommunicationServerTime instance;

	private string url = "http://97.74.205.45:7600/gameapi/GameCommon.do?action=groovy&json={}";

	protected string cmd = "GetServerTime";

	public static CommunicationServerTime Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("CommunicationServerTime").AddComponent<CommunicationServerTime>();
			}
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void SentData()
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("cmd", cmd);
		string request = JsonMapper.ToJson(hashtable);
		int num = HttpManager.Instance().SendRequest(url, request, null, 15f, OnResponse, OnRequestTimeout);
	}

	private void OnResponse(int task_id, string param, int code, string response)
	{
		Debug.LogWarning("OnResponse code : " + code + " &  param : " + param + " & response :" + response);
		Hashtable hashtable = JsonMapper.ToObject<Hashtable>(response);
		if (callback != null)
		{
			callback(hashtable["datetime"].ToString());
			callback = null;
		}
	}

	private void OnRequestTimeout(int task_id, string param)
	{
		Debug.LogWarning("++++++++++++++CommunicationServerTime----OnRequestTimeout");
		Crazy_GlobalData.m_bShowDailyReward = false;
		if (callback_error != null)
		{
			callback_error();
			callback_error = null;
		}
	}
}
