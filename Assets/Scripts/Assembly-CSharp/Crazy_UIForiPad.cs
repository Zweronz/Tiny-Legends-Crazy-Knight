using UnityEngine;

public class Crazy_UIForiPad : MonoBehaviour
{
	private void Start()
	{
		if (Screen.width >= 1024 && Screen.height >= 768)
		{
			ModifyUI();
		}
	}

	private void ModifyUI()
	{
		TUIControl[] componentsInChildren = base.gameObject.GetComponentsInChildren<TUIControl>();
		TUIControl[] array = componentsInChildren;
		foreach (TUIControl tUIControl in array)
		{
			Vector3 localPosition = tUIControl.transform.localPosition;
			tUIControl.transform.localPosition = new Vector3(localPosition.x / 960f * 1024f, localPosition.y / 640f * 768f, localPosition.z);
		}
	}
}
