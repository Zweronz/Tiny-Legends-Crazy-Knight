using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Crazy_ItemPoint
{
	protected static Dictionary<int, Crazy_ItemPoint> itempoint;

	public int id;

	public List<Vector2> point = new List<Vector2>();

	public Crazy_ItemPoint(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (itempoint == null)
		{
			itempoint = new Dictionary<int, Crazy_ItemPoint>();
			ReadXml("Crazy_ItemPoint");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("ItemPoint" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int key = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_ItemPoint data = new Crazy_ItemPoint(key);
				ReadCrazyItemPoint(childNode, ref data);
				itempoint.Add(key, data);
			}
		}
	}

	protected static void ReadCrazyItemPoint(XmlNode xmlnode, ref Crazy_ItemPoint data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if ("Point" == childNode.Name)
			{
				Vector2 vector = default(Vector2);
				vector = new Vector2(float.Parse(xmlElement.GetAttribute("x").Trim()), float.Parse(xmlElement.GetAttribute("z").Trim()));
				data.point.Add(vector);
			}
		}
	}

	public static Crazy_ItemPoint GetCrazyItemPoint(int id)
	{
		if (itempoint == null)
		{
			Initialize();
		}
		Crazy_ItemPoint value = new Crazy_ItemPoint(0);
		if (itempoint.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
