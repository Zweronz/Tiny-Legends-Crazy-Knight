using System.Collections.Generic;
using System.Xml;

public class Crazy_LevelModify
{
	protected static Dictionary<Crazy_LevelType, Crazy_LevelModify> modifyinfo;

	protected Dictionary<int, Crazy_Modify> modify;

	public static void Initialize()
	{
		if (modifyinfo == null)
		{
			modifyinfo = new Dictionary<Crazy_LevelType, Crazy_LevelModify>();
			Crazy_LevelModify value = ReadXml("normal1");
			modifyinfo.Add(Crazy_LevelType.Normal1, value);
			value = ReadXml("normal2");
			modifyinfo.Add(Crazy_LevelType.Normal2, value);
			value = ReadXml("normal3");
			modifyinfo.Add(Crazy_LevelType.Normal3, value);
			value = ReadXml("boss");
			modifyinfo.Add(Crazy_LevelType.Boss, value);
		}
	}

	protected static Crazy_LevelModify ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		Crazy_LevelModify crazy_LevelModify = new Crazy_LevelModify();
		crazy_LevelModify.modify = new Dictionary<int, Crazy_Modify>();
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Modify" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int key = int.Parse(xmlElement.GetAttribute("level").Trim());
				Crazy_Modify crazy_Modify = new Crazy_Modify();
				crazy_Modify.speedmodify = float.Parse(xmlElement.GetAttribute("speed").Trim());
				crazy_Modify.preattacktimemodify = float.Parse(xmlElement.GetAttribute("preattacktime").Trim());
				crazy_Modify.waveid = int.Parse(xmlElement.GetAttribute("my:waveid").Trim());
				crazy_Modify.combocount = int.Parse(xmlElement.GetAttribute("my:combocount").Trim());
				crazy_Modify.time = int.Parse(xmlElement.GetAttribute("my:time").Trim());
				crazy_Modify.quantity = int.Parse(xmlElement.GetAttribute("my:quantity").Trim());
				crazy_Modify.rangedcooldown = float.Parse(xmlElement.GetAttribute("my:rangedcd").Trim());
				crazy_Modify.rangedcount = int.Parse(xmlElement.GetAttribute("my:rangedcount").Trim());
				crazy_LevelModify.modify.Add(key, crazy_Modify);
			}
		}
		return crazy_LevelModify;
	}

	public static Crazy_Modify GetModify(Crazy_LevelType type, int level)
	{
		if (modifyinfo == null)
		{
			Initialize();
		}
		Crazy_LevelModify value = new Crazy_LevelModify();
		if (modifyinfo.TryGetValue(type, out value))
		{
			Crazy_Modify value2 = new Crazy_Modify();
			if (value.modify.TryGetValue(level, out value2))
			{
				return value2;
			}
			return null;
		}
		return null;
	}

	public static bool FindLevel(Crazy_LevelType type, int down, int up, ref int level)
	{
		if (modifyinfo == null)
		{
			Initialize();
		}
		Crazy_LevelModify value = new Crazy_LevelModify();
		if (modifyinfo.TryGetValue(type, out value))
		{
			Crazy_Modify value2 = new Crazy_Modify();
			for (int i = down; i <= up; i++)
			{
				if (value.modify.TryGetValue(i, out value2))
				{
					level = i;
					return true;
				}
			}
			return false;
		}
		return false;
	}
}
