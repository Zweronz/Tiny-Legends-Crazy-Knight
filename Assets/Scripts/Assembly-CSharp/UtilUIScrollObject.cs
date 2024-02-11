using UnityEngine;

public class UtilUIScrollObject : MonoBehaviour
{
	private TUIButtonClick[] m_buttons;

	public void OnScrollBegin()
	{
		ResetButtonClick();
	}

	public void OnScrollMove()
	{
	}

	public void OnScrollEnd()
	{
		ResetButtonClick();
	}

	protected void ResetButtonClick()
	{
		if (m_buttons == null)
		{
			m_buttons = base.gameObject.GetComponentsInChildren<TUIButtonClick>(true);
		}
		for (int i = 0; i < m_buttons.Length; i++)
		{
			m_buttons[i].Reset();
		}
	}
}
