using TNetSdk;
using UnityEngine;

public class TNetManager : MonoBehaviour
{
	private static TNetManager mInstance;

	private static TNetObject tnetObject;

	public static bool isLogin;

	public static bool isConnection;

	public static TNetObject Connection
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = new GameObject("TNetManager").AddComponent(typeof(TNetManager)) as TNetManager;
			}
			if (isConnection)
			{
				return tnetObject;
			}
			return null;
		}
	}

	public static TNetObject Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = new GameObject("TNetManager").AddComponent(typeof(TNetManager)) as TNetManager;
			}
			return tnetObject;
		}
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public static void CreateTNetObject()
	{
		tnetObject = new TNetObject();
	}

	public static void Connect(string ip, int port)
	{
		tnetObject.Connect(ip, port);
	}

	public static void ConnectionSuccess()
	{
		isConnection = true;
	}

	public static void ConnectionLost()
	{
		isLogin = false;
		isConnection = false;
		if (tnetObject != null)
		{
			tnetObject.RemoveAllEventListeners();
		}
		tnetObject = null;
	}

	public static void ManualDisconnect()
	{
		if (tnetObject != null)
		{
			tnetObject.Close();
			ConnectionLost();
		}
	}

	public static void Login(string username)
	{
		if (tnetObject != null)
		{
			tnetObject.Send(new LoginRequest(username));
		}
	}

	public static void LoginSuccess()
	{
		isLogin = true;
	}

	public static void LoginFailed()
	{
		ManualDisconnect();
	}

	private void OnApplicationQuit()
	{
		ManualDisconnect();
	}
}
