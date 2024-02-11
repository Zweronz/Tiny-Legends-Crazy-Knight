using System.Collections.Generic;
using System.Xml;

public class Crazy_Package
{
	public int id;

	public List<Crazy_Package_Item> item = new List<Crazy_Package_Item>();

	protected static Dictionary<int, Crazy_Package> info;

	public Crazy_Package()
	{
	}

	public Crazy_Package(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_Package>();
			ReadXml("Crazy_Package");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Package" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int key = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_Package data = new Crazy_Package(key);
				ReadCrazyPackage(childNode, ref data);
				info.Add(key, data);
			}
		}
	}

	protected static void ReadCrazyPackage(XmlNode xmlnode, ref Crazy_Package data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if ("Award" == childNode.Name)
			{
				Crazy_Package_Item crazy_Package_Item = new Crazy_Package_Item();
				crazy_Package_Item.awardid = int.Parse(xmlElement.GetAttribute("awardid").Trim());
				crazy_Package_Item.type = (Crazy_Award_Item_Type)int.Parse(xmlElement.GetAttribute("type").Trim());
				crazy_Package_Item.typeid = int.Parse(xmlElement.GetAttribute("typeid").Trim());
				crazy_Package_Item.rate = float.Parse(xmlElement.GetAttribute("my:rate").Trim());
				data.item.Add(crazy_Package_Item);
			}
		}
	}

	public static Crazy_Package GetPackageInfo(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_Package value = new Crazy_Package();
		if (info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
