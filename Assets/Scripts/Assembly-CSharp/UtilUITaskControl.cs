using UnityEngine;

public class UtilUITaskControl : MonoBehaviour
{
	private TUIMeshText title;

	private TUIMeshSprite icon;

	private TUIMeshText text;

	private TUIMeshText count;

	protected bool show;

	public void ShowTask(int id)
	{
		if (show)
		{
			return;
		}
		Crazy_Achievement achievementInfo = Crazy_Achievement.GetAchievementInfo(id);
		Crazy_Award awardInfo = Crazy_Award.GetAwardInfo(achievementInfo.award);
		title.text = achievementInfo.name.ToUpper();
		title.UpdateMesh();
		string frameName = null;
		if (awardInfo != null && awardInfo.item.Count != 0 && awardInfo.item[0].type == Crazy_Award_Item_Type.Currency)
		{
			switch (awardInfo.item[0].id)
			{
			case 0:
				frameName = "GoldBig";
				break;
			case 1:
				frameName = "CrystalBig";
				break;
			}
		}
		icon.frameName = frameName;
		icon.UpdateMesh();
		text.text = achievementInfo.des;
		text.UpdateMesh();
		count.text = awardInfo.item[0].count.ToString();
		count.UpdateMesh();
		base.gameObject.transform.localPosition = new Vector3(0f, 130f, -10f);
		show = true;
		Invoke("HideTask", 3f);
	}

	private void HideTask()
	{
		if (show)
		{
			base.gameObject.transform.localPosition = new Vector3(0f, 3000f, -10f);
			show = false;
		}
	}

	private void Awake()
	{
		title = base.transform.Find("Title").GetComponent<TUIMeshText>();
		icon = base.transform.Find("Icon").GetComponent<TUIMeshSprite>();
		text = base.transform.Find("Text").GetComponent<TUIMeshText>();
		count = base.transform.Find("Count").GetComponent<TUIMeshText>();
		Crazy_TaskManager.GetInstance().Register(base.gameObject);
		base.gameObject.transform.localPosition = new Vector3(0f, 3000f, -10f);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnDestory()
	{
		Crazy_TaskManager.GetInstance().Unregister();
	}
}
