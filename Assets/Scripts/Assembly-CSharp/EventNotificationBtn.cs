using UnityEngine;

public class EventNotificationBtn : MonoBehaviour
{
	public GameObject m_notificationDialog;

	public UtilUIStoryProcessControl uspc;

	public TUIBlock block;

	private void Awake()
	{
		base.gameObject.SetActiveRecursively(false);
		MyGUIEventListener.Get(base.gameObject).EventHandleOnClicked += OpenDialog;
		if (m_notificationDialog != null)
		{
			m_notificationDialog.SetActiveRecursively(false);
		}
		if ((bool)block)
		{
			block.enabled = false;
		}
	}

	private void OpenDialog(GameObject go)
	{
		m_notificationDialog.SetActiveRecursively(true);
		if ((bool)block)
		{
			block.enabled = true;
		}
		if ((bool)uspc)
		{
			uspc.ShowStory(102);
		}
	}
}
