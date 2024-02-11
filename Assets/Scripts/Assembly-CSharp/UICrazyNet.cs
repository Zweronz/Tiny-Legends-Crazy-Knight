using UnityEngine;

public class UICrazyNet : MonoBehaviour, TUIHandler
{
	private TUI m_tui_down;

	private TUI m_tui_up;

	public void Start()
	{
		m_tui_down = TUI.Instance("TUI/Down/TUI");
		m_tui_down.SetHandler(this);
		m_tui_up = TUI.Instance("TUI/Up/TUI");
		m_tui_up.SetHandler(this);
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
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
			OnMap();
		}
		else if (control.name == "ChatButton" && eventType == 3)
		{
			OnChat();
		}
		else if (control.name == "Button" && eventType == 3)
		{
			Crazy_GlobalData_Net.Instance.netmissionID = int.Parse(control.transform.parent.name.Replace("NetMission", string.Empty));
			Crazy_GlobalData.cur_leveltype = Crazy_LevelType.NetBoss;
			OnLogin();
		}
	}

	private void OnMap()
	{
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut("CrazyMap");
	}

	public void OnLogin()
	{
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut("CrazyLogin");
	}

	public void OnChat()
	{
		Crazy_Global.ShowChatRoom(Crazy_Data.CurData().GetNetName());
	}
}
