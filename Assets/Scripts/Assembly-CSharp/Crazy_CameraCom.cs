using UnityEngine;

public class Crazy_CameraCom : MonoBehaviour
{
	public Vector3 localPosition;

	public Vector3 localRotation;

	private void Start()
	{
		base.transform.localPosition = localPosition;
		base.transform.localEulerAngles = localRotation;
		Transform transform = base.gameObject.transform.Find("Blood_pfb");
		if (transform != null)
		{
			float num = (float)Screen.width / (float)Screen.height;
			transform.localScale = new Vector3(1f * num, 2f / 3f * num, 1f);
		}
	}

	private void Update()
	{
	}
}
