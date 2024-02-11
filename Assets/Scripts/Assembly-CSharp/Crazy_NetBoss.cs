using System.Collections.Generic;
using System.Xml;

public class Crazy_NetBoss
{
	public int id;

	public int lv;

	public int hp;

	public float move;

	protected static Dictionary<int, Crazy_NetBoss> info;

	public Crazy_NetBoss()
	{
	}

	public Crazy_NetBoss(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_NetBoss>();
			ReadXml("Crazy_NetBoss");
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
				int key = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_NetBoss crazy_NetBoss = new Crazy_NetBoss(key);
				crazy_NetBoss.lv = int.Parse(xmlElement.GetAttribute("lv").Trim());
				crazy_NetBoss.hp = int.Parse(xmlElement.GetAttribute("hp").Trim());
				crazy_NetBoss.move = float.Parse(xmlElement.GetAttribute("move").Trim());
				info.Add(key, crazy_NetBoss);
			}
		}
	}

	public static Crazy_NetBoss GetNetBossInfo(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_NetBoss value = new Crazy_NetBoss();
		if (info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
