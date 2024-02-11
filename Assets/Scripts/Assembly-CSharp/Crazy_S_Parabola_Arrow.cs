using UnityEngine;

public class Crazy_S_Parabola_Arrow : MonoBehaviour
{
	public bool bFly = true;

	public float vel2D = 6f;

	public Vector2 dir2D = Vector2.up;

	public float velY = 10f;

	public float accY = -60f;

	protected float rotatetime;

	public float maxrotatetime = 0.033f;

	private void Start()
	{
		dir2D = dir2D.normalized;
	}

	private void Update()
	{
		if (bFly)
		{
			velY += accY * Time.deltaTime;
			Vector3 vector = new Vector3(dir2D.x * vel2D, velY, dir2D.y * vel2D) * Time.deltaTime;
			base.transform.position += vector;
			rotatetime += Time.deltaTime;
			if (rotatetime >= maxrotatetime)
			{
				base.transform.localEulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
				rotatetime = 0f;
			}
			if (base.transform.position.y < 0.5f && velY < 0f)
			{
				base.transform.position = new Vector3(base.transform.position.x, 0.2f, base.transform.position.z);
				Object.Destroy(this);
			}
		}
	}
}
