using UnityEngine;

public class Crazy_AutoDestroy : MonoBehaviour
{
	public float timeToDestroy = 1f;

	private void Update()
	{
		timeToDestroy -= Time.deltaTime;
		if (timeToDestroy < 0f)
		{
			Object.DestroyObject(base.gameObject);
		}
	}
}
