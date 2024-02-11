using UnityEngine;

public class Crazy_ItemCollider : MonoBehaviour
{
	public GameObject target;

	public float radius;

	private bool iscollider;

	private void Start()
	{
	}

	private void Trigger()
	{
		iscollider = true;
	}

	private void Collider()
	{
		if (iscollider && target != null && (target.transform.position - base.gameObject.transform.position).sqrMagnitude <= radius * radius)
		{
			iscollider = false;
			Crazy_PlayTAudio component = base.gameObject.GetComponent<Crazy_PlayTAudio>();
			if (component != null)
			{
				component.Play();
			}
			NetworkItemManager.Instance.SendColliderItem(base.gameObject);
		}
	}

	private void Update()
	{
		Collider();
	}
}
