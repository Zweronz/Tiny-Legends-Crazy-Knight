using UnityEngine;

public class UICrazyUILoading : MonoBehaviour
{
	private void Start()
	{
		Application.LoadLevel(Crazy_GlobalData.next_scene);
	}
}
