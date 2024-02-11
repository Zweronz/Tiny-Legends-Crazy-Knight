using System.Collections.Generic;
using System.Xml;

public class Crazy_BossStatusInfo
{
	protected static Dictionary<string, List<Crazy_Boss_Status>> bossinfo;

	public static void Initialize(string path, Dictionary<int, Crazy_Boss_Skill> s)
	{
		if (bossinfo == null)
		{
			bossinfo = new Dictionary<string, List<Crazy_Boss_Status>>();
		}
		if (!bossinfo.ContainsKey(path))
		{
			List<Crazy_Boss_Status> value = ReadXml(path, s);
			bossinfo.Add(path, value);
		}
	}

	protected static List<Crazy_Boss_Status> ReadXml(string path, Dictionary<int, Crazy_Boss_Skill> s)
	{
		List<Crazy_Boss_Status> list = new List<Crazy_Boss_Status>();
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if (!("Status" == childNode.Name))
			{
				continue;
			}
			XmlElement xmlElement = (XmlElement)childNode;
			Crazy_Boss_Status crazy_Boss_Status = new Crazy_Boss_Status();
			crazy_Boss_Status.id = int.Parse(xmlElement.GetAttribute("my:id").Trim());
			crazy_Boss_Status.priority = int.Parse(xmlElement.GetAttribute("priority").Trim());
			crazy_Boss_Status.condition = (Crazy_Boss_Status_ConditionType)int.Parse(xmlElement.GetAttribute("condition").Trim());
			crazy_Boss_Status.conditionparam = float.Parse(xmlElement.GetAttribute("param").Trim());
			foreach (XmlNode childNode2 in childNode.ChildNodes)
			{
				if (!("Data" == childNode2.Name))
				{
					continue;
				}
				XmlElement xmlElement2 = (XmlElement)childNode2;
				crazy_Boss_Status.data.moverate = float.Parse(xmlElement2.GetAttribute("moverate").Trim());
				crazy_Boss_Status.data.preattacktimerate = float.Parse(xmlElement2.GetAttribute("preattacktimerate").Trim());
				crazy_Boss_Status.data.endattacktimerate = float.Parse(xmlElement2.GetAttribute("endattacktimerate").Trim());
				foreach (XmlNode childNode3 in childNode2.ChildNodes)
				{
					if ("Skill" == childNode3.Name)
					{
						XmlElement xmlElement3 = (XmlElement)childNode3;
						Crazy_Boss_Skill key = s[int.Parse(xmlElement3.GetAttribute("id").Trim())];
						crazy_Boss_Status.data.m_skill.Add(key, int.Parse(xmlElement3.GetAttribute("skillpriority").Trim()));
					}
				}
			}
			list.Add(crazy_Boss_Status);
		}
		return list;
	}

	public static List<Crazy_Boss_Status> GetCrazyBossStatus(string path, Dictionary<int, Crazy_Boss_Skill> s)
	{
		if (bossinfo == null || !bossinfo.ContainsKey(path))
		{
			Initialize(path, s);
		}
		List<Crazy_Boss_Status> value = new List<Crazy_Boss_Status>();
		if (bossinfo.TryGetValue(path, out value))
		{
			return value;
		}
		return null;
	}
}
