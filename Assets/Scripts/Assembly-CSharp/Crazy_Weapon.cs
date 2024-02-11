using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Crazy_Weapon
{
	public int id;

	public string dcid;

	public int order;

	public string name;

	public string iconname;

	public string loadpath;

	public Crazy_Weapon_Type type;

	public string type_name;

	public float length;

	public int level;

	public int price;

	public Crazy_Price_Type price_type;

	public int damage;

	public int speed;

	public float move;

	public int need;

	public Crazy_Weapon_Enchant enchant;

	public bool isshow;

	public Vector3 modifyPos;

	public Vector3 modifyAngle;

	public List<Crazy_WeaponSkill> skill;

	public static List<Crazy_Weapon> weaponinfolist;

	public void AddWeaponSkillImage(PlayerAnimationSynchronizer pas)
	{
		if (skill == null)
		{
			return;
		}
		foreach (Crazy_WeaponSkill item in skill)
		{
			item.SetUserImage(pas);
			item.AddSkillImage();
		}
	}

	public void AddWeaponSkill(Crazy_PlayerControl cpc)
	{
		if (skill == null)
		{
			return;
		}
		foreach (Crazy_WeaponSkill item in skill)
		{
			item.SetUser(cpc);
			item.AddSkill();
		}
	}

	public void RemoveWeaponSkill()
	{
		if (skill == null)
		{
			return;
		}
		foreach (Crazy_WeaponSkill item in skill)
		{
			item.RemoveSkill();
		}
	}

	public static void Initialize()
	{
		if (weaponinfolist == null)
		{
			weaponinfolist = new List<Crazy_Weapon>();
			ReadXml("Crazy_Weapon");
		}
	}

	protected static void ReadXml(string path)
	{
		XmlDocument xmlDocument = Crazy_Global.ReadXml(path);
		XmlNode documentElement = xmlDocument.DocumentElement;
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			if (!("Weapon" == childNode.Name))
			{
				continue;
			}
			XmlElement xmlElement = (XmlElement)childNode;
			Crazy_Weapon crazy_Weapon = new Crazy_Weapon();
			crazy_Weapon.id = int.Parse(xmlElement.GetAttribute("id").Trim());
			crazy_Weapon.dcid = xmlElement.GetAttribute("my:DCid").Trim();
			crazy_Weapon.order = int.Parse(xmlElement.GetAttribute("my:order").Trim());
			crazy_Weapon.name = xmlElement.GetAttribute("name").Trim();
			crazy_Weapon.iconname = xmlElement.GetAttribute("iconname").Trim();
			crazy_Weapon.loadpath = xmlElement.GetAttribute("loadpath").Trim();
			crazy_Weapon.type = (Crazy_Weapon_Type)int.Parse(xmlElement.GetAttribute("type").Trim());
			crazy_Weapon.type_name = xmlElement.GetAttribute("type_name").Trim();
			crazy_Weapon.length = float.Parse(xmlElement.GetAttribute("length").Trim());
			crazy_Weapon.level = int.Parse(xmlElement.GetAttribute("level").Trim());
			crazy_Weapon.price = int.Parse(xmlElement.GetAttribute("price").Trim());
			crazy_Weapon.price_type = (Crazy_Price_Type)int.Parse(xmlElement.GetAttribute("price_type").Trim());
			crazy_Weapon.damage = int.Parse(xmlElement.GetAttribute("damage").Trim());
			crazy_Weapon.speed = int.Parse(xmlElement.GetAttribute("speed").Trim());
			crazy_Weapon.move = float.Parse(xmlElement.GetAttribute("move").Trim());
			crazy_Weapon.need = int.Parse(xmlElement.GetAttribute("need").Trim());
			crazy_Weapon.enchant = (Crazy_Weapon_Enchant)int.Parse(xmlElement.GetAttribute("my:enchant").Trim());
			crazy_Weapon.isshow = bool.Parse(xmlElement.GetAttribute("my:isshow").Trim());
			foreach (XmlNode childNode2 in childNode.ChildNodes)
			{
				if ("ModifyPos" == childNode2.Name)
				{
					XmlElement xmlElement2 = (XmlElement)childNode2;
					Vector3 vector = default(Vector3);
					vector.x = float.Parse(xmlElement2.GetAttribute("Px").Trim());
					vector.y = float.Parse(xmlElement2.GetAttribute("Py").Trim());
					vector.z = float.Parse(xmlElement2.GetAttribute("Pz").Trim());
					crazy_Weapon.modifyPos = vector;
				}
				else if ("ModifyAngle" == childNode2.Name)
				{
					XmlElement xmlElement3 = (XmlElement)childNode2;
					Vector3 vector2 = default(Vector3);
					vector2.x = float.Parse(xmlElement3.GetAttribute("Ax").Trim());
					vector2.y = float.Parse(xmlElement3.GetAttribute("Ay").Trim());
					vector2.z = float.Parse(xmlElement3.GetAttribute("Az").Trim());
					crazy_Weapon.modifyAngle = vector2;
				}
				else
				{
					if (!("my:Skill" == childNode2.Name))
					{
						continue;
					}
					XmlElement xmlElement4 = (XmlElement)childNode2;
					Crazy_WeaponSkill crazy_WeaponSkill = new Crazy_WeaponSkill();
					int num = int.Parse(xmlElement4.GetAttribute("my:SkillType").Trim());
					if (num != 0)
					{
						crazy_WeaponSkill.Init((Crazy_WeaponSkillType)num, float.Parse(xmlElement4.GetAttribute("my:SkillParam")));
						if (crazy_Weapon.skill == null)
						{
							crazy_Weapon.skill = new List<Crazy_WeaponSkill>();
						}
						crazy_Weapon.skill.Add(crazy_WeaponSkill);
					}
				}
			}
			weaponinfolist.Add(crazy_Weapon);
		}
	}

	public static Crazy_Weapon FindWeaponByOrder(List<Crazy_Weapon> list, int order)
	{
		int num = 1000000;
		Crazy_Weapon result = null;
		foreach (Crazy_Weapon item in list)
		{
			int num2 = item.order - order;
			if (num2 >= 0 && num2 < num)
			{
				result = item;
				num = num2;
			}
		}
		return result;
	}

	public static Crazy_Weapon FindWeaponByID(List<Crazy_Weapon> list, int id)
	{
		foreach (Crazy_Weapon item in list)
		{
			if (item.id == id)
			{
				return item;
			}
		}
		return null;
	}

	public static List<Crazy_Weapon> ReadWeaponInfo(Crazy_Weapon_Type type)
	{
		if (weaponinfolist == null)
		{
			Initialize();
		}
		List<Crazy_Weapon> list = new List<Crazy_Weapon>();
		foreach (Crazy_Weapon item in weaponinfolist)
		{
			if (item.type == type)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public static List<Crazy_Weapon> ReadWeaponInfo()
	{
		if (weaponinfolist == null)
		{
			Initialize();
		}
		return weaponinfolist;
	}
}
