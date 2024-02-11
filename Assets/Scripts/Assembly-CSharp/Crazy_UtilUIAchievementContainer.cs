using UnityEngine;

public class Crazy_UtilUIAchievementContainer : TUIContainer
{
	public GameObject moveObject;

	private new void Start()
	{
	}

	private void Update()
	{
	}

	public override void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "Move" && eventType == 2)
		{
			moveObject.SendMessage("Move", lparam, SendMessageOptions.DontRequireReceiver);
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
	}
}
