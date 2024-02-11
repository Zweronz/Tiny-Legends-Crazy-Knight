using System.Collections.Generic;

public class Crazy_UIButtonSelectGroup : TUIContainer
{
	protected List<TUIButtonSelect> buttonSelectGroup;

	public new void Awake()
	{
		buttonSelectGroup = new List<TUIButtonSelect>();
		base.Awake();
	}

	public void AddControl(TUIButtonSelect select)
	{
		buttonSelectGroup.Add(select);
	}

	public void RemoveControl(TUIButtonSelect select)
	{
		buttonSelectGroup.Remove(select);
	}

	public virtual void ResetControl()
	{
		foreach (TUIButtonSelect item in buttonSelectGroup)
		{
			item.SetSelected(false);
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
				}
			}
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
	}
}
