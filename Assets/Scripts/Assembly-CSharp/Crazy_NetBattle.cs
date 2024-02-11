using System.Collections.Generic;
using System.Xml;

public class Crazy_NetBattle
{
	public struct bossItem
	{
		public int maxcount;

		public string bossname;

		public int bossid;

		public int sceneid;
	}

	public List<bossItem> bossItems = new List<bossItem>();

	public int hallid;

	public string hallname;

	protected static Dictionary<int, Crazy_NetBattle> info;

	public Crazy_NetBattle()
	{
	}

	public Crazy_NetBattle(int _id)
	{
		hallid = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_NetBattle>();
			ReadXml("Crazy_NetBattle");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("NetBattle" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int num = int.Parse(xmlElement.GetAttribute("hallid").Trim());
				Crazy_NetBattle data = new Crazy_NetBattle(num);
				data.hallname = xmlElement.GetAttribute("name").Trim();
				ReadBossItem(childNode, ref data);
				info.Add(num, data);
			}
		}
	}

	protected static void ReadBossItem(XmlNode xmlnode, ref Crazy_NetBattle data)
	{
		bossItem item = default(bossItem);
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if ("BossList" == childNode.Name)
			{
				item.maxcount = int.Parse(xmlElement.GetAttribute("players").Trim());
				item.bossname = xmlElement.GetAttribute("bossname").Trim();
				item.bossid = int.Parse(xmlElement.GetAttribute("bossid").Trim());
				item.sceneid = int.Parse(xmlElement.GetAttribute("sceneid").Trim());
				data.bossItems.Add(item);
			}
		}
	}

	public static List<Crazy_NetBattle> GetNetBattleInfoList()
	{
		if (info == null)
		{
			Initialize();
		}
		List<Crazy_NetBattle> list = new List<Crazy_NetBattle>();
		foreach (Crazy_NetBattle value in info.Values)
		{
			list.Add(value);
		}
		return list;
	}

	public static Crazy_NetBattle GetNetBattleInfo(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_NetBattle value = new Crazy_NetBattle();
		if (info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
