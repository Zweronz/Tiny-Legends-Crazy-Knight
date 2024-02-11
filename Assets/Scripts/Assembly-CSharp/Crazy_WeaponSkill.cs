using UnityEngine;

public class Crazy_WeaponSkill
{
	protected Crazy_PlayerControl m_user;

	protected Crazy_WeaponSkillType m_type;

	protected PlayerAnimationSynchronizer m_user_image;

	protected float m_param;

	public void Init(Crazy_WeaponSkillType type, float param)
	{
		m_type = type;
		m_param = param;
	}

	public void SetUserImage(PlayerAnimationSynchronizer user)
	{
		m_user_image = user;
	}

	public void SetUser(Crazy_PlayerControl user)
	{
		m_user = user;
	}

	public new Crazy_WeaponSkillType GetType()
	{
		return m_type;
	}

	public float GetParam()
	{
		return m_param;
	}

	public void AddSkillImage()
	{
		switch (m_type)
		{
		case Crazy_WeaponSkillType.ReduceMove:
			m_user_image.SetReduceMove(true, Mathf.Clamp01(m_param));
			break;
		case Crazy_WeaponSkillType.ReduceSpeed:
			m_user_image.SetReduceSpeed(true, m_param);
			break;
		}
	}

	public void AddSkill()
	{
		switch (m_type)
		{
		case Crazy_WeaponSkillType.HealUp:
			m_user.SetMaxHealth((int)m_param);
			break;
		case Crazy_WeaponSkillType.ReduceMove:
			m_user.SetReduceMove(true, Mathf.Clamp01(m_param));
			break;
		case Crazy_WeaponSkillType.ReduceSpeed:
			m_user.SetReduceSpeed(true, m_param);
			break;
		case Crazy_WeaponSkillType.SwordSkill:
		case Crazy_WeaponSkillType.HammerSkill:
		case Crazy_WeaponSkillType.AxeSkill:
		case Crazy_WeaponSkillType.BowSkill:
		case Crazy_WeaponSkillType.StaffSkill:
			m_user.SetSkill(true);
			break;
		}
	}

	public int GetSkillLevel()
	{
		Crazy_WeaponSkillType type = m_type;
		if (type == Crazy_WeaponSkillType.HealUp)
		{
			return (int)m_param;
		}
		return 1;
	}

	public void RemoveSkill()
	{
		switch (m_type)
		{
		case Crazy_WeaponSkillType.HealUp:
			m_user.SetMaxHealth((int)m_param * -1);
			break;
		case Crazy_WeaponSkillType.ReduceMove:
			m_user.SetReduceMove(false, 0f);
			break;
		case Crazy_WeaponSkillType.ReduceSpeed:
			m_user.SetReduceSpeed(false, 0f);
			break;
		case Crazy_WeaponSkillType.SwordSkill:
		case Crazy_WeaponSkillType.HammerSkill:
		case Crazy_WeaponSkillType.AxeSkill:
		case Crazy_WeaponSkillType.BowSkill:
		case Crazy_WeaponSkillType.StaffSkill:
			m_user.SetSkill(false);
			break;
		}
	}
}
