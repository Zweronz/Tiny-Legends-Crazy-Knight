using UnityEngine;

public class Crazy_Flying : MonoBehaviour
{
	public float gravity = -9.8f;

	public float time = 1f;

	protected Vector3 beginposition;

	protected Vector3 endposition;

	protected bool flying;

	protected float speedxz;

	protected float speedy;

	protected Vector2 vxz;

	private void Start()
	{
	}

	public void OnFly(Vector3 des)
	{
		beginposition = base.gameObject.transform.position;
		endposition = des;
		flying = true;
		Vector2 vector = new Vector2(endposition.x - beginposition.x, endposition.z - beginposition.z);
		speedxz = vector.magnitude / time;
		vxz = vector.normalized;
		speedy = (endposition.y - beginposition.y) / time - 0.5f * gravity * time;
		flying = true;
	}

	private void Update()
	{
		if (flying)
		{
			Vector3 vector = new Vector3(vxz.x * speedxz * Time.deltaTime, speedy * Time.deltaTime + 0.5f * gravity * Time.deltaTime * Time.deltaTime, vxz.y * speedxz * Time.deltaTime);
			speedy += gravity * Time.deltaTime;
			base.gameObject.transform.position += vector;
			if ((endposition - base.gameObject.transform.position).sqrMagnitude < 0.1f || (base.gameObject.transform.position.y < 0.1f && speedy < 0f))
			{
				base.gameObject.transform.position = endposition;
				flying = false;
				base.gameObject.SendMessage("Trigger", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
