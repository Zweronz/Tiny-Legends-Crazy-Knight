using System.Collections.Generic;

public class Crazy_Boss_Skill_Data
{
	public int id;

	public int priority;

	public int originalpriority;

	public float cooldowntime;

	public float seed;

	public float begintopreparerange;

	public bool isDelay;

	public float preparetime;

	public string preanimationname;

	public List<Crazy_SkillPoint> damagejudgment = new List<Crazy_SkillPoint>();

	public string useanimationname;

	public float endtime;

	public string endanimationname;
}
