using System.Collections;
using LitJson;
using UnityEngine;

public class CommunicationConfirm : MonoBehaviour
{
	public OnResponse_Confirm callback;

	public OnRequestTimeout_Confirm callback_error;

	private static CommunicationConfirm instance;

	private string url = "http://account.trinitigame.com:8081/gameapi/turboPlatform2.do?action=groovy&json={}";

	protected string deviceid = string.Empty;

	protected string cmd = "CheckGameName";

	protected string gamename = "TLHE";

	public static CommunicationConfirm Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("CommunicationConfirm").AddComponent<CommunicationConfirm>();
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
		deviceid = OtherPlugin.GetMacAddr();
		Hashtable hashtable = new Hashtable();
		hashtable.Add("deviceid", deviceid);
		hashtable.Add("cmd", cmd);
		hashtable.Add("gamename", gamename);
		string request = JsonMapper.ToJson(hashtable);
		HttpManager.Instance().SendRequest(url, request, null, 15f, OnResponse, OnRequestTimeout);
	}

	private void OnResponse(int task_id, string param, int code, string response)
	{
		Hashtable hashtable = JsonMapper.ToObject<Hashtable>(response);
		if (callback != null)
		{
			callback((hashtable["has"].ToString() == "1") ? true : false);
			callback = null;
		}
	}

	private void OnRequestTimeout(int task_id, string param)
	{
		Debug.LogWarning("OnRequestTimeout");
		if (callback_error != null)
		{
			callback_error();
			callback_error = null;
		}
	}

	private void Update()
	{
	}
}
