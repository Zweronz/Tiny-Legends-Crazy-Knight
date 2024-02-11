using UnityEngine;

public class UtilUIDeath02 : TUIIgnoreTimeScale
{
	protected TUIMeshSprite item;

	protected GameObject completeEffect;

	private void Awake()
	{
		completeEffect = base.transform.Find("TitleParent/EffectParent/EffectComplete").gameObject;
		item = base.transform.Find("Item/Item").GetComponent<TUIMeshSprite>();
	}

	public void OnComplete(string framename)
	{
		if (framename != null)
		{
			item.frameName = framename;
			item.UpdateMesh();
		}
		completeEffect.transform.localPosition = Vector3.zero;
		completeEffect.GetComponent<Animation>().Play("jiesuan");
	}
}
