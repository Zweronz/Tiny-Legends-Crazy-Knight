using UnityEngine;

public class UICrazyStory : MonoBehaviour, TUIHandler
{
	private TUI m_tui;

	public TUIMeshSprite[] bklist;

	private int bkid;

	public void Start()
	{
		m_tui = TUI.Instance("TUI");
		m_tui.SetHandler(this);
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeIn();
		Invoke("UpdateStory", 0.5f);
		Crazy_Global.PlayBGM("BGM_Intro01");
	}

	private void UpdateStory()
	{
		if (bkid >= bklist.Length)
		{
			OnNextScene();
			return;
		}
		if (bkid == 1)
		{
			Crazy_Global.PlayAddBGM("BGM_Intro02");
		}
		Invoke("PlayAnimation", 6f);
	}

	private void PlayAnimation()
	{
		bklist[bkid].GetComponent<Animation>().Play("StoryBkAnimation");
		bkid++;
		UpdateStory();
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
		if (control.name == "Button" && eventType == 3)
		{
			CancelInvoke("PlayAnimation");
			PlayAnimation();
		}
	}

	public void OnNextScene()
	{
		Crazy_Beginner.instance.isStory = false;
		m_tui.transform.Find("TUIControl").Find("Fade").GetComponent<TUIFade>()
			.FadeOut("CrazyMap");
	}
}
