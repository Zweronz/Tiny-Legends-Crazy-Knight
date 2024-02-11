using System.Collections.Generic;
using System.Xml;

public class Crazy_Process
{
	public class AssignStage
	{
		public int landid;

		public int waveid;

		public Crazy_LevelType leveltype;

		public int level;
	}

	protected static Dictionary<int, Crazy_Process> info;

	public int id;

	public int condition;

	public bool stage;

	public List<AssignStage> stages = new List<AssignStage>();

	public int action;

	public int loadingid;

	public int mapstory;

	public int stagestory;

	public int completestory;

	public int present;

	public int seq;

	public Crazy_Process(int _id)
	{
		id = _id;
	}

	public static void Initialize()
	{
		if (info == null)
		{
			info = new Dictionary<int, Crazy_Process>();
			ReadXml("Crazy_Process");
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
				Crazy_Process data = new Crazy_Process(key);
				data.condition = int.Parse(xmlElement.GetAttribute("condition").Trim());
				data.stage = bool.Parse(xmlElement.GetAttribute("stage").Trim());
				data.action = int.Parse(xmlElement.GetAttribute("action").Trim());
				data.mapstory = int.Parse(xmlElement.GetAttribute("mapstory").Trim());
				data.stagestory = int.Parse(xmlElement.GetAttribute("stagestory").Trim());
				data.completestory = int.Parse(xmlElement.GetAttribute("completestory").Trim());
				data.present = int.Parse(xmlElement.GetAttribute("present").Trim());
				data.loadingid = int.Parse(xmlElement.GetAttribute("my:loadingid").Trim());
				data.seq = int.Parse(xmlElement.GetAttribute("my:seq").Trim());
				ReadStage(childNode, ref data);
				info.Add(key, data);
			}
		}
	}

	protected static void ReadStage(XmlNode xmlnode, ref Crazy_Process data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if ("Stage" == childNode.Name)
			{
				AssignStage assignStage = new AssignStage();
				assignStage.landid = int.Parse(xmlElement.GetAttribute("landid").Trim());
				assignStage.waveid = int.Parse(xmlElement.GetAttribute("waveid").Trim());
				assignStage.leveltype = (Crazy_LevelType)int.Parse(xmlElement.GetAttribute("my:type").Trim());
				assignStage.level = int.Parse(xmlElement.GetAttribute("my:level").Trim());
				data.stages.Add(assignStage);
			}
		}
	}

	public static Crazy_Process FindNextProcessId(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_Process value = new Crazy_Process(0);
		if (info.TryGetValue(id, out value))
		{
			int num = 100000;
			Crazy_Process result = null;
			{
				foreach (Crazy_Process value2 in info.Values)
				{
					if (value2.seq > value.seq && value2.seq <= num)
					{
						num = value2.seq;
						result = value2;
					}
				}
				return result;
			}
		}
		return null;
	}

	public static Crazy_Process GetProcessInfo(int id)
	{
		if (info == null)
		{
			Initialize();
		}
		Crazy_Process value = new Crazy_Process(0);
		if (info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
