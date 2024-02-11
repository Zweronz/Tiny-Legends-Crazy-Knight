using UnityEngine;

public class UICrazyAchievement : MonoBehaviour, TUIHandler
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
	}

	private void OnMap()
	{
		m_tui_up.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut("CrazyMap");
	}

	public void OnDestroy()
	{
	}
}
