using System.Collections.Generic;

public class Crazy_UIContainerWapper
{
	protected List<UIControl> m_control = new List<UIControl>();

	public virtual void Add(UIControl control)
	{
		m_control.Add(control);
	}

	public virtual void Remove(UIControl control)
	{
		m_control.Remove(control);
	}

	public virtual void SetVisible(bool visible)
	{
		foreach (UIControl item in m_control)
		{
			item.Visible = visible;
		}
	}

	public virtual void SetEnable(bool enable)
	{
		foreach (UIControl item in m_control)
		{
			item.Enable = enable;
		}
	}
}
