using UnityEngine;

public class NetworkTransformSender : MonoBehaviour
{
	public delegate void SendTransformDelegate(NetworkTransform trans, string id);

	public static readonly float sendingPeriod = 0.2f;

	private readonly float accuracy = 0.002f;

	private float timeLastSending;

	private bool send;

	private NetworkTransform lastState;

	private Transform thisTransform;

	private SendTransformDelegate sendTransformDelegate;

	private string innerid;

	public void Init(SendTransformDelegate del, string id)
	{
		sendTransformDelegate = del;
		innerid = id;
	}

	private void Start()
	{
		thisTransform = base.transform;
		lastState = NetworkTransform.FromTransform(thisTransform);
	}

	private void StartSendTransform()
	{
		send = true;
	}

	private void FixedUpdate()
	{
		if (send)
		{
			SendTransform();
		}
	}

	private void SendTransform()
	{
		if (timeLastSending >= sendingPeriod)
		{
			lastState = NetworkTransform.FromTransform(thisTransform);
			if (sendTransformDelegate != null)
			{
				sendTransformDelegate(lastState, innerid);
			}
			timeLastSending = 0f;
		}
		else
		{
			timeLastSending += Time.deltaTime;
		}
	}
}
