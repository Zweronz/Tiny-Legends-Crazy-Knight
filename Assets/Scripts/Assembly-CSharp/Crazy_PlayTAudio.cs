using UnityEngine;

[RequireComponent(typeof(TAudioController))]
public class Crazy_PlayTAudio : MonoBehaviour
{
	public string audioname;

	public void Play()
	{
		TAudioController tAudioController = base.gameObject.GetComponent("TAudioController") as TAudioController;
		tAudioController.PlayAudio(audioname);
	}

	public void Stop()
	{
		TAudioController tAudioController = base.gameObject.GetComponent("TAudioController") as TAudioController;
		tAudioController.StopAudio(audioname);
	}
}
