using System.Collections.Generic;
using UnityEngine;

public class UtilUIFightBoard : MonoBehaviour
{
	protected TUIMeshText missiontitle;

	protected TUIMeshText timetext;

	protected TUIMeshSprite timeicon;

	protected List<TUIMeshSprite> monstersicon = new List<TUIMeshSprite>();

	protected GameObject iconsample;

	private void Awake()
	{
		missiontitle = base.transform.Find("Text/MissionTitle").GetComponent<TUIMeshText>();
		timeicon = base.transform.Find("Text/TimeIcon").GetComponent<TUIMeshSprite>();
		timetext = base.transform.Find("Text/TimeText").GetComponent<TUIMeshText>();
		iconsample = base.transform.Find("IconParent/IconSample").gameObject;
		for (int i = 0; i < 5; i++)
		{
			GameObject gameObject = Object.Instantiate(iconsample) as GameObject;
			gameObject.name = string.Format("Icon{0:D02}", i);
			gameObject.transform.parent = iconsample.transform.parent;
			gameObject.transform.localPosition = iconsample.transform.localPosition;
			monstersicon.Add(gameObject.GetComponent<TUIMeshSprite>());
		}
	}

	public void SeqIcon(List<string> icons)
	{
		float num = 5 * icons.Count;
		float num2 = 0f;
		for (int i = 0; i < icons.Count; i++)
		{
			monstersicon[i].frameName = icons[i];
			float x = TUITextureManager.Instance().GetFrame(icons[i]).size.x;
			num2 += x / 2f;
			monstersicon[i].transform.localPosition = new Vector3(num2, 0f, monstersicon[i].transform.localPosition.z);
			num2 += x / 2f;
			num2 -= num;
			monstersicon[i].UpdateMesh();
		}
		num2 += num;
		monstersicon[0].transform.parent.localPosition = new Vector3((0f - num2) / 2f, monstersicon[0].transform.parent.localPosition.y, monstersicon[0].transform.parent.localPosition.z);
	}

	public void ResIcon()
	{
		for (int i = 0; i < monstersicon.Count; i++)
		{
			monstersicon[i].transform.localPosition = new Vector3(1000f, 0f, monstersicon[i].transform.localPosition.z);
		}
	}

	public void SetMissionTitle(string text)
	{
		missiontitle.text = text;
		missiontitle.UpdateMesh();
	}

	protected void SetTimeText(string text)
	{
		timetext.text = text;
		timetext.UpdateMesh();
	}

	public void SetTimeIcon(string text)
	{
		timeicon.active = true;
		timetext.active = true;
		SetTimeText(text);
	}

	public void RemoveTimeIcon()
	{
		timeicon.active = false;
		timetext.active = false;
	}

	public void Show()
	{
		base.transform.localPosition = new Vector3(0f, 0f, base.transform.localPosition.z);
	}

	public void Hide()
	{
		ResIcon();
		base.transform.localPosition = new Vector3(1000f, 1000f, base.transform.localPosition.z);
	}

	private void Update()
	{
	}
}
