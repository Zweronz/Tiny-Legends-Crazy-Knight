using System.Collections.Generic;
using UnityEngine;

public class UtilUIFinishInfo : MonoBehaviour
{
	protected GameObject sample;

	protected List<GameObject> samplelist = new List<GameObject>();

	private void Start()
	{
		sample = base.transform.Find("Sample").gameObject;
		int num = 3;
		foreach (Crazy_MonsterType key in Crazy_Statistics.monster_killStatistics.Keys)
		{
			Dictionary<int, int> dictionary = Crazy_Statistics.monster_killStatistics[key];
			switch (key)
			{
			case Crazy_MonsterType.Normal:
			{
				int num8 = 1;
				foreach (int key2 in dictionary.Keys)
				{
					int num9 = dictionary[key2];
					GameObject gameObject4 = Object.Instantiate(sample) as GameObject;
					gameObject4.name = string.Format("SampleNormal{0:D02}", num8);
					gameObject4.transform.parent = sample.transform.parent;
					gameObject4.transform.localPosition = new Vector3(0f, (num8 - 1) % num * -40, sample.transform.localPosition.z);
					TUIMeshSprite component7 = gameObject4.transform.Find("Icon").GetComponent<TUIMeshSprite>();
					component7.frameName = Crazy_Monster_Template_Manager.GetMonsterTemplate(key2).iconname;
					component7.UpdateMesh();
					Vector2 size4 = TUITextureManager.Instance().GetFrame(component7.frameName).size;
					gameObject4.transform.Find("TextAll").gameObject.transform.localPosition = new Vector3(size4.x / 2f - 30f, 0f, 0f);
					TUIMeshText component8 = gameObject4.transform.Find("TextAll/TextNumber").GetComponent<TUIMeshText>();
					component8.text = num9.ToString();
					component8.UpdateMesh();
					samplelist.Add(gameObject4);
					num8++;
				}
				break;
			}
			case Crazy_MonsterType.MiddleBoss:
			{
				int num6 = 1;
				foreach (int key3 in dictionary.Keys)
				{
					int num7 = dictionary[key3];
					GameObject gameObject3 = Object.Instantiate(sample) as GameObject;
					gameObject3.name = string.Format("SampleMiddleBoss{0:D02}", num6);
					gameObject3.transform.parent = sample.transform.parent;
					gameObject3.transform.localPosition = new Vector3(142f, (num6 - 1) % num * -40, sample.transform.localPosition.z);
					TUIMeshSprite component5 = gameObject3.transform.Find("Icon").GetComponent<TUIMeshSprite>();
					component5.frameName = Crazy_Monster_Template_Manager.GetMonsterTemplate(key3).iconname;
					component5.UpdateMesh();
					Vector2 size3 = TUITextureManager.Instance().GetFrame(component5.frameName).size;
					gameObject3.transform.Find("TextAll").gameObject.transform.localPosition = new Vector3(size3.x / 2f - 30f, 0f, 0f);
					TUIMeshText component6 = gameObject3.transform.Find("TextAll/TextNumber").GetComponent<TUIMeshText>();
					component6.text = num7.ToString();
					component6.UpdateMesh();
					samplelist.Add(gameObject3);
					num6++;
				}
				break;
			}
			case Crazy_MonsterType.Boss:
			{
				int num4 = 1;
				foreach (int key4 in dictionary.Keys)
				{
					int num5 = dictionary[key4];
					GameObject gameObject2 = Object.Instantiate(sample) as GameObject;
					gameObject2.name = string.Format("SampleBoss{0:D02}", num4);
					gameObject2.transform.parent = sample.transform.parent;
					gameObject2.transform.localPosition = new Vector3(284f, (num4 - 1) % num * -40, sample.transform.localPosition.z);
					TUIMeshSprite component3 = gameObject2.transform.Find("Icon").GetComponent<TUIMeshSprite>();
					component3.frameName = Crazy_Monster_Template_Manager.GetMonsterTemplate(key4).iconname;
					component3.UpdateMesh();
					Vector2 size2 = TUITextureManager.Instance().GetFrame(component3.frameName).size;
					gameObject2.transform.Find("TextAll").gameObject.transform.localPosition = new Vector3(size2.x / 2f - 30f, 0f, 0f);
					TUIMeshText component4 = gameObject2.transform.Find("TextAll/TextNumber").GetComponent<TUIMeshText>();
					component4.text = num5.ToString();
					component4.UpdateMesh();
					samplelist.Add(gameObject2);
					num4++;
				}
				break;
			}
			case Crazy_MonsterType.Ranged:
			{
				int num2 = 1;
				foreach (int key5 in dictionary.Keys)
				{
					int num3 = dictionary[key5];
					GameObject gameObject = Object.Instantiate(sample) as GameObject;
					gameObject.name = string.Format("SampleRanged{0:D02}", num2);
					gameObject.transform.parent = sample.transform.parent;
					gameObject.transform.localPosition = new Vector3(284f, (num2 - 1) % num * -40 - 80, sample.transform.localPosition.z);
					TUIMeshSprite component = gameObject.transform.Find("Icon").GetComponent<TUIMeshSprite>();
					component.frameName = Crazy_Monster_Template_Manager.GetMonsterTemplate(key5).iconname;
					component.UpdateMesh();
					Vector2 size = TUITextureManager.Instance().GetFrame(component.frameName).size;
					gameObject.transform.Find("TextAll").gameObject.transform.localPosition = new Vector3(size.x / 2f - 30f, 0f, 0f);
					TUIMeshText component2 = gameObject.transform.Find("TextAll/TextNumber").GetComponent<TUIMeshText>();
					component2.text = num3.ToString();
					component2.UpdateMesh();
					samplelist.Add(gameObject);
					num2++;
				}
				break;
			}
			}
		}
		Crazy_Statistics.ResetMonsterKillNumber();
	}

	private void Update()
	{
	}
}
