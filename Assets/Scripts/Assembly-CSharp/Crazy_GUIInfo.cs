using System.Collections.Generic;
using UnityEngine;

public class Crazy_GUIInfo : MonoBehaviour
{
	public bool isDebug;

	private static Crazy_GUIInfo instance;

	protected List<string> m_show = new List<string>();

	protected int length = 20;

	public static Crazy_GUIInfo Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameObject("Crazy_GUIInfo").AddComponent<Crazy_GUIInfo>();
			}
			return instance;
		}
	}

	private void Awake()
	{
		isDebug = Crazy_Global.IsTestVersion;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Add(string str)
	{
		if (m_show.Count >= length)
		{
			m_show.RemoveAt(0);
			m_show.Add(str);
		}
		else
		{
			m_show.Add(str);
		}
	}

	private void OnGUI()
	{
		if (!isDebug)
		{
			return;
		}
		string text = string.Empty;
		foreach (string item in m_show)
		{
			text = text + item + "\n";
		}
		GUILayout.Label(text);
		if (TNetManager.Connection == null)
		{
		}
	}
}
