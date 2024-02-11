using UnityEngine;

public class Crazy_ItemTrigger : MonoBehaviour, ICrazyEffectEvent
{
	public void Trigger()
	{
		Animation[] componentsInChildren = base.gameObject.transform.GetComponentsInChildren<Animation>();
		Animation[] array = componentsInChildren;
		foreach (Animation animation in array)
		{
			animation.Play("Float");
		}
		Crazy_ParticleSystemSequenceScript[] componentsInChildren2 = base.gameObject.transform.GetComponentsInChildren<Crazy_ParticleSystemSequenceScript>();
		Crazy_ParticleSystemSequenceScript[] array2 = componentsInChildren2;
		foreach (Crazy_ParticleSystemSequenceScript crazy_ParticleSystemSequenceScript in array2)
		{
			crazy_ParticleSystemSequenceScript.Trigger();
		}
	}

	public void Stop()
	{
	}
}
