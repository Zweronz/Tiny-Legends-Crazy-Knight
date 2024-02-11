using System;
using System.Collections.Generic;
using System.Reflection;

public class EventProcessor
{
	private static EventProcessor m_instance;

	private Dictionary<Type, List<object>> m_handlers = new Dictionary<Type, List<object>>();

	public static EventProcessor Instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = new EventProcessor();
			}
			return m_instance;
		}
	}

	public void AddEventDelegate<T>(GameEventHandler<T> handler)
	{
		Type typeFromHandle = typeof(T);
		if (!m_handlers.ContainsKey(typeFromHandle))
		{
			m_handlers.Add(typeFromHandle, new List<object>());
		}
		if (!m_handlers[typeFromHandle].Contains(handler))
		{
			m_handlers[typeFromHandle].Add(handler);
		}
	}

	public void RemoveEventDelegate<T>(GameEventHandler<T> handler)
	{
		Type typeFromHandle = typeof(T);
		if (m_handlers.ContainsKey(typeFromHandle))
		{
			m_handlers[typeFromHandle].Remove(handler);
		}
	}

	public void Process<T>(object sender, T evt)
	{
		Type typeFromHandle = typeof(T);
		if (!m_handlers.ContainsKey(typeFromHandle))
		{
			return;
		}
		foreach (object item in m_handlers[typeFromHandle])
		{
			MethodInfo method = item.GetType().GetMethod("Invoke");
			method.Invoke(item, new object[2] { sender, evt });
		}
	}
}
