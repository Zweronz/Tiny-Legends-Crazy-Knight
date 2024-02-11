using System.Collections.Generic;
using System.Xml;

public class Crazy_Monster_Level_Manager
{
	protected static Dictionary<int, Crazy_Monster_Level> monster_level;

	protected static Dictionary<int, Crazy_Monster_Level> middleboss_level;

	public static void Initialize()
	{
		if (monster_level == null)
		{
			monster_level = new Dictionary<int, Crazy_Monster_Level>();
			middleboss_level = new Dictionary<int, Crazy_Monster_Level>();
			ReadXml("Crazy_Monster_Level", ref monster_level);
			ReadXml("Crazy_MiddleBoss_Level", ref middleboss_level);
		}
	}

	protected static void ReadXml(string path, ref Dictionary<int, Crazy_Monster_Level> info)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Level" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int num = int.Parse(xmlElement.GetAttribute("lv").Trim());
				Crazy_Monster_Level data = new Crazy_Monster_Level(num);
				ReadCrazyMonsterLevel(childNode, ref data);
				info.Add(num, data);
			}
		}
	}

	protected static void ReadCrazyMonsterLevel(XmlNode xmlnode, ref Crazy_Monster_Level data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if ("HP" == childNode.Name)
			{
				data.hp = float.Parse(xmlElement.InnerText.Trim());
			}
			else if ("my:gold" == childNode.Name)
			{
				data.gold = int.Parse(xmlElement.InnerText.Trim());
			}
			else if ("my:exp" == childNode.Name)
			{
				data.exp = int.Parse(xmlElement.InnerText.Trim());
			}
		}
	}

	public static Crazy_Monster_Level GetMonsterLevel(int lv)
	{
		if (monster_level == null)
		{
			Initialize();
		}
		Crazy_Monster_Level value = new Crazy_Monster_Level(0);
		if (monster_level.TryGetValue(lv, out value))
		{
			return value;
		}
		return null;
	}

	public static Crazy_Monster_Level GetMiddleBossLevel(int lv)
	{
		if (middleboss_level == null)
		{
			Initialize();
		}
		Crazy_Monster_Level value = new Crazy_Monster_Level(0);
		if (middleboss_level.TryGetValue(lv, out value))
		{
			return value;
		}
		return null;
	}
}
