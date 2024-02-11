using UnityEngine;

public class Crazy_AudioListener : MonoBehaviour
{
	private void Start()
	{
		AudioListener audioListener = Crazy_Global.GetAudioListener();
		audioListener.transform.parent = base.gameObject.transform;
		audioListener.transform.localPosition = Vector3.zero;
		audioListener.transform.localEulerAngles = Vector3.zero;
	}

	private void OnDestroy()
	{
		AudioListener audioListener = Crazy_Global.GetAudioListener();
		if (!(audioListener == null) && audioListener.transform.parent == base.gameObject.transform)
		{
			audioListener.transform.parent = null;
			audioListener.transform.localPosition = Vector3.zero;
			audioListener.transform.localEulerAngles = Vector3.zero;
		}
	}
}
