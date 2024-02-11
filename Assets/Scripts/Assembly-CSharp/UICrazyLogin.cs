using UnityEngine;

public class UICrazyLogin : MonoBehaviour, TUIHandler
{
	private TUI m_tui;

	private GameObject dialog;

	public void Start()
	{
		m_tui = TUI.Instance("TUI");
		m_tui.SetHandler(this);
		dialog = m_tui.transform.Find("TUIControl/ReviewDialog").gameObject;
	}

	public void Update()
	{
		TUIInput[] input = TUIInputManager.GetInput();
		for (int i = 0; i < input.Length; i++)
		{
			m_tui.HandleInput(input[i]);
		}
	}

	public void ShowDialog()
	{
		dialog.transform.localPosition = new Vector3(0f, 0f, dialog.transform.localPosition.z);
	}

	public void HideDialog()
	{
		dialog.transform.localPosition = new Vector3(4000f, 6000f, dialog.transform.localPosition.z);
	}

	public void OnVersionError()
	{
		ShowDialog();
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "YesButton" && eventType == 3)
		{
			Crazy_Global.OpenReviewURL();
			HideDialog();
			OnPreScene();
		}
		else if (control.name == "NoButton" && eventType == 3)
		{
			HideDialog();
			OnPreScene();
		}
	}

	public void OnPreScene()
	{
		Application.LoadLevel("CrazyNet");
	}
}
