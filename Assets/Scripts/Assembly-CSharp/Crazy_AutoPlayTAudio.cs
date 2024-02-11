using UnityEngine;

[RequireComponent(typeof(TAudioController))]
public class Crazy_AutoPlayTAudio : MonoBehaviour
{
	public string audioname;

	private void Start()
	{
		Play();
	}

	public void Play()
	{
		TAudioController tAudioController = base.gameObject.GetComponent("TAudioController") as TAudioController;
		tAudioController.PlayAudio(audioname);
	}
}
