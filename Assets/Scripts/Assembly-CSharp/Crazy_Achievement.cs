using System.Collections.Generic;
using System.Xml;

public class Crazy_Achievement
{
	public int id;

	public int seq;

	public string name;

	public string des;

	public string icon;

	public int award;

	public bool duplicate;

	public bool hide;

	public int condition;

	protected static Dictionary<int, Crazy_Achievement> info;

	public Crazy_Achievement()
	{
	}

	public Crazy_Achievement(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_Achievement>();
			ReadXml("Crazy_Achievement");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Achievement" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int key = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_Achievement crazy_Achievement = new Crazy_Achievement(key);
				crazy_Achievement.seq = int.Parse(xmlElement.GetAttribute("seq").Trim());
				crazy_Achievement.name = xmlElement.GetAttribute("name").Trim();
				crazy_Achievement.des = xmlElement.GetAttribute("des").Trim();
				crazy_Achievement.icon = xmlElement.GetAttribute("icon").Trim();
				crazy_Achievement.award = int.Parse(xmlElement.GetAttribute("award").Trim());
				crazy_Achievement.duplicate = bool.Parse(xmlElement.GetAttribute("duplicate").Trim());
				crazy_Achievement.hide = bool.Parse(xmlElement.GetAttribute("hide").Trim());
				crazy_Achievement.condition = int.Parse(xmlElement.GetAttribute("condition").Trim());
				info.Add(key, crazy_Achievement);
			}
		}
	}

	public static Crazy_Achievement FindAchievementByOrder(List<Crazy_Achievement> list, int order)
	{
		int num = 1000000;
		Crazy_Achievement result = null;
		foreach (Crazy_Achievement item in list)
		{
			int num2 = item.seq - order;
			if (num2 >= 0 && num2 < num)
			{
				result = item;
				num = num2;
			}
		}
		return result;
	}

	public static List<Crazy_Achievement> GetAchievementInfoList()
	{
		if (info == null)
		{
			Initialize();
		}
		List<Crazy_Achievement> list = new List<Crazy_Achievement>();
		foreach (Crazy_Achievement value in info.Values)
		{
			list.Add(value);
		}
		return list;
	}

	public static Crazy_Achievement GetAchievementInfo(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_Achievement value = new Crazy_Achievement();
		if (info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
