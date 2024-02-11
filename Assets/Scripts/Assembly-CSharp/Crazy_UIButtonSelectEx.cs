using UnityEngine;

public class Crazy_UIButtonSelectEx : TUIButtonSelect
{
	public override bool HandleInput(TUIInput input)
	{
		if (input.inputType == TUIInputType.Began)
		{
			if (PtInControl(input.position))
			{
				fingerId = input.fingerId;
				return true;
			}
			return false;
		}
		if (input.fingerId == fingerId)
		{
			if (input.inputType == TUIInputType.Ended)
			{
				fingerId = -1;
				if (PtInControl(input.position) && !pressed)
				{
					pressed = true;
					UpdateFrame();
					PostEvent(this, 1, 0f, 0f, null);
					SendMessage("Play", SendMessageOptions.DontRequireReceiver);
				}
			}
			return true;
		}
		return false;
	}
}
