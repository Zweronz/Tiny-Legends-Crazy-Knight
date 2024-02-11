using System;
using UnityEngine;

public class Crazy_ParticleSequenceScript : MonoBehaviour, ICrazyEffectEvent
{
	[Serializable]
	public class Propterty
	{
		public GameObject m_Obj;

		public float m_fDelayTime;
	}

	public Propterty[] m_Particles;

	private float m_time;

	private int m_index;

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
		if (m_index >= m_Particles.Length)
		{
			m_time = 0f;
			fplay = false;
			return;
		}
		m_time += Time.deltaTime;
		Propterty propterty = m_Particles[m_index];
		if (m_time > propterty.m_fDelayTime)
		{
			StartParticle();
		}
	}

	private void PlayParticle()
	{
		m_time = 0f;
		m_index = 0;
		StartParticle();
	}

	private void StartParticle()
	{
		while (true)
		{
			Propterty propterty = m_Particles[m_index];
			if (propterty.m_fDelayTime <= m_time)
			{
				propterty.m_Obj.GetComponent<ParticleEmitter>().Emit();
				m_time = 0f;
				m_index++;
				if (m_index >= m_Particles.Length)
				{
					m_time = 0f;
					break;
				}
				continue;
			}
			break;
		}
	}

	public void EmitParticle()
	{
		fplay = true;
		PlayParticle();
	}

	public bool Isplay()
	{
		return fplay;
	}

	public void Trigger()
	{
		EmitParticle();
	}

	public void Stop()
	{
	}
}
