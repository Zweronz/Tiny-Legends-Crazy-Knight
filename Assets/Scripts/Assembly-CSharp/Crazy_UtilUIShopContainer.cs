using UnityEngine;

public class Crazy_UtilUIShopContainer : TUIContainer
{
	public TUIScroll scroll;

	private new void Start()
	{
	}

	private void Update()
	{
	}

	public override void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		for (int i = 1; i <= 5; i++)
		{
			if (control.name == "PageDot" + i.ToString("D02") && eventType == 1)
			{
				int num = 240;
				scroll.position.y = (i - 1) * num;
				scroll.SendMessage("ScrollObjectMove", SendMessageOptions.DontRequireReceiver);
			}
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
	}
}
