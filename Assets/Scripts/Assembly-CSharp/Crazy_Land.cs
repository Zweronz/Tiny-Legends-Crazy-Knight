using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Crazy_Land
{
	protected static Dictionary<int, Crazy_Land> land;

	public int id;

	public List<Vector2> point = new List<Vector2>();

	public List<int> scene = new List<int>();

	public List<int> monster = new List<int>();

	public List<int> boss = new List<int>();

	public List<int> icon = new List<int>();

	public List<int> loading = new List<int>();

	public List<int> ranged = new List<int>();

	public List<int> rangedicon = new List<int>();

	public Crazy_Land(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (land == null)
		{
			land = new Dictionary<int, Crazy_Land>();
			ReadXml("Crazy_Land");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Land" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int key = int.Parse(xmlElement.GetAttribute("landid").Trim());
				Crazy_Land data = new Crazy_Land(key);
				ReadCrazyLand(childNode, ref data);
				land.Add(key, data);
			}
		}
	}

	protected static void ReadCrazyLand(XmlNode xmlnode, ref Crazy_Land data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if ("Point" == childNode.Name)
			{
				Vector2 item = new Vector2(float.Parse(xmlElement.GetAttribute("x").Trim()), float.Parse(xmlElement.GetAttribute("y").Trim()));
				data.point.Add(item);
			}
			else if ("Scene" == childNode.Name)
			{
				data.scene.Add(int.Parse(xmlElement.GetAttribute("sceneid").Trim()));
			}
			else if ("Monster" == childNode.Name)
			{
				data.monster.Add(int.Parse(xmlElement.GetAttribute("monsterid").Trim()));
			}
			else if ("Boss" == childNode.Name)
			{
				data.boss.Add(int.Parse(xmlElement.GetAttribute("bossid").Trim()));
			}
			else if ("my:Icon" == childNode.Name)
			{
				data.icon.Add(int.Parse(xmlElement.GetAttribute("my:IconId").Trim()));
			}
			else if ("my:Loading" == childNode.Name)
			{
				data.loading.Add(int.Parse(xmlElement.GetAttribute("my:LoadingId").Trim()));
			}
			else if ("my:Ranged" == childNode.Name)
			{
				data.ranged.Add(int.Parse(xmlElement.GetAttribute("my:rangedid").Trim()));
			}
			else if ("my:RangedIcon" == childNode.Name)
			{
				data.rangedicon.Add(int.Parse(xmlElement.GetAttribute("my:RangedIconId").Trim()));
			}
		}
	}

	public static Dictionary<int, Crazy_Land> GetLand()
	{
		if (land == null)
		{
			Initialize();
		}
		return land;
	}

	public static Crazy_Land GetLandinfo(int id)
	{
		if (land == null)
		{
			Initialize();
		}
		Crazy_Land value = new Crazy_Land(0);
		if (land.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
