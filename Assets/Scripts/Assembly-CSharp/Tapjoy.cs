using UnityEngine;

public class Tapjoy : MonoBehaviour
{
	public GameObject go;

	private void Start()
	{
		go.SetActiveRecursively(false);
	}

	private void Update()
	{
	}
}
