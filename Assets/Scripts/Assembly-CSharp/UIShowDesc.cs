using UnityEngine;

public class UIShowDesc : MonoBehaviour
{
	public TUIMeshText text;

	private void Awake()
	{
		if (text == null)
		{
			Debug.Log("not set text");
		}
	}

	private void Start()
	{
		MyGUIEventListener myGUIEventListener = MyGUIEventListener.Get(base.gameObject);
		myGUIEventListener.EventHandleOnPressed += ShowDesc;
	}

	private void ShowDesc(GameObject sender, Vector2 vec2)
	{
		if ((bool)text)
		{
			text.text = "desc:...";
			text.UpdateMesh();
		}
	}
}
