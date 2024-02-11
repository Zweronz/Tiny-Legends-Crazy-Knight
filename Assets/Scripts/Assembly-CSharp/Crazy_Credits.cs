using UnityEngine;

public class Crazy_Credits : MonoBehaviour
{
	public GameObject container;

	private void BoardEnd()
	{
		container.SendMessage("CreditsEnd", SendMessageOptions.DontRequireReceiver);
	}
}
