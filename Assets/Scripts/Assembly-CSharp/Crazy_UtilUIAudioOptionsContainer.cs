public class Crazy_UtilUIAudioOptionsContainer : TUIContainer
{
	public TUIButtonSelectGroup music;

	public TUIButtonSelectGroup sound;

	private new void Start()
	{
		if (TAudioManager.instance.isMusicOn)
		{
			music.transform.Find("OnButton").GetComponent<TUIButtonSelect>().SetSelected(true);
		}
		else
		{
			music.transform.Find("OffButton").GetComponent<TUIButtonSelect>().SetSelected(true);
		}
		if (TAudioManager.instance.isSoundOn)
		{
			sound.transform.Find("OnButton").GetComponent<TUIButtonSelect>().SetSelected(true);
		}
		else
		{
			sound.transform.Find("OffButton").GetComponent<TUIButtonSelect>().SetSelected(true);
		}
	}

	private void Update()
	{
	}

	public override void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "OffButton" && eventType == 1)
		{
			if (control.transform.parent.parent.name == "Music")
			{
				TAudioManager.instance.isMusicOn = false;
			}
			else if (control.transform.parent.parent.name == "Sound")
			{
				TAudioManager.instance.isSoundOn = false;
			}
		}
		else if (control.name == "OnButton" && eventType == 1)
		{
			if (control.transform.parent.parent.name == "Music")
			{
				TAudioManager.instance.isMusicOn = true;
			}
			else if (control.transform.parent.parent.name == "Sound")
			{
				TAudioManager.instance.isSoundOn = true;
			}
		}
		base.HandleEvent(control, eventType, wparam, lparam, data);
	}
}
