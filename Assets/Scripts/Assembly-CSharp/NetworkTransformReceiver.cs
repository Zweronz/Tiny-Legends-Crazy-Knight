using UnityEngine;

public class NetworkTransformReceiver : MonoBehaviour
{
	private Transform thisTransform;

	private NetworkTransformInterpolation interpolator;

	private AnimationSynchronizer animator;

	private NetworkTransform preTransform;

	private void Awake()
	{
		thisTransform = base.transform;
		preTransform = NetworkTransform.FromTransform(thisTransform);
		animator = GetComponent<AnimationSynchronizer>();
		interpolator = GetComponent<NetworkTransformInterpolation>();
		if (interpolator != null)
		{
			interpolator.StartReceiving();
		}
	}

	public void ReceiveTransform(NetworkTransform ntransform)
	{
		if (animator != null)
		{
			animator.CalculateAnimation((preTransform.Position - ntransform.Position).sqrMagnitude);
		}
		preTransform = ntransform;
		if (interpolator != null)
		{
			interpolator.ReceivedTransform(ntransform);
			return;
		}
		thisTransform.position = ntransform.Position;
		thisTransform.localEulerAngles = ntransform.AngleRotationFPS;
	}
}
