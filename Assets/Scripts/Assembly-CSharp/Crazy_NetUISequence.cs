using System.Collections.Generic;
using System.Xml;

public class Crazy_NetUISequence
{
	public int id;

	public List<int> mass = new List<int>();

	protected static Dictionary<int, Crazy_NetUISequence> info;

	public Crazy_NetUISequence()
	{
	}

	public Crazy_NetUISequence(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_NetUISequence>();
			ReadXml("Crazy_NetUISequence");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Sequence" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int key = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_NetUISequence data = new Crazy_NetUISequence(key);
				ReadMass(childNode, ref data);
				info.Add(key, data);
			}
		}
	}

	protected static void ReadMass(XmlNode xmlnode, ref Crazy_NetUISequence data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if ("Mass" == childNode.Name)
			{
				data.mass.Add(int.Parse(xmlElement.GetAttribute("massid").Trim()));
			}
		}
	}

	public static Crazy_NetUISequence GetNetUISequenceInfo(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_NetUISequence value = new Crazy_NetUISequence();
		if (info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
