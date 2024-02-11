using UnityEngine;

public class Crazy_TestCamera : MonoBehaviour
{
	private int count;

	private void Start()
	{
		Print();
	}

	private void Update()
	{
		base.GetComponent<Camera>().RenderWithShader(Resources.Load("Shaders/miaoBian") as Shader, string.Empty);
	}

	private void LateUpdate()
	{
	}

	private void FixedUpdate()
	{
	}

	private void OnPreCull()
	{
	}

	private void OnPreRender()
	{
	}

	private void OnPostRender()
	{
	}

	private void Print()
	{
		Invoke("Print", 1f);
	}
}
