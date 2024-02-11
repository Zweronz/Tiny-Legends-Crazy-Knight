using UnityEngine;

public class Test_Animation : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<Animation>().wrapMode = WrapMode.Once;
		base.GetComponent<Animation>()["Idle01"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>().Play("Idle01");
	}

	private void Update()
	{
		if (Input.GetKeyDown("a"))
		{
			base.GetComponent<Animation>().CrossFade("Damage01", 0.08f);
			base.GetComponent<Animation>()["Damage01"].speed = 0f;
		}
	}
}
