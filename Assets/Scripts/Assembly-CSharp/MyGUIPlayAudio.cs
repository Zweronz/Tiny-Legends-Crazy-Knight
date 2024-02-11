using UnityEngine;

public class MyGUIPlayAudio : MonoBehaviour
{
	public new Crazy_PlayTAudio audio;

	private void Start()
	{
		MyGUIEventListener.Get(base.gameObject).EventHandleOnClicked += OnPlayAudio;
	}

	private void OnPlayAudio(GameObject go)
	{
		audio.Play();
	}

	private void Update()
	{
	}
}
