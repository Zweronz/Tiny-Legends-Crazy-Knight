using UnityEngine;

public class Crazy_ColliderMessage : MonoBehaviour
{
	public ColliderMessage cm;

	public float distance;

	public float dtime;

	private IColliderMessage icm;

	private GameObject target;

	private float cur_time;

	public float distime;

	private void Start()
	{
		target = GameObject.Find("Player");
		icm = target.GetComponent("Crazy_PlayerControl") as IColliderMessage;
		cur_time = 0f;
	}

	private void Update()
	{
		cur_time += Time.deltaTime;
		if (cur_time >= dtime)
		{
			if (distance * distance >= Vector3.SqrMagnitude(base.transform.position - target.transform.position))
			{
				icm.IColliderSendMessage(base.gameObject, cm);
			}
			else if (cur_time >= distime + dtime)
			{
				Crazy_ItemManager.DeleteItem(base.gameObject, cm);
			}
		}
	}
}
