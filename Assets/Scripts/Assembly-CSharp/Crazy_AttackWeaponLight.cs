using UnityEngine;

public class Crazy_AttackWeaponLight : MonoBehaviour
{
	private bool bshowlight;

	private void Start()
	{
		bshowlight = false;
		UpdateMesh();
	}

	private void Update()
	{
	}

	private void UpdateMesh()
	{
		if (bshowlight)
		{
			base.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			base.GetComponent<Renderer>().enabled = false;
		}
	}

	public void ShowLight(bool bShow)
	{
		bshowlight = bShow;
		UpdateMesh();
	}
}
