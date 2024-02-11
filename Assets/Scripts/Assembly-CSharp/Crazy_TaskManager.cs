using UnityEngine;

public sealed class Crazy_TaskManager
{
	private static Crazy_TaskManager s_instance;

	private GameObject hint;

	public static Crazy_TaskManager GetInstance()
	{
		if (s_instance == null)
		{
			s_instance = new Crazy_TaskManager();
		}
		return s_instance;
	}

	public static void DeleteInstance()
	{
		if (s_instance != null)
		{
			s_instance = null;
		}
	}

	public void Register(GameObject t)
	{
		hint = t;
	}

	public void Unregister()
	{
		hint = null;
	}

	public void updateTask(Crazy_TaskId id, int branch, float param)
	{
		bool flag = SetTask((int)id, branch, param);
		if (flag)
		{
			Settlement((int)id);
		}
		if (hint != null && flag)
		{
			hint.SendMessage("ShowTask", (int)id);
		}
	}

	protected void Settlement(int id)
	{
		Crazy_Achievement achievementInfo = Crazy_Achievement.GetAchievementInfo(id);
		Crazy_Global.Prize(achievementInfo.award);
	}

	protected bool SetTask(int id, int branch, float param)
	{
		switch (id)
		{
		case 0:
			return Crazy_Data.CurData().SetTaskBranch(id, branch, Mathf.Clamp(Crazy_Data.CurData().GetTaskBranch(id, branch) + param / 500f, 0f, 1f));
		case 1:
			return Crazy_Data.CurData().SetTaskBranch(id, branch, Mathf.Clamp(Crazy_Data.CurData().GetTaskBranch(id, branch) + param / 1000f, 0f, 1f));
		case 2:
			return Crazy_Data.CurData().SetTaskBranch(id, branch, Mathf.Clamp(Crazy_Data.CurData().GetTaskBranch(id, branch) + param / 5000f, 0f, 1f));
		case 3:
			return Crazy_Data.CurData().SetTaskBranch(id, branch, Mathf.Clamp(Crazy_Data.CurData().GetTaskBranch(id, branch) + param / 10000f, 0f, 1f));
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
			return Crazy_Data.CurData().SetTaskBranch(id, branch, 1f);
		case 22:
			return Crazy_Data.CurData().SetTaskBranch(id, branch, Mathf.Clamp(Crazy_Data.CurData().GetTaskBranch(id, branch) + param / 10000f, 0f, 1f));
		case 23:
			return Crazy_Data.CurData().SetTaskBranch(id, branch, Mathf.Clamp(Crazy_Data.CurData().GetTaskBranch(id, branch) + param / 100000f, 0f, 1f));
		case 24:
			return Crazy_Data.CurData().SetTaskBranch(id, branch, Mathf.Clamp(Crazy_Data.CurData().GetTaskBranch(id, branch) + param / 1000000f, 0f, 1f));
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
			return Crazy_Data.CurData().SetTaskBranch(id, branch, 1f);
		default:
			return false;
		}
	}
}
