using System.Collections;
using UnityEngine;

public class UICrazyRoom : MonoBehaviour, TUIHandler
{
	private TUI m_tui_down;

	private TUI m_tui_up;

	public GameObject dialog;

	protected TUIMeshText dialogtext;

	private bool gamebegin;

	public void Start()
	{
		m_tui_down = TUI.Instance("TUI/Down/TUI");
		m_tui_down.SetHandler(this);
		m_tui_up = TUI.Instance("TUI/Up/TUI");
		m_tui_up.SetHandler(this);
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		dialogtext = dialog.transform.Find("Text").GetComponent<TUIMeshText>();
	}

	public void Update()
	{
		TUIInput[] input = TUIInputManager.GetInput();
		for (int i = 0; i < input.Length; i++)
		{
			if (!m_tui_up.HandleInput(input[i]))
			{
				m_tui_down.HandleInput(input[i]);
			}
		}
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "BackButton" && eventType == 3)
		{
			OnNet();
		}
		else if (control.name == "ChatButton" && eventType == 3)
		{
			OnChat();
		}
		else if (control.name == "FightButton" && eventType == 3)
		{
			FlurryPlugin.logEvent("Find_Game", true);
			ShowDialog();
			base.gameObject.SendMessage("OnFindGameRoom", Crazy_NetMission.GetNetMissionInfo(Crazy_GlobalData_Net.Instance.netmissionID), SendMessageOptions.DontRequireReceiver);
		}
		else if (control.name == "NoButton" && eventType == 3 && !gamebegin)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("IsBeginGame", "NO");
			FlurryPlugin.endTimedEvent("Find_Game", hashtable);
			HideDialog();
			base.gameObject.SendMessage("OnLeaveGameRoom", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnChat()
	{
		Crazy_Global.ShowChatRoom(Crazy_Data.CurData().GetNetName());
	}

	private void OnNet()
	{
		base.gameObject.SendMessage("OnLeaveScene");
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut("CrazyNet");
	}

	public void OnGame()
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("IsBeginGame", "YES");
		FlurryPlugin.endTimedEvent("Find_Game", hashtable);
		gamebegin = true;
		Crazy_GlobalData.next_scene = "CrazyScene" + Crazy_GlobalData_Net.Instance.sceneID.ToString("D03");
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut("CrazyUILoading");
	}

	public void UserCountChange(string text)
	{
		dialogtext.text = "Finding players...(" + text + ")\nWhen your party is full,\nthe battle will start automatically.";
		dialogtext.UpdateMesh();
	}

	private void ShowDialog()
	{
		dialogtext.text = "Finding players.\nWhen your party is full,\nthe battle will start automatically.";
		dialogtext.UpdateMesh();
		dialog.transform.localPosition = new Vector3(0f, 0f, dialog.transform.localPosition.z);
	}

	private void HideDialog()
	{
		dialog.transform.localPosition = new Vector3(5000f, 5000f, dialog.transform.localPosition.z);
	}
}
