using System.Collections.Generic;
using TNetSdk;

public sealed class Crazy_GlobalData_Net
{
	private static Crazy_GlobalData_Net instance;

	public bool bossGenerate;

	public int bossID;

	public int sceneID;

	public int netmissionID;

	public Dictionary<TNetUser, List<int>> netitem = new Dictionary<TNetUser, List<int>>();

	public Dictionary<TNetUser, UserLocalInfo> userlocalinfo = new Dictionary<TNetUser, UserLocalInfo>();

	public static Crazy_GlobalData_Net Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new Crazy_GlobalData_Net();
			}
			return instance;
		}
	}

	public void Reset()
	{
		netitem.Clear();
		userlocalinfo.Clear();
		bossGenerate = false;
	}
}
