using UnityEngine;

public class Crazy_LocalNotification : MonoBehaviour
{
	public TUIFade fade;

	public static bool isSpecialLocalNotification;

	private void Start()
	{
		Application.LoadLevel("CrazyStart");
	}
}
