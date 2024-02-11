using System.Collections.Generic;
using UnityEngine;

public class UtilUIHintControl : MonoBehaviour
{
	protected Crazy_PlayAnimation effectanimation;

	protected TUIMeshText text;

	public float NextHintMessageTime;

	private float nextHintMessageTime;

	private float nexthintmessagetime;

	protected Queue<Crazy_HintMessage> hintqueue = new Queue<Crazy_HintMessage>();

	private void Start()
	{
		effectanimation = base.transform.Find("Hint").gameObject.GetComponent("Crazy_PlayAnimation") as Crazy_PlayAnimation;
		effectanimation.Hide();
		text = base.transform.Find("Hint/Plane01").gameObject.GetComponent("TUIMeshText") as TUIMeshText;
	}

	private void UpdateHintMessage()
	{
		Crazy_HintMessage hintMessage = Crazy_SceneManager.GetInstance().GetScene().GetHintMessage();
		if (hintMessage != null)
		{
			hintqueue.Enqueue(hintMessage);
		}
	}

	private void UpdateHint(Crazy_HintMessage mess)
	{
		text.text = mess.text;
		text.UpdateMesh();
		effectanimation.Play();
	}

	private void Update()
	{
	}

	private void UpdateHandleHint()
	{
		nexthintmessagetime += Time.deltaTime;
		if (nexthintmessagetime >= nextHintMessageTime)
		{
			MessageHint();
			nexthintmessagetime = 0f;
		}
	}

	private void MessageHint()
	{
		if (hintqueue.ToArray().GetLength(0) != 0)
		{
			Crazy_HintMessage mess = hintqueue.Dequeue();
			UpdateHint(mess);
			nextHintMessageTime = NextHintMessageTime;
		}
		else
		{
			nextHintMessageTime = 0f;
		}
	}
}
