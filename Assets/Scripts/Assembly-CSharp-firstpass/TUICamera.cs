using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TUICamera : MonoBehaviour
{
	public void Initialize(bool landscape, int layer, int depth)
	{
		float width;
		float height;
		bool hd;
		GetScreenInfo(out width, out height, out hd);
		if (landscape)
		{
			float num = width;
			width = height;
			height = num;
		}
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.identity;
		base.transform.localScale = Vector3.one;
		base.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
		base.GetComponent<Camera>().backgroundColor = Color.white;
		base.GetComponent<Camera>().nearClipPlane = -128f;
		base.GetComponent<Camera>().farClipPlane = 128f;
		base.GetComponent<Camera>().orthographic = true;
		base.GetComponent<Camera>().depth = depth;
		base.GetComponent<Camera>().cullingMask = 1 << layer;
		base.GetComponent<Camera>().aspect = width / height;
		base.GetComponent<Camera>().orthographicSize = height / ((!hd) ? 2f : 4f);
	}

	private void GetScreenInfo(out float width, out float height, out bool hd)
	{
		width = 0f;
		height = 0f;
		hd = false;
		float num = Mathf.Max(Screen.width, Screen.height);
		float num2 = Mathf.Min(Screen.width, Screen.height);
		if (num <= 1136f && num >= 960f && num <= 768f && num >= 640f)
		{
			width = num2;
			height = num;
			hd = true;
		}
		else
		{
			float num3 = Mathf.Min(768f / num2, 1136f / num);
			width = num2 * num3;
			height = num * num3;
			hd = true;
		}
	}
}
