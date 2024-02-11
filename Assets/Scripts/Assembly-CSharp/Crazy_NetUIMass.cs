using System.Collections.Generic;
using System.Xml;

public class Crazy_NetUIMass
{
	public int id;

	public string bk;

	public string left;

	public string right;

	protected static Dictionary<int, Crazy_NetUIMass> info;

	public Crazy_NetUIMass()
	{
	}

	public Crazy_NetUIMass(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_NetUIMass>();
			ReadXml("Crazy_NetUIMass");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Mass" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int key = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_NetUIMass crazy_NetUIMass = new Crazy_NetUIMass(key);
				crazy_NetUIMass.bk = xmlElement.GetAttribute("bk").Trim();
				crazy_NetUIMass.left = xmlElement.GetAttribute("left").Trim();
				crazy_NetUIMass.right = xmlElement.GetAttribute("right").Trim();
				info.Add(key, crazy_NetUIMass);
			}
		}
	}

	public static Crazy_NetUIMass GetNetUIMassInfo(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_NetUIMass value = new Crazy_NetUIMass();
		if (info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
