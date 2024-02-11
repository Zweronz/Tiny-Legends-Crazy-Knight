using System.Collections.Generic;
using System.Xml;

public class Crazy_BossDrop
{
	public int bossid;

	public int dropid;

	protected static Dictionary<int, Crazy_BossDrop> info;

	public Crazy_BossDrop()
	{
	}

	public Crazy_BossDrop(int _id)
	{
		bossid = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_BossDrop>();
			ReadXml("Crazy_BossDrop");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Boss" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int num = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_BossDrop crazy_BossDrop = new Crazy_BossDrop(num);
				crazy_BossDrop.dropid = int.Parse(xmlElement.GetAttribute("dropid").Trim());
				info.Add(num, crazy_BossDrop);
			}
		}
	}

	public static Crazy_BossDrop GetBossDropInfo(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_BossDrop value = new Crazy_BossDrop();
		if (info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
