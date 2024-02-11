using System.Collections.Generic;
using System.Xml;

public class Crazy_Drop
{
	public int id;

	public List<int> item = new List<int>();

	protected static Dictionary<int, Crazy_Drop> info;

	public Crazy_Drop()
	{
	}

	public Crazy_Drop(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_Drop>();
			ReadXml("Crazy_Drop");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Drop" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int key = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_Drop data = new Crazy_Drop(key);
				ReadCrazyDrop(childNode, ref data);
				info.Add(key, data);
			}
		}
	}

	protected static void ReadCrazyDrop(XmlNode xmlnode, ref Crazy_Drop data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if ("Package" == childNode.Name)
			{
				int num = int.Parse(xmlElement.GetAttribute("packageid").Trim());
				data.item.Add(num);
			}
		}
	}

	public static Crazy_Drop GetDropInfo(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_Drop value = new Crazy_Drop();
		if (info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
