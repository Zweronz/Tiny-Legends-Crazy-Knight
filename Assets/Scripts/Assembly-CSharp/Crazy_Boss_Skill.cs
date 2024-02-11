public class Crazy_Boss_Skill
{
	public Crazy_Boss_Skill_Status cur_status;

	public Crazy_Boss_Skill_Process cur_process;

	public Crazy_Boss_Skill_Data cur_data;

	public float lastcooldowntime;

	public Crazy_Boss_Skill()
	{
		cur_status = Crazy_Boss_Skill_Status.CoolDown;
		cur_process = Crazy_Boss_Skill_Process.NotUse;
		cur_data = new Crazy_Boss_Skill_Data();
		lastcooldowntime = 0f;
	}
}
