using UnityEngine;

public class Crazy_S_Auto_Rotate : MonoBehaviour
{
	public bool bRotate = true;

	public float rotatespeed = 1f;

	public Vector3 rotatedir = Vector3.zero;

	private void Start()
	{
		rotatedir = rotatedir.normalized;
	}

	private void Update()
	{
		if (bRotate && base.transform.position.y >= 0.5f)
		{
			base.transform.localEulerAngles += rotatedir * rotatespeed * Time.deltaTime;
		}
	}
}
