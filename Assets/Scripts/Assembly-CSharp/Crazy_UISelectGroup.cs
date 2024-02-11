using System.Collections.Generic;

public class Crazy_UISelectGroup : Crazy_UIContainerWapper
{
	protected List<UISelectButton> m_select = new List<UISelectButton>();

	public virtual void Add(UISelectButton select)
	{
		m_select.Add(select);
		base.Add(select);
	}

	public virtual void Remove(UISelectButton select)
	{
		m_select.Remove(select);
		base.Remove(select);
	}

	public virtual void SetSelect(UISelectButton select)
	{
		foreach (UISelectButton item in m_select)
		{
			if (select == item)
			{
				item.Set(true);
			}
			else
			{
				item.Set(false);
			}
		}
	}
}
