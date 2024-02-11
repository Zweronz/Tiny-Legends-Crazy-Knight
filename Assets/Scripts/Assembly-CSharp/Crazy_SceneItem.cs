using System.Collections.Generic;
using System.Xml;

public class Crazy_SceneItem
{
	public int sceneid;

	public int itempointid;

	protected static Dictionary<int, Crazy_SceneItem> info;

	public Crazy_SceneItem()
	{
	}

	public Crazy_SceneItem(int _id)
	{
		sceneid = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_SceneItem>();
			ReadXml("Crazy_SceneItem");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("SceneItem" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int num = int.Parse(xmlElement.GetAttribute("sceneid").Trim());
				Crazy_SceneItem crazy_SceneItem = new Crazy_SceneItem(num);
				crazy_SceneItem.itempointid = int.Parse(xmlElement.GetAttribute("itempointid").Trim());
				info.Add(num, crazy_SceneItem);
			}
		}
	}

	public static Crazy_SceneItem GetSceneItemInfo(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_SceneItem value = new Crazy_SceneItem();
		if (info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
