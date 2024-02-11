using UnityEngine;

public class AndriodHide : MonoBehaviour
{
	private void Start()
	{
		base.gameObject.SetActiveRecursively(false);
	}
}
