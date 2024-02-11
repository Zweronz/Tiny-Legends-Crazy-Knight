using UnityEngine;

public class DailyRewardItem : MonoBehaviour
{
	public string icon_normal;

	public string icon_highlight;

	public string rewardText;

	public string rewardType;

	public TUIMeshText rewardNum;

	public TUIMeshSprite rewardTypeIcon;

	private string bk_highlight = "daily_bigON";

	private string bk_normal = "daily_bignomal";

	private void Start()
	{
	}

	public void OnSelected()
	{
		base.transform.Find("BK").GetComponent<TUIMeshSprite>().frameName = bk_highlight;
		base.transform.Find("BK").GetComponent<TUIMeshSprite>().UpdateMesh();
		base.transform.Find("Icon").GetComponent<TUIMeshSprite>().frameName = icon_highlight;
		base.transform.Find("Icon").GetComponent<TUIMeshSprite>().UpdateMesh();
		rewardNum.text = rewardText;
		rewardNum.UpdateMesh();
		rewardTypeIcon.frameName = rewardType;
		rewardTypeIcon.UpdateMesh();
	}

	public void OnDisabled()
	{
		base.transform.Find("BK").GetComponent<TUIMeshSprite>().frameName = bk_normal;
		base.transform.Find("BK").GetComponent<TUIMeshSprite>().UpdateMesh();
		base.transform.Find("Icon").GetComponent<TUIMeshSprite>().frameName = icon_normal;
		base.transform.Find("Icon").GetComponent<TUIMeshSprite>().UpdateMesh();
	}
}
