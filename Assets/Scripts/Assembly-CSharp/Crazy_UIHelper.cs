using System.Collections.Generic;
using UnityEngine;

public class Crazy_UIHelper : UIHelper
{
	public string RootNode;

	protected List<Crazy_UIHelper> m_children = new List<Crazy_UIHelper>();

	protected Crazy_UIHelper m_parent;

	public new void Start()
	{
		base.Start();
		GameObject gameObject = GameObject.Find(RootNode);
		if (null != gameObject)
		{
			m_UIManagerRef.gameObject.transform.parent = gameObject.transform;
		}
		else
		{
			m_UIManagerRef.gameObject.transform.parent = null;
		}
	}

	public void SetParent(Crazy_UIHelper parent)
	{
		if (m_parent != null)
		{
			m_parent.RemoveChild(this);
		}
		m_parent = parent;
		parent.AddChild(this);
	}

	public void AddChild(Crazy_UIHelper child)
	{
		m_children.Add(child);
	}

	public void RemoveChild(Crazy_UIHelper child)
	{
		m_children.Remove(child);
	}

	protected virtual bool HandleInputEvent(UITouchInner touch)
	{
		return m_UIManagerRef.HandleInput(touch);
	}

	public override void Update()
	{
		base.Update();
		if (!(m_parent == null))
		{
			return;
		}
		UITouchInner[] array = iPhoneInputMgr.MockTouches();
		foreach (UITouchInner touch in array)
		{
			bool flag = false;
			foreach (Crazy_UIHelper child in m_children)
			{
				if (child.HandleInputEvent(touch))
				{
					flag = true;
					break;
				}
			}
			if (flag || HandleInputEvent(touch))
			{
			}
		}
	}

	public virtual void SendUIMessage(Crazy_UIHelper sender, int messageId, float lparam, float wparam)
	{
	}
}
