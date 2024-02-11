using UnityEngine;

public class Crazy_ParticleEmitter : MonoBehaviour, ICrazyEffectEvent
{
	public GameObject[] children;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void EmitParticle()
	{
		Emit();
	}

	public void Emit()
	{
		GameObject[] array = children;
		foreach (GameObject gameObject in array)
		{
			gameObject.GetComponent<ParticleEmitter>().emit = true;
		}
	}

	public void Stop()
	{
		GameObject[] array = children;
		foreach (GameObject gameObject in array)
		{
			gameObject.GetComponent<ParticleEmitter>().emit = false;
		}
	}

	public void OneShot()
	{
		GameObject[] array = children;
		foreach (GameObject gameObject in array)
		{
			gameObject.GetComponent<ParticleEmitter>().Emit();
		}
	}

	public void Trigger()
	{
		Emit();
	}
}
