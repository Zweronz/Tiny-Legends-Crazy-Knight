using System.Collections.Generic;
using UnityEngine;

public class UtilUIMonsterSlot : MonoBehaviour
{
	protected GameObject item;

	protected TUIMeshSpriteClip monster;

	protected TUIRect monstercliprect;

	protected TUIMeshSprite monstericon;

	protected Crazy_ParticleSequenceScript bosseffect;

	protected Queue<float> bossqueue = new Queue<float>();

	protected float cur_percent;

	private void Start()
	{
		item = base.transform.Find("Boss").gameObject;
		InitBoss();
		monster = base.transform.Find("Monster").gameObject.GetComponent("TUIMeshSpriteClip") as TUIMeshSpriteClip;
		monstercliprect = base.transform.Find("MonsterClipRect").gameObject.GetComponent("TUIRect") as TUIRect;
		monstericon = base.transform.Find("MonsterIcon").gameObject.GetComponent("TUIMeshSprite") as TUIMeshSprite;
		bosseffect = base.transform.Find("MonsterIcon/IconEffect").GetComponent("Crazy_ParticleSequenceScript") as Crazy_ParticleSequenceScript;
		cur_percent = 0f;
	}

	private void InitBoss()
	{
		List<float> bossPercent = Crazy_UpdateMonster.GetBossPercent();
		for (int i = 0; i < bossPercent.Count; i++)
		{
			GameObject gameObject = Object.Instantiate(item) as GameObject;
			gameObject.name = string.Format("Boss{0:D02}", i);
			gameObject.transform.parent = item.transform.parent;
			gameObject.transform.localPosition = new Vector3(-75.75f + 151.5f * bossPercent[i], 0f, item.transform.localPosition.z);
			bossqueue.Enqueue(bossPercent[i]);
		}
	}

	private void UpdateMonsterSlot()
	{
		UpdateMonsterSlot(Crazy_UpdateMonster.GetCurPercent());
	}

	private void UpdateMonsterSlot(float percent)
	{
		monstericon.transform.localPosition = new Vector3(-75.5f + 151.5f * percent, 0f, monstericon.transform.localPosition.z);
		monstercliprect.rect = new Rect(monstercliprect.rect.x, monstercliprect.rect.y, 151.5f * percent, monstercliprect.rect.height);
		monstercliprect.UpdateRect();
		monster.UpdateMesh();
		cur_percent = percent;
		UpdateBossEffect();
	}

	private void UpdateBossEffect()
	{
		if (bossqueue.Count != 0 && cur_percent >= bossqueue.Peek())
		{
			bosseffect.EmitParticle();
			bossqueue.Dequeue();
		}
	}

	private void Update()
	{
		UpdateMonsterSlot();
	}
}
