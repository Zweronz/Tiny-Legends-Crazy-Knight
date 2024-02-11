using UnityEngine;

public class Crazy_UtilUIStoryProcessContainer : TUIContainer
{
	private new void Start()
	{
	}

	private void Update()
	{
	}

	public override void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "StoryButton" && eventType == 3)
		{
			base.gameObject.SendMessage("HideStory", SendMessageOptions.DontRequireReceiver);
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
	}
}
