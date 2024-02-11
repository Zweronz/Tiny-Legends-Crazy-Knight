using System.Collections.Generic;
using System.Xml;

public class Crazy_ComboLevel
{
	protected static Dictionary<int, Crazy_ComboData> m_ComboLevel;

	public static void Initialize()
	{
		if (m_ComboLevel == null)
		{
			m_ComboLevel = new Dictionary<int, Crazy_ComboData>();
			ReadXml("Crazy_Combo");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("Combo" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int num = int.Parse(xmlElement.GetAttribute("level").Trim());
				int count = int.Parse(xmlElement.GetAttribute("combocount").Trim());
				Crazy_ComboData value = new Crazy_ComboData(num, count);
				m_ComboLevel.Add(num, value);
			}
		}
	}

	public static int GetComboLevelCount()
	{
		if (m_ComboLevel == null)
		{
			Initialize();
		}
		return m_ComboLevel.Count;
	}

	public static int FindComboLevel(int combo)
	{
		if (m_ComboLevel == null)
		{
			Initialize();
		}
		int result = 0;
		for (int i = 1; i <= m_ComboLevel.Count; i++)
		{
			if (combo >= m_ComboLevel[i].count)
			{
				result = i;
			}
		}
		return result;
	}

	public static int GetComboByLevel(int level)
	{
		if (m_ComboLevel == null)
		{
			Initialize();
		}
		if (level <= m_ComboLevel.Count)
		{
			return m_ComboLevel[level].count;
		}
		return 0;
	}
}
