using UnityEngine;

public class Devil_S_CameraAdjustForiPad : MonoBehaviour
{
	public bool bFullScreen;

	private void Awake()
	{
		if (!bFullScreen)
		{
			Camera camera = base.GetComponent<Camera>();
			float num = Mathf.Max(Screen.width, Screen.height);
			float num2 = Mathf.Min(Screen.width, Screen.height);
			float num3 = 960f;
			float num4 = 640f;
			float num5 = 1f;
			num5 = ((!(num <= 1136f) || !(num >= 960f) || !(num <= 768f) || !(num >= 640f)) ? Mathf.Min(768f / num2, 1136f / num) : 1f);
			num3 /= num5;
			num4 /= num5;
			camera.pixelRect = new Rect(((float)Screen.width - num3) * 0.5f, ((float)Screen.height - num4) * 0.5f, num3, num4);
			Debug.LogWarning("Android : " + camera.pixelRect);
		}
	}
}
