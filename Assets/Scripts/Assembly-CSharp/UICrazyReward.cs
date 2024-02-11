using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICrazyReward : MonoBehaviour, TUIHandler
{
	public UtilUIDeath01 death01;

	public UtilUIDeath02 death02;

	public GameObject storyprocess;

	public GameObject hintdialog;

	private TUI m_tui;

	public void Start()
	{
		Crazy_GlobalData.OpenClikTimes++;
		OpenClickNew.Show(false);
		Crazy_GlobalData.cur_UI5Times++;
		Crazy_GlobalData.cur_ui = 5;
		m_tui = TUI.Instance("TUI");
		m_tui.SetHandler(this);
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		if (Crazy_Statistics.isfinish)
		{
			Crazy_Data.CurData().SetMoney(Crazy_Data.CurData().GetMoney() + Crazy_Statistics.mgold + Crazy_Statistics.cgold);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task23, 0, Crazy_Statistics.mgold + Crazy_Statistics.cgold);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task24, 0, Crazy_Statistics.mgold + Crazy_Statistics.cgold);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task25, 0, Crazy_Statistics.mgold + Crazy_Statistics.cgold);
			Crazy_Data.CurData().AddKillNumber(Crazy_Statistics.mkillnumber);
			string text = null;
			if (Crazy_GlobalData.cur_leveltype == Crazy_LevelType.Boss && Crazy_Data.CurData().GetBossLevel() <= 30)
			{
				Crazy_Boss_Level bossLevel = Crazy_Boss_Level.GetBossLevel(Crazy_GlobalData.cur_level);
				if (bossLevel.player != Crazy_PlayerClass.None)
				{
					text = "NewChar" + ((int)(bossLevel.player + 1)).ToString("D02");
					Crazy_Data.CurData().SetUnlock(bossLevel.player, true);
					Crazy_GlobalData.maphinticon = text;
					Crazy_GlobalData.mapiconpos = 0;
					Crazy_GlobalData.endpicture = "NewHero" + ((int)(bossLevel.player + 1)).ToString("D02");
				}
				else if (bossLevel.weaponid != -1)
				{
					Crazy_Weapon crazy_Weapon = Crazy_Weapon.FindWeaponByID(Crazy_Weapon.ReadWeaponInfo(), bossLevel.weaponid);
					text = crazy_Weapon.iconname;
					Crazy_Data.CurData().SetWeapon(bossLevel.weaponid, true);
					if (Crazy_Data.CurData().GetPlayerClass() != Crazy_PlayerClass.Mage)
					{
						Crazy_Data.CurData().SetWeaponId(bossLevel.weaponid);
					}
					Crazy_GlobalData.unlock_weaponid.Add(bossLevel.weaponid);
					Crazy_GlobalData.maphinticon = text;
					Crazy_GlobalData.mapiconpos = 1;
				}
				Crazy_Data.CurData().SetBossLevel(Crazy_Data.CurData().GetBossLevel() + 1);
				if (Crazy_Data.CurData().GetBossLevel() > 30)
				{
					Crazy_GlobalData.endpicture = "StoryEnd";
				}
			}
			if (Crazy_GlobalData.cur_process != null && Crazy_Data.CurData().GetBossLevel() > Crazy_GlobalData.cur_process.condition)
			{
				if (Crazy_GlobalData.cur_process.completestory != -1)
				{
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("{gold1}", Crazy_Statistics.mgold.ToString());
					dictionary.Add("{gold2}", Crazy_Statistics.cgold.ToString());
					dictionary.Add("{hits}", Crazy_Statistics.ccombo.ToString());
					UtilUIStoryProcessControl component = storyprocess.GetComponent<UtilUIStoryProcessControl>();
					component.ShowStory(Crazy_GlobalData.cur_process.completestory, dictionary);
				}
				if (Crazy_GlobalData.cur_process.present != -1)
				{
					Crazy_Weapon crazy_Weapon2 = Crazy_Weapon.FindWeaponByID(Crazy_Weapon.ReadWeaponInfo(), Crazy_GlobalData.cur_process.present);
					text = crazy_Weapon2.iconname;
					Crazy_Data.CurData().SetWeapon(Crazy_GlobalData.cur_process.present, true);
					if (Crazy_Data.CurData().GetPlayerClass() != Crazy_PlayerClass.Mage)
					{
						Crazy_Data.CurData().SetWeaponId(Crazy_GlobalData.cur_process.present);
					}
					Crazy_GlobalData.unlock_weaponid.Add(Crazy_GlobalData.cur_process.present);
					Crazy_GlobalData.maphinticon = text;
					Crazy_GlobalData.mapiconpos = 1;
				}
				Crazy_Process crazy_Process = Crazy_Process.FindNextProcessId(Crazy_Data.CurData().GetProcess());
				if (crazy_Process != null)
				{
					Crazy_Data.CurData().SetProcess(crazy_Process.id);
					Hashtable hashtable = new Hashtable();
					hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
					int num = Crazy_Data.CurData().GetAllPlayTime() / 60;
					if (num <= 10)
					{
						hashtable.Add("Playtime", num);
					}
					else
					{
						hashtable.Add("Playtime", num / 10 * 10);
					}
					string eventName = "Process_" + Crazy_Data.CurData().GetProcess();
					FlurryPlugin.logEvent(eventName, hashtable);
				}
				else
				{
					Crazy_Data.CurData().SetProcess(-1);
				}
				Crazy_GlobalData.cur_process = null;
			}
			death01.OnComplete(text, Crazy_Statistics.ccombo, Crazy_Statistics.mgold + Crazy_Statistics.cgold);
			death02.OnComplete(text);
		}
		else
		{
			Crazy_Data.CurData().SetMoney(Crazy_Data.CurData().GetMoney() + Crazy_Statistics.mgold);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task23, 0, Crazy_Statistics.mgold);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task24, 0, Crazy_Statistics.mgold);
			Crazy_TaskManager.GetInstance().updateTask(Crazy_TaskId.Task25, 0, Crazy_Statistics.mgold);
			Crazy_Data.CurData().AddKillNumber(Crazy_Statistics.mkillnumber);
			death01.OnDeath(Crazy_Statistics.mgold);
			if (Crazy_Data.CurData().GetWeaponIcon())
			{
				ShowHint();
			}
		}
		Crazy_GameCenter.instance.UpdateGameCenterData();
		Crazy_Data.SaveData();
	}

	protected void ShowHint()
	{
		hintdialog.transform.localPosition = new Vector3(0f, 0f, hintdialog.transform.localPosition.z);
	}

	protected void HideHint()
	{
		hintdialog.transform.localPosition = new Vector3(1000f, 1000f, hintdialog.transform.localPosition.z);
	}

	public void Update()
	{
		TUIInput[] input = TUIInputManager.GetInput();
		for (int i = 0; i < input.Length; i++)
		{
			m_tui.HandleInput(input[i]);
		}
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "ExitButton" && eventType == 3)
		{
			OnNextScene();
		}
		else if (control.name == "RetryButton" && eventType == 3)
		{
			OnBackScene();
		}
		else if (control.name == "NextButton" && eventType == 3)
		{
			OnNext();
		}
		else if (control.name == "HintNoButton" && eventType == 3)
		{
			HideHint();
		}
		else if (control.name == "HintYesButton" && eventType == 3)
		{
			OnWeaponScene();
		}
	}

	public void OnNext()
	{
		death02.transform.localPosition = new Vector3(0f, 0f, death02.transform.localPosition.z);
		death01.transform.localPosition = new Vector3(1000f, 1000f, death01.transform.localPosition.z);
	}

	public void OnWeaponScene()
	{
		OpenClickNew.Hide();
		Crazy_GameCenter.instance.UpdateGameCenterAchievement();
		Crazy_GameCenter.instance.UpdateGameCenterLeaderBoard(Crazy_Data.CurData().GetKillNumber());
		Resources.UnloadUnusedAssets();
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut("CrazyUI");
		Crazy_GlobalData.iapweapon = 34;
	}

	public void OnNextScene()
	{
		OpenClickNew.Hide();
		Crazy_GameCenter.instance.UpdateGameCenterAchievement();
		Crazy_GameCenter.instance.UpdateGameCenterLeaderBoard(Crazy_Data.CurData().GetKillNumber());
		if (Crazy_GlobalData.endpicture == null)
		{
			Resources.UnloadUnusedAssets();
			m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
				.FadeOut("CrazyMap");
		}
		else
		{
			Resources.UnloadUnusedAssets();
			m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
				.FadeOut("CrazyEnd");
		}
	}

	public void OnBackScene()
	{
		OpenClickNew.Hide();
		Crazy_GlobalData.cur_kill_number = 0;
		Crazy_GlobalData.cur_player_time = 0f;
		Crazy_GlobalData.cur_game_state = Crazy_GameState.PreGame;
		Resources.UnloadUnusedAssets();
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut("CrazyLoading");
	}
}
