using System.Collections.Generic;
using System.Xml;

public class Crazy_Language
{
	protected static Dictionary<int, Crazy_Language> info;

	public int id;

	public string content;

	public Crazy_Language(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_Language>();
			ReadXml("Crazy_Language");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Sentence" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int key = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_Language crazy_Language = new Crazy_Language(key);
				crazy_Language.content = xmlElement.GetAttribute("contents");
				info.Add(key, crazy_Language);
			}
		}
	}

	public static string GetLanguage(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_Language value = new Crazy_Language(0);
		if (info.TryGetValue(id, out value))
		{
			return value.content;
		}
		return null;
	}
}
