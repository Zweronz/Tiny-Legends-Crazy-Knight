using UnityEngine;

public class Crazy_ParticleSystemAutoStop : MonoBehaviour
{
	public float delay;

	private void Start()
	{
	}

	private void AutoStop()
	{
		Invoke("Stop", delay);
	}

	private void Stop()
	{
		ParticleSystem particleSystem = base.gameObject.GetComponent("ParticleSystem") as ParticleSystem;
		particleSystem.Stop(true);
	}

	private void Update()
	{
	}
}
