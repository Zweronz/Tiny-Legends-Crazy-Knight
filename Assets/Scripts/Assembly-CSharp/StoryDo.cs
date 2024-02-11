using System.Collections.Generic;

public class StoryDo
{
	public int storyid;

	public Dictionary<string, string> replace;

	public UtilUIStoryProcessControl.ShowDelegate sbd;

	public UtilUIStoryProcessControl.HideDelegate sed;

	public StoryDo(int _storyid, Dictionary<string, string> _replace, UtilUIStoryProcessControl.ShowDelegate _sbd, UtilUIStoryProcessControl.HideDelegate _sed)
	{
		storyid = _storyid;
		replace = _replace;
		sbd = _sbd;
		sed = _sed;
	}
}
