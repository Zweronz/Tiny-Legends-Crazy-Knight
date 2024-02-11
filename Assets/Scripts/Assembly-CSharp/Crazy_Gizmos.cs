using UnityEngine;

public class Crazy_Gizmos : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(base.transform.position, 0.3f);
	}
}
