using UnityEngine;

public class NetworkStatusReceiver : MonoBehaviour
{
	private PlayerAnimationSynchronizer animator;

	private void Awake()
	{
		animator = GetComponent<PlayerAnimationSynchronizer>();
	}

	public void ReceiveStatus(PlayerStatusInfo info)
	{
		animator.ReceiverStatus(info.status, info.attackname);
	}
}
