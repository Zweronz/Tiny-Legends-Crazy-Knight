using System.Collections.Generic;
using System.Xml;

public class Crazy_Monster_Template_Manager
{
	protected static Dictionary<int, Crazy_Monster_Template> monster_template;

	public static void Initialize()
	{
		if (monster_template == null)
		{
			monster_template = new Dictionary<int, Crazy_Monster_Template>();
			ReadXml("Crazy_Monster_Template");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Template" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int num = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_Monster_Template data = new Crazy_Monster_Template(num);
				ReadCrazyMonsterTemplate(childNode, ref data);
				monster_template.Add(num, data);
			}
		}
	}

	protected static void ReadCrazyMonsterTemplate(XmlNode xmlnode, ref Crazy_Monster_Template data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if ("Name" == childNode.Name)
			{
				data.name = xmlElement.InnerText.Trim();
			}
			else if ("Path" == childNode.Name)
			{
				data.path = xmlElement.InnerText.Trim();
			}
			else if ("Type" == childNode.Name)
			{
				data.type = int.Parse(xmlElement.InnerText.Trim());
			}
			else if ("Size" == childNode.Name)
			{
				data.size = float.Parse(xmlElement.InnerText.Trim());
			}
			else if ("MoveSpeed" == childNode.Name)
			{
				data.movespeed = float.Parse(xmlElement.InnerText.Trim());
			}
			else if ("RotateSpeed" == childNode.Name)
			{
				data.rotatespeed = float.Parse(xmlElement.InnerText.Trim());
			}
			else if ("my:HitBoxSize" == childNode.Name)
			{
				data.hitboxsize = float.Parse(xmlElement.InnerText.Trim());
			}
			else if ("my:PreAttackRange" == childNode.Name)
			{
				data.preattackrange = float.Parse(xmlElement.InnerText.Trim());
			}
			else if ("my:PreAttackTime" == childNode.Name)
			{
				data.preattacktime = float.Parse(xmlElement.InnerText.Trim());
			}
			else if ("my:AttackInfoPointLength" == childNode.Name)
			{
				data.pointlength = float.Parse(xmlElement.InnerText.Trim());
			}
			else if ("my:AttackInfoPointAngle" == childNode.Name)
			{
				data.pointangle = float.Parse(xmlElement.InnerText.Trim());
			}
			else if ("my:AttackJudgmentRange" == childNode.Name)
			{
				data.judgmentrange = float.Parse(xmlElement.InnerText.Trim());
			}
			else if ("my:AttackJudgmentAngle" == childNode.Name)
			{
				data.judgmentangle = float.Parse(xmlElement.InnerText.Trim());
			}
			else if ("my:hp" == childNode.Name)
			{
				data.hpmodify = float.Parse(xmlElement.InnerText.Trim());
			}
			else if ("my:Icon" == childNode.Name)
			{
				data.iconname = xmlElement.InnerText.Trim();
			}
		}
	}

	public static Crazy_Monster_Template GetMonsterTemplate(int id)
	{
		if (monster_template == null)
		{
			Initialize();
		}
		Crazy_Monster_Template value = new Crazy_Monster_Template(0);
		if (monster_template.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
