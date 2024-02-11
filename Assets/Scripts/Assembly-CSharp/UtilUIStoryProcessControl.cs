using System;
using System.Collections.Generic;
using UnityEngine;

public class UtilUIStoryProcessControl : MonoBehaviour
{
	public delegate void ShowDelegate();

	public delegate void HideDelegate();

	protected TUIMeshSprite texture;

	protected TUIMeshText text;

	protected Queue<string> m_story = new Queue<string>();

	protected Dictionary<string, string> m_replace;

	protected ShowDelegate showdo;

	protected HideDelegate hidedo;

	private void Awake()
	{
		texture = base.transform.Find("Texture").GetComponent<TUIMeshSprite>();
		text = base.transform.Find("Text").GetComponent<TUIMeshText>();
	}

	public void ShowStory(int storyid, Dictionary<string, string> replace = null, ShowDelegate show = null, HideDelegate hide = null)
	{
		showdo = show;
		hidedo = hide;
		m_replace = replace;
		Crazy_StoryProcess storyProcessInfo = Crazy_StoryProcess.GetStoryProcessInfo(storyid);
		texture.frameName = storyProcessInfo.iconname;
		m_story.Clear();
		string language = Crazy_Language.GetLanguage(storyProcessInfo.textid);
		language = language.Replace("\\t", "\t");
		string[] array = language.Split("\t".ToCharArray(), StringSplitOptions.None);
		string[] array2 = array;
		foreach (string item in array2)
		{
			m_story.Enqueue(item);
		}
		if (m_story.Count != 0)
		{
			text.text = CheckReplace(m_story.Dequeue().Replace("\\n", "\n"));
		}
		texture.UpdateMesh();
		text.UpdateMesh();
		base.GetComponent<Animation>().Play("Process");
		if (showdo != null)
		{
			showdo();
			showdo = null;
		}
	}

	protected string CheckReplace(string show)
	{
		string text = show;
		if (m_replace == null)
		{
			return show;
		}
		foreach (string key in m_replace.Keys)
		{
			text = text.Replace(key, m_replace[key]);
		}
		return text;
	}

	public void HideStory()
	{
		if (m_story.Count != 0)
		{
			text.text = CheckReplace(m_story.Dequeue().Replace("\\n", "\n"));
			text.UpdateMesh();
		}
		else
		{
			base.GetComponent<Animation>().Play("ProcessHide");
			m_story.Clear();
		}
		if (hidedo != null)
		{
			hidedo();
			hidedo = null;
		}
	}

	private void Update()
	{
	}
}
