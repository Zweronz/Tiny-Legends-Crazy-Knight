using UnityEngine;

public class UtilUINameLabel : MonoBehaviour
{
	private TUIMeshText text;

	private void Start()
	{
		text = base.gameObject.GetComponent<TUIMeshText>();
	}

	private void UpdateText()
	{
		string showString = UtilUIEnterName.GetShowString();
		text.text = showString;
		text.UpdateMesh();
	}

	private void Update()
	{
		UpdateText();
	}
}
