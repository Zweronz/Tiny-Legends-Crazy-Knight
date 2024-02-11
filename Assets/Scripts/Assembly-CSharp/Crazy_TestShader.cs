using UnityEngine;

public class Crazy_TestShader : MonoBehaviour
{
	private void Start()
	{
		ShaderStatus();
	}

	private void ShaderStatus()
	{
		Invoke("ShaderStatus", 2f);
	}

	private void Update()
	{
	}
}
