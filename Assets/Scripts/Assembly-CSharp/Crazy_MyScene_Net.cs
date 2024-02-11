using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crazy_MyScene_Net : Crazy_MyScene
{
	private bool isnetwork;

	private bool networkdirty;

	private bool isnetworkbegin;

	protected UtilUIReviveControl UIRevive;

	private float lastinvoketime;

	private bool invoke;

	private float delta;

	private int countTime;

	private int revivecrystal = 5;

	public override void Awake(UtilUIPause uipause, UtilUIBeginnerHintControl uibeginnerhint)
	{
		switch (NetworkManager.Instance.GetPlayerPosition())
		{
		case 0:
			playerposition = new Vector3(1f, 0.1f, 8f);
			break;
		case 1:
			playerposition = new Vector3(-1f, 0.1f, 8f);
			break;
		case 2:
			playerposition = new Vector3(3f, 0.1f, 8f);
			break;
		case 3:
			playerposition = new Vector3(-3f, 0.1f, 8f);
			break;
		}
		isnetwork = true;
		networkdirty = true;
		isnetworkbegin = false;
		base.Awake(uipause, uibeginnerhint);
	}

	public void Awake(UtilUIReviveControl uirevive)
	{
		UIRevive = uirevive;
	}

	public void InvokeToReward(float time)
	{
		invoke = true;
		lastinvoketime = time;
		countTime = (int)time - 5;
		countZero.SetActiveRecursively(true);
		int num = countTime / 10;
		int num2 = countTime % 10;
		TUIMeshSprite component = countZero.transform.Find("Decade").GetComponent<TUIMeshSprite>();
		component.frameName = num.ToString();
		component.UpdateMesh();
		TUIMeshSprite component2 = countZero.transform.Find("Single").GetComponent<TUIMeshSprite>();
		component2.frameName = num2.ToString();
	}

	public void UpdateInvokeToReward()
	{
		if (invoke)
		{
			delta += Time.deltaTime;
			if (delta >= 1f)
			{
				delta = 0f;
				countTime--;
				int num = countTime / 10;
				int num2 = countTime % 10;
				TUIMeshSprite component = countZero.transform.Find("Decade").GetComponent<TUIMeshSprite>();
				component.frameName = num.ToString();
				component.UpdateMesh();
				TUIMeshSprite component2 = countZero.transform.Find("Single").GetComponent<TUIMeshSprite>();
				component2.frameName = num2.ToString();
				component2.UpdateMesh();
			}
			lastinvoketime -= Time.deltaTime;
			if (lastinvoketime <= 5f)
			{
				playerCom.PlaySpeedDown(0f, 5f);
				playerCom.bForbidRoll = true;
				waitTotal.SetActiveRecursively(true);
				countZero.SetActiveRecursively(false);
			}
			if (lastinvoketime <= 0f)
			{
				invoke = false;
				lastinvoketime = 0f;
				ToReward();
			}
		}
	}

	public override void OnGameEnd()
	{
		InvokeToReward(20f);
	}

	protected override Crazy_PlayerControl AddPlayerControl(GameObject player)
	{
		if (Crazy_Data.CurData().GetPlayerClass() == Crazy_PlayerClass.Mage)
		{
			return player.AddComponent<Crazy_PlayerControlMage_Net>() as Crazy_PlayerControl;
		}
		return player.AddComponent<Crazy_PlayerControl_Net>() as Crazy_PlayerControl;
	}

	private void OnNet()
	{
		if (networkdirty && isnetwork && NetworkManager.Instance != null)
		{
			NetworkManager.Instance.UpdateUserVariables(new PlayerSettingInfo(Crazy_Data.CurData().GetPlayerClass(), Crazy_Data.CurData().GetWeaponId(), true, true, true));
			networkdirty = false;
			NetworkTransformSender networkTransformSender = GetPlayerControl().gameObject.AddComponent<NetworkTransformSender>();
			networkTransformSender.Init(NetworkManager.Instance.SendTransform, "this");
			GetPlayerControl().SendMessage("StartSendTransform");
			GetPlayerControl().gameObject.AddComponent<NetworkStatusSender>();
			GetPlayerControl().SendMessage("StartSendStatus");
		}
	}

	public override void OnPauseDown()
	{
		OnPause();
	}

	public override bool IsGameBegin()
	{
		return isnetworkbegin;
	}

	public bool IsNetWork()
	{
		return isnetwork;
	}

	protected override void OnPause()
	{
		UIPause.transform.localPosition = new Vector3(0f, 0f, UIPause.transform.localPosition.z);
		OpenClickNew.Show(false);
	}

	protected override void OffPause()
	{
		UIPause.transform.localPosition = new Vector3(1000f, 1000f, UIPause.transform.localPosition.z);
		OpenClickNew.Hide();
	}

	protected override void ToReward()
	{
		Time.timeScale = 1f;
		TUIFade component = GameObject.Find("TUI/TUIControl/Fade").GetComponent<TUIFade>();
		OpenClickNew.Hide();
		Crazy_GlobalData.next_scene = "CrazyNetReward";
		component.FadeOut("CrazyUILoading");
		NetworkManager.Instance.SendLeave();
		Hashtable hashtable = new Hashtable();
		hashtable.Add("BossID", Crazy_GlobalData_Net.Instance.bossID);
		hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
		FlurryPlugin.logEvent("Victory_Online", hashtable);
		FlurryPlugin.endTimedEvent("Fight_Online");
	}

	protected override void BackToMenu()
	{
		Time.timeScale = 1f;
		TUIFade component = GameObject.Find("TUI/TUIControl/Fade").GetComponent<TUIFade>();
		OpenClickNew.Hide();
		Crazy_GlobalData.next_scene = "CrazyMap";
		component.FadeOut("CrazyUILoading");
		NetworkManager.Instance.SendLeave();
		Hashtable hashtable = new Hashtable();
		hashtable.Add("BossID", Crazy_GlobalData_Net.Instance.bossID);
		hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
		FlurryPlugin.logEvent("Defeat_Online", hashtable);
		FlurryPlugin.endTimedEvent("Fight_Online");
	}

	public override void UpdateGameBegin()
	{
		if (!isnetworkbegin)
		{
			isnetworkbegin = NetworkManager.Instance.IsAllRoomUserInGame();
		}
	}

	public void ShowRevive()
	{
		UIRevive.Show();
		Hashtable hashtable = new Hashtable();
		hashtable.Add("BossID", Crazy_GlobalData_Net.Instance.bossID);
		hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
		FlurryPlugin.logEvent("User_Died_Online", hashtable);
	}

	public override void OnReviveDown()
	{
		if (Crazy_Data.CurData().GetCrystal() >= revivecrystal)
		{
			Crazy_Data.CurData().SetCrystal(Crazy_Data.CurData().GetCrystal() - revivecrystal);
			Crazy_PlayerControl_Net crazy_PlayerControl_Net = playerCom as Crazy_PlayerControl_Net;
			crazy_PlayerControl_Net.Revive();
			UIRevive.Hide();
			Hashtable hashtable = new Hashtable();
			hashtable.Add("BossID", Crazy_GlobalData_Net.Instance.bossID);
			hashtable.Add("Level", Crazy_Data.CurData().GetLevel());
			FlurryPlugin.logEvent("User_Revived_Online", hashtable);
		}
	}

	public override void Update()
	{
		try
		{
			OnNet();
			UpdateInvokeToReward();
			total_fight_time += Time.deltaTime;
			updateCamera();
			timeToUnload += Time.deltaTime;
			if (timeToUnload > 30f)
			{
				timeToUnload = 0f;
				Resources.UnloadUnusedAssets();
			}
			if (!playerCom.IsDie())
			{
				UpdateHandleMessage();
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	protected override void updateCamera()
	{
		if (door != null)
		{
			mainCameraCom.transform.parent.position = Vector3.Lerp(mainCameraCom.transform.parent.position, door.transform.position, 0.5f);
		}
		else if (!playerCom.IsDie())
		{
			mainCameraCom.transform.parent.position = Vector3.Lerp(mainCameraCom.transform.parent.position, GetPlayerPositon(), 0.5f);
		}
		else
		{
			mainCameraCom.transform.parent.position = Vector3.Lerp(mainCameraCom.transform.parent.position, GetEnemyPosition(), 0.5f);
		}
	}

	public Vector3 GetEnemyPosition()
	{
		if (Crazy_GlobalData.enemyList != null && Crazy_GlobalData.enemyList.Count != 0)
		{
			using (Dictionary<int, GameObject>.ValueCollection.Enumerator enumerator = Crazy_GlobalData.enemyList.Values.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					GameObject current = enumerator.Current;
					return current.transform.position;
				}
			}
		}
		return Vector3.zero;
	}
}
