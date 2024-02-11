public class BossSkillInfo
{
	public string objectid;

	public int skillid;

	public float seed;

	public BossSkillInfo(string _objectid, int _skillid, float _seed)
	{
		objectid = _objectid;
		skillid = _skillid;
		seed = _seed;
	}

	public BossSkillInfo()
	{
	}
}
