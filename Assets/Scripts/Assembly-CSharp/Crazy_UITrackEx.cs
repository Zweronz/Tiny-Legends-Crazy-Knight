public class Crazy_UITrackEx : Crazy_UITrack
{
	protected bool m_active = true;

	public void SetActive(bool active)
	{
		m_active = active;
		if (active)
		{
			ShowFrame();
		}
		else
		{
			HideFrame();
		}
	}

	private new void Update()
	{
		if (m_active)
		{
			base.Update();
		}
	}
}
