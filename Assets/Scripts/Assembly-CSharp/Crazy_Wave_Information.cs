using System.Collections.Generic;
using System.Xml;

public class Crazy_Wave_Information
{
	protected static Dictionary<int, Crazy_Wave_Data> crazy_wave_information;

	public static void Initialize()
	{
		if (crazy_wave_information == null)
		{
			crazy_wave_information = new Dictionary<int, Crazy_Wave_Data>();
			for (int i = 1; i <= 59; i++)
			{
				ReadXml("wave" + i.ToString("D2"), i);
			}
		}
	}

	protected static void ReadXml(string path, int _id)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Wave" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				Crazy_Wave_Data data = new Crazy_Wave_Data(_id);
				ReadCrazyWaveData(childNode, ref data);
				crazy_wave_information.Add(_id, data);
			}
		}
	}

	protected static void ReadCrazyWaveData(XmlNode xmlnode, ref Crazy_Wave_Data data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			if ("Control" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				Crazy_Wave_Control crazy_Wave_Control = new Crazy_Wave_Control();
				crazy_Wave_Control.message = int.Parse(xmlElement.GetAttribute("message").Trim());
				crazy_Wave_Control.bgm = int.Parse(xmlElement.GetAttribute("bgm").Trim());
				crazy_Wave_Control.number = int.Parse(xmlElement.GetAttribute("number").Trim());
				crazy_Wave_Control.rate = int.Parse(xmlElement.GetAttribute("rate").Trim());
				crazy_Wave_Control.volume = int.Parse(xmlElement.GetAttribute("volume").Trim());
				crazy_Wave_Control.count = int.Parse(xmlElement.GetAttribute("my:count").Trim());
				ReadCrazyMonsterControl(childNode, ref crazy_Wave_Control.monsterlist);
				data.controldata.Add(crazy_Wave_Control);
			}
		}
	}

	protected static void ReadCrazyMonsterControl(XmlNode xmlnode, ref List<Crazy_Monster_Control> data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			if ("Data" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				Crazy_Monster_Control crazy_Monster_Control = new Crazy_Monster_Control();
				crazy_Monster_Control.id = int.Parse(xmlElement.GetAttribute("monster").Trim());
				crazy_Monster_Control.gold = int.Parse(xmlElement.GetAttribute("gold").Trim());
				crazy_Monster_Control.item = int.Parse(xmlElement.GetAttribute("item").Trim());
				data.Add(crazy_Monster_Control);
			}
		}
	}

	public static Crazy_Wave_Data GetWaveInformation(int wave)
	{
		if (crazy_wave_information == null)
		{
			Initialize();
		}
		Crazy_Wave_Data value = new Crazy_Wave_Data(0);
		if (crazy_wave_information.TryGetValue(wave, out value))
		{
			return value;
		}
		return UpdateWaveData(wave);
	}

	protected static Crazy_Wave_Data UpdateWaveData(int wave)
	{
		Crazy_Wave_Data crazy_Wave_Data = new Crazy_Wave_Data(wave);
		Crazy_Wave_Control crazy_Wave_Control = new Crazy_Wave_Control();
		crazy_Wave_Control.volume = 45;
		crazy_Wave_Control.rate = 1;
		crazy_Wave_Control.number = 5;
		crazy_Wave_Control.message = -1;
		crazy_Wave_Control.bgm = 1;
		crazy_Wave_Control.count = 8;
		crazy_Wave_Data.controldata.Add(crazy_Wave_Control);
		crazy_wave_information.Add(wave, crazy_Wave_Data);
		return crazy_Wave_Data;
	}
}
