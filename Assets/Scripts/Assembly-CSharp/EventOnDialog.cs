using UnityEngine;

public class EventOnDialog : MonoBehaviour
{
	public GameObject go_yes;

	private void Start()
	{
		MyGUIEventListener.Get(go_yes).EventHandleOnClicked += OnHideDialog;
		MyGUIEventListener.Get(base.gameObject).EventHandleOnClicked += OnHideDialog;
	}

	private void OnHideDialog(GameObject go)
	{
		base.gameObject.SetActiveRecursively(false);
	}
}
