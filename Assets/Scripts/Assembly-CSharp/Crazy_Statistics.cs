using System.Collections.Generic;

public class Crazy_Statistics
{
	public static Dictionary<Crazy_MonsterType, Dictionary<int, int>> monster_killStatistics = new Dictionary<Crazy_MonsterType, Dictionary<int, int>>();

	public static bool isfinish = true;

	public static int mkillnumber = 0;

	public static int mgold = 0;

	public static int ccombo = 0;

	public static int cgold = 0;

	public static void AddMonsterKillNumber(Crazy_MonsterType type, int id)
	{
		Dictionary<int, int> value;
		if (monster_killStatistics.TryGetValue(type, out value))
		{
			int value2 = 0;
			if (value.TryGetValue(id, out value2))
			{
				value2++;
				value[id] = value2;
				monster_killStatistics[type] = value;
			}
			else
			{
				monster_killStatistics[type].Add(id, 1);
			}
		}
		else
		{
			value = new Dictionary<int, int>();
			value.Add(id, 1);
			monster_killStatistics.Add(type, value);
		}
	}

	public static void ResetMonsterKillNumber()
	{
		monster_killStatistics.Clear();
	}
}
