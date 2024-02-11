using System.Collections.Generic;
using System.Xml;

public class Crazy_PropItem
{
	public class PriceItem
	{
		public string strPayType;

		public int nUseCount;

		public string nPrice;
	}

	public List<PriceItem> priceItems = new List<PriceItem>();

	public int m_nPropID;

	public string m_strPropName;

	public string m_strSmallIcon;

	public string m_strSmallIconHL;

	public string m_strMiddleIcon;

	public string m_strBigIcon;

	public string m_strEffect;

	public string m_strDesc;

	public int m_fCDTime;

	protected static Dictionary<int, Crazy_PropItem> items;

	public Crazy_PropItem()
	{
	}

	public Crazy_PropItem(int _id)
	{
		m_nPropID = _id;
	}

	public static void Initialize()
	{
		if (items == null)
		{
			items = new Dictionary<int, Crazy_PropItem>();
			ReadXml("PropItems");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if ("PropItem" == childNode.Name)
			{
				XmlElement xmlElement = (XmlElement)childNode;
				int num = int.Parse(xmlElement.GetAttribute("id").Trim());
				Crazy_PropItem data = new Crazy_PropItem(num);
				data.m_strPropName = xmlElement.GetAttribute("name").Trim();
				data.m_strSmallIcon = xmlElement.GetAttribute("smallIcon").Trim();
				data.m_strSmallIconHL = xmlElement.GetAttribute("smallIconHL").Trim();
				data.m_strMiddleIcon = xmlElement.GetAttribute("middleIcon").Trim();
				data.m_strBigIcon = xmlElement.GetAttribute("bigIcon").Trim();
				data.m_strDesc = xmlElement.GetAttribute("desc").Trim();
				data.m_fCDTime = int.Parse(xmlElement.GetAttribute("my:CDTime").Trim());
				ReadPriceItems(childNode, ref data);
				items.Add(num, data);
			}
		}
	}

	protected static void ReadPriceItems(XmlNode xmlnode, ref Crazy_PropItem data)
	{
		foreach (XmlNode childNode in xmlnode.ChildNodes)
		{
			XmlElement xmlElement = (XmlElement)childNode;
			if ("Price" == childNode.Name)
			{
				PriceItem priceItem = new PriceItem();
				priceItem.strPayType = xmlElement.GetAttribute("payType").Trim();
				priceItem.nPrice = xmlElement.GetAttribute("usecount").Trim();
				priceItem.nPrice = xmlElement.GetAttribute("price").Trim();
				data.priceItems.Add(priceItem);
			}
		}
	}

	public static List<Crazy_PropItem> GetSkillItems()
	{
		if (items == null)
		{
			Initialize();
		}
		List<Crazy_PropItem> list = new List<Crazy_PropItem>();
		foreach (Crazy_PropItem value in items.Values)
		{
			list.Add(value);
		}
		return list;
	}

	public static Crazy_PropItem GetSkillItem(int id)
	{
		if (items == null)
		{
			Initialize();
		}
		Crazy_PropItem value = new Crazy_PropItem();
		if (items.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
