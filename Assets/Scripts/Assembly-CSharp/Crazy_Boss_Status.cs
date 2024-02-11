public class Crazy_Boss_Status
{
	public int id;

	public int priority;

	public Crazy_Boss_Status_ConditionType condition;

	public float conditionparam;

	public Crazy_Boss_Status_Data data = new Crazy_Boss_Status_Data();

	public Crazy_Boss_Skill GetPrioritySkill()
	{
		Crazy_Boss_Skill crazy_Boss_Skill = null;
		foreach (Crazy_Boss_Skill key in data.m_skill.Keys)
		{
			if (key.cur_status == Crazy_Boss_Skill_Status.Active && (crazy_Boss_Skill == null || crazy_Boss_Skill.cur_data.priority > key.cur_data.priority))
			{
				crazy_Boss_Skill = key;
			}
		}
		return crazy_Boss_Skill;
	}

	public void updateSkillPriority()
	{
		foreach (Crazy_Boss_Skill key in data.m_skill.Keys)
		{
			if (data.m_skill[key] == -1)
			{
				key.cur_data.priority = key.cur_data.originalpriority;
			}
			else
			{
				key.cur_data.priority = data.m_skill[key];
			}
		}
	}
}
