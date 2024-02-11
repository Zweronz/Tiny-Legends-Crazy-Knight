using UnityEngine;

public class Crazy_Name : MonoBehaviour
{
	public string fontname = "TSIScene";

	public Color color = new Color(0.8784314f, 0.84f, 0.4f);

	protected GameObject nameObject;

	public void InitName(string name)
	{
		nameObject = new GameObject("Name");
		TUIMeshText tUIMeshText = nameObject.AddComponent<TUIMeshText>();
		tUIMeshText.fontName = fontname;
		tUIMeshText.color = color;
		tUIMeshText.text = name;
		tUIMeshText.verticalAlignment = TUIMeshText.VerticalAlignment.Center;
		tUIMeshText.horizontalAlignment = TUIMeshText.HorizontalAlignment.Center;
		tUIMeshText.UpdateMesh();
		nameObject.transform.parent = base.transform;
		nameObject.transform.localPosition = new Vector3(0f, 2.5f, 0f);
		nameObject.transform.localScale = new Vector3(0.05f, 0.05f, 1f);
		nameObject.layer = base.gameObject.layer;
	}

	public void UpdateName()
	{
		if (nameObject != null)
		{
			nameObject.transform.forward = new Vector3(0f, 0f, -1f);
		}
	}

	private void Update()
	{
		UpdateName();
	}
}
