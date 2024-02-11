using System;
using UnityEngine;

public class Crazy_UtilUIPanelOptionsContainer : TUIContainer
{
	[Serializable]
	public class DialogInfo
	{
		public GameObject dialog;

		public Vector3 showPosition;

		public Vector3 hidePosition;

		public void Show()
		{
			dialog.transform.localPosition = showPosition;
		}

		public void Hide()
		{
			dialog.transform.localPosition = hidePosition;
		}
	}

	protected GameObject m_char;

	public DialogInfo[] dialog;

	private new void Start()
	{
	}

	private void Update()
	{
	}

	public override void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "CreditsButton" && eventType == 3 && dialog[0] != null)
		{
			dialog[0].dialog.transform.Find("Text").GetComponent<Animation>()["CreditsMove"].time = 0f;
			dialog[0].dialog.transform.Find("Text").GetComponent<Animation>()["CreditsMove"].speed = 1f;
			dialog[0].dialog.transform.Find("Text").GetComponent<Animation>().Play("CreditsMove");
			dialog[0].Show();
		}
		if (control.name == "EndButton" && eventType == 1 && dialog[0] != null)
		{
			dialog[0].dialog.transform.Find("Text").GetComponent<Animation>()["CreditsMove"].speed = 4f;
		}
		if (control.name == "EndButton" && eventType == 2 && dialog[0] != null)
		{
			dialog[0].dialog.transform.Find("Text").GetComponent<Animation>()["CreditsMove"].speed = 1f;
		}
		if (control.name == "ResetButton" && eventType == 3 && dialog[1] != null)
		{
			dialog[1].Show();
		}
		if (control.name == "NoButton" && eventType == 3 && dialog[1] != null)
		{
			dialog[1].Hide();
		}
		if (control.name == "ReviewButton" && eventType == 3)
		{
			Crazy_Global.OpenReviewURL();
		}
		if (control.name == "SupportButton" && eventType == 3)
		{
			Crazy_Global.OpenSupportURL();
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
	}

	private void CreditsEnd()
	{
		if (dialog[0] != null)
		{
			dialog[0].Hide();
			dialog[0].dialog.transform.Find("EndButton").GetComponent<Crazy_UIButtonPress>().Reset();
			dialog[0].dialog.transform.Find("Text").GetComponent<Animation>()["CreditsMove"].speed = 1f;
		}
	}
}
