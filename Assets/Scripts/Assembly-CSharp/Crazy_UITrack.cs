using UnityEngine;

public class Crazy_UITrack : MonoBehaviour
{
	public GameObject frameNormal;

	public GameObject framePressed;

	public GameObject frameDisabled;

	public TUIButtonBase button;

	private void Start()
	{
	}

	protected void Update()
	{
		UpdateFrame();
	}

	protected void UpdateFrame()
	{
		HideFrame();
		ShowFrame();
	}

	protected void HideFrame()
	{
		if ((bool)frameNormal)
		{
			frameNormal.active = false;
		}
		if ((bool)framePressed)
		{
			framePressed.active = false;
		}
		if ((bool)frameDisabled)
		{
			frameDisabled.active = false;
		}
	}

	protected void ShowFrame()
	{
		if (button.disabled)
		{
			if ((bool)frameDisabled)
			{
				frameDisabled.active = true;
			}
		}
		else if (button.pressed)
		{
			if ((bool)framePressed)
			{
				framePressed.active = true;
			}
		}
		else if ((bool)frameNormal)
		{
			frameNormal.active = true;
		}
	}
}
