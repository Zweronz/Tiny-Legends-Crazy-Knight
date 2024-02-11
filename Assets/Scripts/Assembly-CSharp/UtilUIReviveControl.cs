using UnityEngine;

public class UtilUIReviveControl : MonoBehaviour
{
	public Vector3 showvec;

	public void Show()
	{
		base.gameObject.transform.localPosition = showvec;
	}

	public void Hide()
	{
		base.gameObject.transform.localPosition = new Vector3(1000f, 1000f, 0f);
	}

	private void Start()
	{
		Hide();
	}
}
