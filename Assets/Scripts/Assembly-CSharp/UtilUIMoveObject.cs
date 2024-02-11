using UnityEngine;

public class UtilUIMoveObject : MonoBehaviour
{
	private TUIButtonClick[] m_buttons;

	public void OnMoveBegin()
	{
		ResetButtonClick();
	}

	public void OnMoving()
	{
	}

	public void OnMoveEnd()
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
