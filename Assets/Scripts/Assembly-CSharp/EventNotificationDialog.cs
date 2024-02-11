using UnityEngine;

public class EventNotificationDialog : MonoBehaviour
{
	public Transform m_closeBtn;

	public Transform m_openURLBtn;

	public UtilUIStoryProcessControl uspc;

	public TUIBlock block;

	private void Awake()
	{
		if (m_closeBtn == null)
		{
			m_closeBtn = base.transform.Find("CloseBtn");
		}
		if (m_openURLBtn == null)
		{
			m_openURLBtn = base.transform.Find("GoBtn");
		}
		MyGUIEventListener.Get(m_closeBtn.gameObject).EventHandleOnClicked += CloseDialog;
		MyGUIEventListener.Get(m_openURLBtn.gameObject).EventHandleOnClicked += OpenURL;
	}

	private void Start()
	{
	}

	private void OpenURL(GameObject go)
	{
		Crazy_Global._OpenURL("https://itunes.apple.com/us/app/tiny-legends-heroes/id569360073?ls=1&mt=8");
		FlurryPlugin.logEvent("TLHE_BIG_Ad_Click");
	}

	private void CloseDialog(GameObject go)
	{
		if ((bool)block)
		{
			block.enabled = false;
		}
		base.gameObject.SetActiveRecursively(false);
		if ((bool)uspc)
		{
			uspc.HideStory();
		}
	}
}
