using System.Collections.Generic;
using System.Xml;

public class Crazy_NetMission
{
	public int id;

	public int seq;

	public int lv;

	public int maxcount;

	public string groupid;

	public int bossid;

	public int sceneid;

	public string name;

	public string des;

	public string bosstexture;

	public string scenetexture;

	public List<Crazy_NetMission_Item> item = new List<Crazy_NetMission_Item>();

	protected static Dictionary<int, Crazy_NetMission> info;

	public Crazy_NetMission()
	{
	}

	public Crazy_NetMission(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_NetMission>();
			ReadXml("Crazy_NetMission");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("NetMission" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int key = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_NetMission data = new Crazy_NetMission(key);
				data.seq = int.Parse(xmlElement.GetAttribute("seq").Trim());
				data.lv = int.Parse(xmlElement.GetAttribute("lv").Trim());
				data.maxcount = int.Parse(xmlElement.GetAttribute("count").Trim());
				data.groupid = xmlElement.GetAttribute("groupid").Trim();
				data.bossid = int.Parse(xmlElement.GetAttribute("bossid").Trim());
				data.sceneid = int.Parse(xmlElement.GetAttribute("sceneid").Trim());
				data.name = xmlElement.GetAttribute("name").Trim();
				data.des = xmlElement.GetAttribute("des").Trim();
				data.bosstexture = xmlElement.GetAttribute("bosstexture").Trim();
				data.scenetexture = xmlElement.GetAttribute("scenetexture").Trim();
				ReadCrazyNetMissionItem(childNode, ref data);
				info.Add(key, data);
			}
		}
	}

	protected static void ReadCrazyNetMissionItem(XmlNode xmlnode, ref Crazy_NetMission data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if ("Item" == childNode.Name)
			{
				Crazy_NetMission_Item crazy_NetMission_Item = new Crazy_NetMission_Item();
				crazy_NetMission_Item.type = (Crazy_Award_Item_Type)int.Parse(xmlElement.GetAttribute("type"));
				crazy_NetMission_Item.itemid = int.Parse(xmlElement.GetAttribute("itemid"));
				crazy_NetMission_Item.itemseq = int.Parse(xmlElement.GetAttribute("itemseq"));
				data.item.Add(crazy_NetMission_Item);
			}
		}
	}

	public static Crazy_NetMission FindNetMissionByOrder(List<Crazy_NetMission> list, int order)
	{
		int num = 1000000;
		Crazy_NetMission result = null;
		foreach (Crazy_NetMission item in list)
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

	public static List<Crazy_NetMission> GetNetMissionInfoList()
	{
		if (info == null)
		{
			Initialize();
		}
		List<Crazy_NetMission> list = new List<Crazy_NetMission>();
		foreach (Crazy_NetMission value in info.Values)
		{
			list.Add(value);
		}
		return list;
	}

	public static Crazy_NetMission GetNetMissionInfo(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_NetMission value = new Crazy_NetMission();
		if (info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
