using UnityEngine;

public class Crazy_UIButtonSelectGroupSendMessage : Crazy_UIButtonSelectGroup
{
	public string methodname;

	public override void ResetControl()
	{
		foreach (TUIButtonSelect item in buttonSelectGroup)
		{
			item.SetSelected(false);
			SendMessage(methodname, item, SendMessageOptions.DontRequireReceiver);
			item.Reset();
		}
	}

	public override void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		bool flag = false;
		for (int i = 0; i < buttonSelectGroup.Count; i++)
		{
			if (buttonSelectGroup[i] == control && eventType == 1)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			for (int j = 0; j < buttonSelectGroup.Count; j++)
			{
				if (buttonSelectGroup[j] != control && buttonSelectGroup[j].IsSelected())
				{
					buttonSelectGroup[j].SetSelected(false);
					SendMessage(methodname, buttonSelectGroup[j], SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
	}
}
