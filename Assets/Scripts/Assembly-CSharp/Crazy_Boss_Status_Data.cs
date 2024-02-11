using System.Collections.Generic;

public class Crazy_Boss_Status_Data
{
	public Dictionary<Crazy_Boss_Skill, int> m_skill;

	public float moverate;

	public float preattacktimerate;

	public float endattacktimerate;

	public Crazy_Boss_Status_Data()
	{
		m_skill = new Dictionary<Crazy_Boss_Skill, int>();
		moverate = 1f;
		preattacktimerate = 1f;
		endattacktimerate = 1f;
	}
}
