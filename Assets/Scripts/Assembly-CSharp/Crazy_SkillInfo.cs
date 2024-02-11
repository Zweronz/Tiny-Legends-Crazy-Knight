using System.Collections.Generic;

public class Crazy_SkillInfo
{
	public static Dictionary<Crazy_WeaponSkillType, Crazy_SkillInfo> m_info;

	public Crazy_WeaponSkillType type;

	public string name;

	public string explain;

	public static void Initialize()
	{
		if (m_info == null)
		{
			m_info = new Dictionary<Crazy_WeaponSkillType, Crazy_SkillInfo>();
			InitializeString();
		}
	}

	protected static void InitializeString()
	{
		AddString(Crazy_WeaponSkillType.HealUp, "Resilience", "Increases max HP.");
		AddString(Crazy_WeaponSkillType.ReduceMove, "Frost Aura", "Slows nearby enemy movement.");
		AddString(Crazy_WeaponSkillType.ReduceSpeed, "Crippling Aura", "Slows nearby enemy attack speed.");
		AddString(Crazy_WeaponSkillType.SwordSkill, "Doom Slash", "Deal damage to all enemies in front of you.");
		AddString(Crazy_WeaponSkillType.HammerSkill, "Ground Pound", "Slam the ground and damage enemies on all\nsides.");
		AddString(Crazy_WeaponSkillType.AxeSkill, "Cyclone", "Spin into a frenzy and damage all enemies in\nyour path.");
		AddString(Crazy_WeaponSkillType.BowSkill, "Arrow Rain", "Rain arrows on all foes in a large area.");
		AddString(Crazy_WeaponSkillType.StaffSkill, "Meteorite Rain", "Rain down meteorites on enemies.");
	}

	protected static void AddString(Crazy_WeaponSkillType _type, string _name, string _explain)
	{
		Crazy_SkillInfo crazy_SkillInfo = new Crazy_SkillInfo();
		crazy_SkillInfo.type = _type;
		crazy_SkillInfo.name = _name;
		crazy_SkillInfo.explain = _explain;
		m_info.Add(_type, crazy_SkillInfo);
	}

	public static Crazy_SkillInfo GetCrazySkillInfo(Crazy_WeaponSkillType id)
	{
		if (m_info == null)
		{
			Initialize();
		}
		Crazy_SkillInfo value = new Crazy_SkillInfo();
		if (m_info.TryGetValue(id, out value))
		{
			return value;
		}
		return null;
	}
}
