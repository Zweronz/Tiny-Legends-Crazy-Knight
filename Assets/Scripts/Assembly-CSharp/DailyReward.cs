using System;
using System.Collections.Generic;
using UnityEngine;

public class DailyReward : MonoBehaviour
{
	public List<DailyRewardItem> rewardItem;

	public MyGUIImageButton rewardBtn;

	public MyGUIImageButton continueBtn;

	private int nCurrentID;

	private void OnReward(GameObject go)
	{
		if (nCurrentID < 0 || nCurrentID > 4)
		{
			Debug.LogError("out of range-------------------------------");
			return;
		}
		int num = int.Parse(rewardItem[nCurrentID].rewardText);
		if (nCurrentID == 4)
		{
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() + num);
		}
		else
		{
			Crazy_Data.CurData().SetMoney(Crazy_Data.CurData().GetMoney() + num);
		}
		Crazy_Data.CurData().SetLastDailyAwardID(nCurrentID);
		Crazy_Data.CurData().SetLastDailyAwardTime(Crazy_Global.ServerTime);
		Crazy_Data.SaveData();
		base.gameObject.SetActiveRecursively(false);
	}

	private void OnContinue(GameObject go)
	{
		base.gameObject.SetActiveRecursively(false);
	}

	private void OnHide(GameObject go)
	{
		base.gameObject.SetActiveRecursively(false);
	}

	public void ShowDailyAward()
	{
		if (Crazy_Data.CurData().GetLastDailyAwardTime() == string.Empty)
		{
			base.gameObject.SetActiveRecursively(true);
			rewardItem[0].OnSelected();
			rewardItem[1].OnDisabled();
			rewardItem[2].OnDisabled();
			rewardItem[3].OnDisabled();
			rewardItem[4].OnDisabled();
			Crazy_Data.CurData().SetLastDailyAwardID(0);
			Crazy_Data.SaveData();
		}
		else
		{
			OnNextDailyAward();
		}
		Crazy_GlobalData.m_bShowDailyReward = false;
	}

	private void Start()
	{
		MyGUIEventListener.Get(base.gameObject).EventHandleOnClicked += OnHide;
		MyGUIEventListener.Get(rewardBtn.gameObject).EventHandleOnClicked += OnReward;
		MyGUIEventListener.Get(continueBtn.gameObject).EventHandleOnClicked += OnContinue;
	}

	private void OnNextDailyAward()
	{
		if ((DateTime.Parse(Crazy_Global.ServerTime) - DateTime.Parse(Crazy_Data.CurData().GetLastDailyAwardTime())).TotalHours < 48.0)
		{
			base.gameObject.SetActiveRecursively(true);
			nCurrentID = Crazy_Data.CurData().GetLastDailyAwardID();
			nCurrentID++;
			if (nCurrentID > 4)
			{
				nCurrentID = 0;
				rewardItem[0].OnSelected();
				rewardItem[1].OnDisabled();
				rewardItem[2].OnDisabled();
				rewardItem[3].OnDisabled();
				rewardItem[4].OnDisabled();
				return;
			}
			for (int i = 0; i < 5; i++)
			{
				if (i == nCurrentID)
				{
					rewardItem[nCurrentID].OnSelected();
				}
				else
				{
					rewardItem[i].OnDisabled();
				}
			}
		}
		else
		{
			base.gameObject.SetActiveRecursively(true);
			nCurrentID = 0;
			rewardItem[0].OnSelected();
			rewardItem[1].OnDisabled();
			rewardItem[2].OnDisabled();
			rewardItem[3].OnDisabled();
			rewardItem[4].OnDisabled();
			Crazy_Data.CurData().SetLastDailyAwardTime(string.Empty);
			Crazy_Data.CurData().SetLastDailyAwardID(0);
			Crazy_Data.SaveData();
		}
	}
}
