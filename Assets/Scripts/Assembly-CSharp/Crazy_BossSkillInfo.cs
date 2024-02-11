using System.Collections.Generic;
using System.Xml;

public class Crazy_BossSkillInfo
{
	protected static Dictionary<string, Dictionary<int, Crazy_Boss_Skill>> bossinfo;

	public static void Initialize(string path)
	{
		if (bossinfo == null)
		{
			bossinfo = new Dictionary<string, Dictionary<int, Crazy_Boss_Skill>>();
		}
		if (!bossinfo.ContainsKey(path))
		{
			Dictionary<int, Crazy_Boss_Skill> value = ReadXml(path);
			bossinfo.Add(path, value);
		}
	}

	protected static Dictionary<int, Crazy_Boss_Skill> ReadXml(string path)
	{
		Dictionary<int, Crazy_Boss_Skill> dictionary = new Dictionary<int, Crazy_Boss_Skill>();
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if (!("Skill" == childNode.Name))
			{
				continue;
			}
			XmlElement xmlElement = (XmlElement)childNode;
			Crazy_Boss_Skill crazy_Boss_Skill = new Crazy_Boss_Skill();
			crazy_Boss_Skill.cur_data.id = int.Parse(xmlElement.GetAttribute("id").Trim());
			crazy_Boss_Skill.cur_data.originalpriority = int.Parse(xmlElement.GetAttribute("priority").Trim());
			crazy_Boss_Skill.cur_data.priority = crazy_Boss_Skill.cur_data.originalpriority;
			crazy_Boss_Skill.cur_data.cooldowntime = float.Parse(xmlElement.GetAttribute("cooldowntime").Trim());
			foreach (XmlNode childNode2 in childNode.ChildNodes)
			{
				if ("Begin" == childNode2.Name)
				{
					XmlElement xmlElement2 = (XmlElement)childNode2;
					crazy_Boss_Skill.cur_data.begintopreparerange = float.Parse(xmlElement2.GetAttribute("begintopreparerange").Trim());
					crazy_Boss_Skill.cur_data.isDelay = bool.Parse(xmlElement2.GetAttribute("isDelay").Trim());
				}
				else if ("Prepare" == childNode2.Name)
				{
					XmlElement xmlElement3 = (XmlElement)childNode2;
					crazy_Boss_Skill.cur_data.preparetime = float.Parse(xmlElement3.GetAttribute("preparetime").Trim());
					crazy_Boss_Skill.cur_data.preanimationname = xmlElement3.GetAttribute("preanimationname").Trim();
				}
				else if ("Use" == childNode2.Name)
				{
					XmlElement xmlElement4 = (XmlElement)childNode2;
					crazy_Boss_Skill.cur_data.useanimationname = xmlElement4.GetAttribute("useanimationname").Trim();
					foreach (XmlNode childNode3 in childNode2.ChildNodes)
					{
						if ("SkillPoint" == childNode3.Name)
						{
							XmlElement xmlElement5 = (XmlElement)childNode3;
							Crazy_SkillPoint crazy_SkillPoint = new Crazy_SkillPoint();
							crazy_SkillPoint.angle = float.Parse(xmlElement5.GetAttribute("angle").Trim());
							crazy_SkillPoint.fixangle = float.Parse(xmlElement5.GetAttribute("fixangle").Trim());
							crazy_SkillPoint.rangelength = float.Parse(xmlElement5.GetAttribute("rangelength").Trim());
							crazy_SkillPoint.length = float.Parse(xmlElement5.GetAttribute("length").Trim());
							crazy_SkillPoint.rangeangle = float.Parse(xmlElement5.GetAttribute("rangeangle").Trim());
							crazy_Boss_Skill.cur_data.damagejudgment.Add(crazy_SkillPoint);
						}
					}
				}
				else if ("End" == childNode2.Name)
				{
					XmlElement xmlElement6 = (XmlElement)childNode2;
					crazy_Boss_Skill.cur_data.endtime = float.Parse(xmlElement6.GetAttribute("endtime").Trim());
					crazy_Boss_Skill.cur_data.endanimationname = xmlElement6.GetAttribute("endanimationname").Trim();
				}
			}
			dictionary.Add(crazy_Boss_Skill.cur_data.id, crazy_Boss_Skill);
		}
		return dictionary;
	}

	public static Dictionary<int, Crazy_Boss_Skill> GetCrazyBossSkill(string path)
	{
		if (bossinfo == null || !bossinfo.ContainsKey(path))
		{
			Initialize(path);
		}
		Dictionary<int, Crazy_Boss_Skill> value = new Dictionary<int, Crazy_Boss_Skill>();
		if (bossinfo.TryGetValue(path, out value))
		{
			return value;
		}
		return null;
	}
}
