using System.Collections.Generic;
using UnityEngine;

public class Crazy_GlobalData
{
	public enum ZombieUser
	{
		None = 0,
		ZombieUser7 = 1,
		ZombieUser14 = 2,
		ZombieUser21 = 3
	}

	public static int OpenClikTimes = 0;

	public static Dictionary<int, GameObject> enemyList;

	public static int cur_wave = 1;

	public static int cur_stage = 0;

	public static int cur_level = 1;

	public static int cur_scene_id = 0;

	public static int cur_land_id = -1;

	public static Crazy_LevelType cur_leveltype = Crazy_LevelType.Normal1;

	public static string next_scene = string.Empty;

	public static float cur_player_time = 0f;

	public static int cur_kill_number = 0;

	public static List<int> unlock_weaponid = new List<int>();

	public static string maphinticon = null;

	public static int mapiconpos = -1;

	public static int max_single_kill_number = 0;

	public static string endpicture = null;

	public static Crazy_GameState cur_game_state = Crazy_GameState.PreGame;

	public static Crazy_Process cur_process = null;

	public static string pre_scene = string.Empty;

	public static int cur_used_money = 0;

	public static int cur_tc_times = 0;

	public static int cur_tc_num = 0;

	public static int cur_UI1Times = 0;

	public static int cur_UI2Times = 0;

	public static int cur_UI3Times = 0;

	public static int cur_UI4Times = 0;

	public static int cur_UI5Times = 0;

	public static int cur_UI6Times = 0;

	public static int cur_ui = 0;

	public static int cur_fight_num = 0;

	public static int cur_fight_longtime = 0;

	public static int cur_fight_shorttime = -1;

	public static int cur_fight_avgtime = 0;

	public static int cur_fight_succ = 0;

	public static int m_prelevel = -1;

	public static int m_curlevel = -1;

	public static bool isreview = false;

	public static int iapweapon = -1;

	public static int cur_EquipPropID = 1;

	public static int cur_FocusPropID = 1;

	public static int cur_EquipPropNum = 0;

	public static bool g_bActiveUser = false;

	public static ZombieUser g_ZombieUser = ZombieUser.None;

	public static bool m_bHD = true;

	public static bool m_bShowDailyReward = false;
}
