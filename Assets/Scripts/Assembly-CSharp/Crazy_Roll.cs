using System.Collections.Generic;
using System.Xml;

public class Crazy_Roll
{
	public float rolltime;

	public float rollspeed;

	public float rollrecovertime;

	public float rollcd;

	protected static Dictionary<Crazy_Weapon_Type, Crazy_Roll> info;

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<Crazy_Weapon_Type, Crazy_Roll>();
			ReadXml("Crazy_Roll");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Roll" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				Crazy_Weapon_Type key = (Crazy_Weapon_Type)int.Parse(xmlElement.GetAttribute("weapontype").Trim());
				Crazy_Roll crazy_Roll = new Crazy_Roll();
				crazy_Roll.rolltime = float.Parse(xmlElement.GetAttribute("rolltime").Trim());
				crazy_Roll.rollspeed = float.Parse(xmlElement.GetAttribute("rollspeed").Trim());
				crazy_Roll.rollrecovertime = float.Parse(xmlElement.GetAttribute("rollrecovertime").Trim());
				crazy_Roll.rollcd = float.Parse(xmlElement.GetAttribute("rollcd").Trim());
				info.Add(key, crazy_Roll);
			}
		}
	}

	public static Crazy_Roll GetRollInfo(Crazy_Weapon_Type type)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_Roll value = new Crazy_Roll();
		if (info.TryGetValue(type, out value))
		{
			return value;
		}
		return null;
	}
}
