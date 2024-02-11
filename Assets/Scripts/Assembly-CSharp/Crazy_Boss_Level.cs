using System.Collections.Generic;
using System.Xml;

public class Crazy_Boss_Level
{
	public int levelid;

	public Crazy_PlayerClass player;

	public int weaponid;

	protected List<Crazy_Boss_Data> bossdata;

	protected static Dictionary<int, Crazy_Boss_Level> levelinfo;

	protected int bossdatacount;

	public static void Initialize()
	{
		if (levelinfo == null)
		{
			levelinfo = new Dictionary<int, Crazy_Boss_Level>();
			ReadXml("Crazy_Boss_Level");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("BossLevel" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				Crazy_Boss_Level data = new Crazy_Boss_Level();
				data.levelid = int.Parse(xmlElement.GetAttribute("level").Trim());
				data.player = (Crazy_PlayerClass)int.Parse(xmlElement.GetAttribute("character").Trim());
				data.weaponid = int.Parse(xmlElement.GetAttribute("weapon").Trim());
				ReadBossData(childNode, ref data);
				levelinfo.Add(data.levelid, data);
			}
		}
	}

	protected static void ReadBossData(XmlNode xmlnode, ref Crazy_Boss_Level data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if ("Boss" == childNode.Name)
			{
				Crazy_Boss_Data crazy_Boss_Data = new Crazy_Boss_Data();
				crazy_Boss_Data.id = int.Parse(xmlElement.GetAttribute("id").Trim());
				crazy_Boss_Data.hp = int.Parse(xmlElement.GetAttribute("hp").Trim());
				crazy_Boss_Data.movemodify = float.Parse(xmlElement.GetAttribute("move").Trim());
				crazy_Boss_Data.force = float.Parse(xmlElement.GetAttribute("force").Trim());
				crazy_Boss_Data.gold = int.Parse(xmlElement.GetAttribute("my:gold").Trim());
				crazy_Boss_Data.exp = int.Parse(xmlElement.GetAttribute("my:exp").Trim());
				if (data.bossdata == null)
				{
					data.bossdata = new List<Crazy_Boss_Data>();
				}
				data.bossdata.Add(crazy_Boss_Data);
			}
		}
	}

	public static Crazy_Boss_Level GetBossLevel(int level)
	{
		if (levelinfo == null)
		{
			Initialize();
		}
		Crazy_Boss_Level value = new Crazy_Boss_Level();
		if (levelinfo.TryGetValue(level, out value))
		{
			return value;
		}
		return null;
	}

	public List<Crazy_Boss_Data> GetBossData()
	{
		return bossdata;
	}

	public Crazy_Boss_Data GetNextBossData()
	{
		if (bossdata != null)
		{
			if (bossdatacount >= bossdata.Count)
			{
				bossdatacount = 0;
			}
			Crazy_Boss_Data result = bossdata[bossdatacount];
			bossdatacount++;
			return result;
		}
		return null;
	}
}
