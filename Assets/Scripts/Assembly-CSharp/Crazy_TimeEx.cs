using UnityEngine;

public class Crazy_TimeEx : MonoBehaviour
{
	private static float lastframetime;

	private static float deltaframetime;

	private void Start()
	{
		lastframetime = 0f;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		deltaframetime = realtimeSinceStartup - lastframetime;
		lastframetime = realtimeSinceStartup;
	}

	public static float RealDeltaTime()
	{
		return deltaframetime;
	}
}
