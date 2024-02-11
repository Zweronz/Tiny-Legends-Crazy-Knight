using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Crazy_LoadingInfo
{
	protected static Dictionary<int, Crazy_LoadingInfo> info;

	public int id;

	public string iconname;

	public string text;

	public Crazy_LoadingInfo(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_LoadingInfo>();
			ReadXml("Crazy_Loading");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Loading" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int key = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_LoadingInfo crazy_LoadingInfo = new Crazy_LoadingInfo(key);
				crazy_LoadingInfo.iconname = xmlElement.GetAttribute("texture");
				crazy_LoadingInfo.text = xmlElement.GetAttribute("text");
				info.Add(key, crazy_LoadingInfo);
			}
		}
	}

	public static Crazy_LoadingInfo GetLoadingInfo(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_LoadingInfo value = new Crazy_LoadingInfo(0);
		if (info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}

	public static Crazy_LoadingInfo GetRandomLoadingInfo()
	{
		if (info == null)
		{
			Initialize();
		}
		int num = Random.Range(0, info.Count);
		int num2 = 0;
		foreach (Crazy_LoadingInfo value in info.Values)
		{
			if (num2 == num)
			{
				return value;
			}
			num2++;
		}
		return null;
	}
}
