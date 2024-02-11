using System.Collections.Generic;
using System.Xml;

public class Crazy_Award
{
	public int id;

	public List<Crazy_Award_Item> item = new List<Crazy_Award_Item>();

	protected static Dictionary<int, Crazy_Award> info;

	public Crazy_Award()
	{
	}

	public Crazy_Award(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_Award>();
			ReadXml("Crazy_Award");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Award" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int key = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_Award data = new Crazy_Award(key);
				ReadCrazyAward(childNode, ref data);
				info.Add(key, data);
			}
		}
	}

	protected static void ReadCrazyAward(XmlNode xmlnode, ref Crazy_Award data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if ("Item" == childNode.Name)
			{
				Crazy_Award_Item crazy_Award_Item = new Crazy_Award_Item();
				crazy_Award_Item.id = int.Parse(xmlElement.GetAttribute("id").Trim());
				crazy_Award_Item.type = (Crazy_Award_Item_Type)int.Parse(xmlElement.GetAttribute("type").Trim());
				crazy_Award_Item.count = int.Parse(xmlElement.GetAttribute("count").Trim());
				data.item.Add(crazy_Award_Item);
			}
		}
	}

	public static Crazy_Award GetAwardInfo(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_Award value = new Crazy_Award();
		if (info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
