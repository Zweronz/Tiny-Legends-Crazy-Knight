using System;
using UnityEngine;

public class Crazy_ParticleEmitterLast : MonoBehaviour, ICrazyEffectEvent
{
	[Serializable]
	public class Propterty
	{
		public GameObject m_Obj;

		public float m_fLastTime;
	}

	public Propterty[] m_Particles;

	private float m_time;

	private bool fplay;

	private void Awake()
	{
	}

	private void Update()
	{
		if (!fplay)
		{
			return;
		}
		m_time += Time.deltaTime;
		bool flag = false;
		Propterty[] particles = m_Particles;
		foreach (Propterty propterty in particles)
		{
			if (m_time >= propterty.m_fLastTime)
			{
				propterty.m_Obj.GetComponent<ParticleEmitter>().emit = false;
			}
			if (propterty.m_Obj.GetComponent<ParticleEmitter>().emit)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			fplay = false;
		}
	}

	public void Emit()
	{
		m_time = 0f;
		fplay = true;
		Propterty[] particles = m_Particles;
		foreach (Propterty propterty in particles)
		{
			propterty.m_Obj.GetComponent<ParticleEmitter>().emit = true;
		}
	}

	public void Stop()
	{
		fplay = false;
		Propterty[] particles = m_Particles;
		foreach (Propterty propterty in particles)
		{
			propterty.m_Obj.GetComponent<ParticleEmitter>().emit = false;
		}
	}

	public void OneShot()
	{
		m_time = 0f;
		fplay = true;
		Propterty[] particles = m_Particles;
		foreach (Propterty propterty in particles)
		{
			propterty.m_Obj.GetComponent<ParticleEmitter>().Emit();
		}
	}

	public void Trigger()
	{
		Emit();
	}
}
