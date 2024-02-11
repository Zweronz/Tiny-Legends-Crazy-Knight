using System.Collections.Generic;
using UnityEngine;

public static class PropsAction
{
	private enum E_Props
	{
		HP1 = 0,
		SPEED1 = 1,
		INVINCIBLE1 = 2,
		HP2 = 3,
		SPEED2 = 4,
		INVINCIBLE2 = 5
	}

	public static Crazy_PlayerControl playerControl;

	private static Dictionary<string, E_Props> mProps = new Dictionary<string, E_Props>();

	private static bool m_bInit = false;

	public static void Init()
	{
		if (!m_bInit)
		{
			mProps.Add("hp1", E_Props.HP1);
			mProps.Add("speed1", E_Props.SPEED1);
			mProps.Add("wudi1", E_Props.INVINCIBLE1);
			mProps.Add("hp2", E_Props.HP2);
			mProps.Add("speed2", E_Props.SPEED2);
			mProps.Add("wudi2", E_Props.INVINCIBLE2);
			m_bInit = true;
		}
	}

	public static void UseProp(string name)
	{
		E_Props value;
		if (mProps.TryGetValue(name, out value))
		{
			switch (value)
			{
			case E_Props.HP1:
				playerControl.PlayHealUp(1);
				break;
			case E_Props.SPEED1:
				playerControl.PlaySpeedUp(1.3f, 3f);
				break;
			case E_Props.INVINCIBLE1:
				playerControl.OnInvincible2(3f);
				break;
			case E_Props.HP2:
				playerControl.PlayHealUp(3);
				break;
			case E_Props.SPEED2:
				playerControl.PlaySpeedUp(1.3f, 8f);
				break;
			case E_Props.INVINCIBLE2:
				playerControl.OnInvincible2(6f);
				break;
			default:
				Debug.Log("unknow prop");
				break;
			}
		}
	}
}
