using System.Collections.Generic;
using System.Xml;

public class Crazy_StoryProcess
{
	protected static Dictionary<int, Crazy_StoryProcess> info;

	public int id;

	public string iconname;

	public int textid;

	public Crazy_StoryProcess(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_StoryProcess>();
			ReadXml("Crazy_Story");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Process" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int key = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_StoryProcess crazy_StoryProcess = new Crazy_StoryProcess(key);
				crazy_StoryProcess.iconname = xmlElement.GetAttribute("texture");
				crazy_StoryProcess.textid = int.Parse(xmlElement.GetAttribute("textid").Trim());
				info.Add(key, crazy_StoryProcess);
			}
		}
	}

	public static Crazy_StoryProcess GetStoryProcessInfo(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_StoryProcess value = new Crazy_StoryProcess(0);
		if (info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
