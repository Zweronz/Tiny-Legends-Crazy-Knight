using UnityEngine;

public class Crazy_GameCenter
{
	private static Crazy_GameCenter s_instance;

	private int[] m_GameCenter;

	public static Crazy_GameCenter instance
	{
		get
		{
			if (s_instance == null)
			{
				s_instance = new Crazy_GameCenter();
			}
			return s_instance;
		}
	}

	public int[] GameCenter
	{
		get
		{
			return m_GameCenter;
		}
		set
		{
			m_GameCenter = value;
			for (int i = 0; i < m_GameCenter.Length; i++)
			{
				PlayerPrefs.SetInt("GameCenter" + (i + 1).ToString("D02"), m_GameCenter[i]);
			}
		}
	}

	public Crazy_GameCenter()
	{
		m_GameCenter = new int[8];
		for (int i = 0; i < m_GameCenter.Length; i++)
		{
			m_GameCenter[i] = PlayerPrefs.GetInt("GameCenter" + (i + 1).ToString("D02"));
		}
	}

	public void UpdateGameCenterLeaderBoard(int param)
	{
		if (GameCenterPlugin.IsSupported() && GameCenterPlugin.IsLogin())
		{
			GameCenterPlugin.SubmitScore("com.trinitigame.tinylegends1crazyknight.l1", param);
		}
	}

	public void UpdateGameCenterAchievement()
	{
		if (GameCenterPlugin.IsSupported() && GameCenterPlugin.IsLogin())
		{
			for (int i = 0; i < m_GameCenter.Length; i++)
			{
				GameCenterPlugin.SubmitAchievement("com.trinitigame.tinylegends1crazyknight.a" + (i + 1), m_GameCenter[i]);
			}
		}
	}

	public void UpdateGameCenterData()
	{
		int[] gameCenter = instance.GameCenter;
		int bossLevel = Crazy_Data.CurData().GetBossLevel();
		if (bossLevel > 30)
		{
			gameCenter[5] = 100;
		}
		if (bossLevel > 25)
		{
			gameCenter[4] = 100;
		}
		if (bossLevel > 20)
		{
			gameCenter[3] = 100;
		}
		if (bossLevel > 15)
		{
			gameCenter[2] = 100;
		}
		if (bossLevel > 10)
		{
			gameCenter[1] = 100;
		}
		if (bossLevel > 5)
		{
			gameCenter[0] = 100;
		}
		if (Crazy_GlobalData.max_single_kill_number >= 30)
		{
			gameCenter[7] = 100;
		}
		int killNumber = Crazy_Data.CurData().GetKillNumber();
		gameCenter[6] = Mathf.Clamp(killNumber / 10, 0, 100);
		instance.GameCenter = gameCenter;
	}
}
