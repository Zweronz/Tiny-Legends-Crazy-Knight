public class Crazy_UIButtonSelectGroupEx : TUIButtonSelectGroupEx
{
	public new void Start()
	{
		base.Start();
		if (tabInfo == null)
		{
			return;
		}
		for (int i = 0; i < tabInfo.Length; i++)
		{
			if (i == current)
			{
				tabInfo[i].control.SetSelected(true);
				HandleEvent(tabInfo[i].control, 1, 0f, 0f, null);
			}
		}
	}
}
