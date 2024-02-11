using UnityEngine;

public class UtilUIClock : MonoBehaviour
{
	protected TUIMeshText text;

	protected int totaltime;

	protected int currenttime;

	private void Start()
	{
		text = base.transform.Find("ClockText").gameObject.GetComponent("TUIMeshText") as TUIMeshText;
		if (Crazy_GlobalData.cur_leveltype != Crazy_LevelType.NetBoss)
		{
			totaltime = Crazy_LevelModify.GetModify(Crazy_GlobalData.cur_leveltype, Crazy_GlobalData.cur_level).time;
			currenttime = totaltime;
		}
		AudioSource[] componentsInChildren = base.gameObject.GetComponentsInChildren<AudioSource>();
		AudioSource[] array = componentsInChildren;
		foreach (AudioSource audioSource in array)
		{
			audioSource.ignoreListenerVolume = true;
		}
		UpdateTime();
	}

	private void UpdateTime()
	{
		int num = currenttime;
		currenttime = totaltime - (int)Crazy_GlobalData.cur_player_time;
		if (currenttime < num && currenttime <= 10)
		{
			AudioListener.volume = 0.5f;
			base.gameObject.GetComponent<Crazy_PlayTAudio>().Play();
		}
		if (currenttime <= 0)
		{
			currenttime = 0;
			AudioListener.volume = 1f;
		}
		text.text = Crazy_Global.SecondToMinuteSecond(currenttime);
		text.UpdateMesh();
	}

	private void Update()
	{
		UpdateTime();
	}
}
