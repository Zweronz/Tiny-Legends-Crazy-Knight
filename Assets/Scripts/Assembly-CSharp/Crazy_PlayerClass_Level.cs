using System.Collections.Generic;
using System.Xml;

public class Crazy_PlayerClass_Level
{
	protected static Dictionary<Crazy_PlayerClass, Crazy_PlayerClass_Level> playerclass;

	protected Dictionary<int, Crazy_Player_Level> player;

	public static void Initialize()
	{
		if (playerclass == null)
		{
			playerclass = new Dictionary<Crazy_PlayerClass, Crazy_PlayerClass_Level>();
			Crazy_PlayerClass_Level value = ReadXml("Fighter");
			playerclass.Add(Crazy_PlayerClass.Fighter, value);
			value = ReadXml("Knight");
			playerclass.Add(Crazy_PlayerClass.Knight, value);
			value = ReadXml("Warrior");
			playerclass.Add(Crazy_PlayerClass.Warrior, value);
			value = ReadXml("Rogue");
			playerclass.Add(Crazy_PlayerClass.Rogue, value);
			value = ReadXml("Paladin");
			playerclass.Add(Crazy_PlayerClass.Paladin, value);
			value = ReadXml("Magic");
			playerclass.Add(Crazy_PlayerClass.Mage, value);
		}
	}

	protected static Crazy_PlayerClass_Level ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		Crazy_PlayerClass_Level crazy_PlayerClass_Level = new Crazy_PlayerClass_Level();
		crazy_PlayerClass_Level.player = new Dictionary<int, Crazy_Player_Level>();
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Level" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int num = int.Parse(xmlElement.GetAttribute("lv").Trim());
				Crazy_Player_Level crazy_Player_Level = new Crazy_Player_Level(num);
				int exp = int.Parse(xmlElement.GetAttribute("exp").Trim());
				crazy_Player_Level.exp = exp;
				int damage = int.Parse(xmlElement.GetAttribute("damage").Trim());
				crazy_Player_Level.damage = damage;
				int hallid = int.Parse(xmlElement.GetAttribute("my:hallid").Trim());
				crazy_Player_Level.hallid = hallid;
				crazy_PlayerClass_Level.player.Add(num, crazy_Player_Level);
			}
		}
		return crazy_PlayerClass_Level;
	}

	public static Crazy_Player_Level GetPlayerLevelinfo(int lv)
	{
		Crazy_PlayerClass playerClass = Crazy_Data.CurData().GetPlayerClass();
		return GetPlayerLevelinfo(playerClass, lv);
	}

	public static Crazy_Player_Level GetPlayerLevelinfo(Crazy_PlayerClass type, int lv)
	{
		if (playerclass == null)
		{
			Initialize();
		}
		Crazy_PlayerClass_Level value = new Crazy_PlayerClass_Level();
		if (playerclass.TryGetValue(type, out value))
		{
			Crazy_Player_Level value2 = new Crazy_Player_Level(0);
			if (value.player.TryGetValue(lv, out value2))
			{
				return value2;
			}
			return null;
		}
		return null;
	}
}
