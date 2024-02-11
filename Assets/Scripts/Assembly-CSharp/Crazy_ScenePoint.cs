using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Crazy_ScenePoint
{
	protected static Dictionary<int, Crazy_ScenePoint> scencpoint;

	public int id;

	public List<Crazy_Point> point = new List<Crazy_Point>();

	public Crazy_ScenePoint(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (scencpoint == null)
		{
			scencpoint = new Dictionary<int, Crazy_ScenePoint>();
			ReadXml("Crazy_ScenePoint");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Scene" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int key = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_ScenePoint data = new Crazy_ScenePoint(key);
				ReadCrazyScenePoint(childNode, ref data);
				scencpoint.Add(key, data);
			}
		}
	}

	protected static void ReadCrazyScenePoint(XmlNode xmlnode, ref Crazy_ScenePoint data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if ("Point" == childNode.Name)
			{
				Crazy_Point item = default(Crazy_Point);
				item.point = new Vector2(float.Parse(xmlElement.GetAttribute("x").Trim()), float.Parse(xmlElement.GetAttribute("z").Trim()));
				item.range = float.Parse(xmlElement.GetAttribute("range").Trim());
				data.point.Add(item);
			}
		}
	}

	public static Crazy_ScenePoint GetCrazyScenePoint(int id)
	{
		if (scencpoint == null)
		{
			Initialize();
		}
		Crazy_ScenePoint value = new Crazy_ScenePoint(0);
		if (scencpoint.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}

	public static Crazy_Point GetRandomCrazyPoint(int id)
	{
		if (scencpoint == null)
		{
			Initialize();
		}
		Crazy_ScenePoint value = new Crazy_ScenePoint(0);
		if (scencpoint.TryGetValue(id, out value) && value.point.Count != 0)
		{
			return value.point[Random.Range(0, value.point.Count)];
		}
		return default(Crazy_Point);
	}
}
