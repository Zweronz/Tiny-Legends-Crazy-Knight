public class Crazy_UIButtonPress : TUIButtonBase
{
	public const int CommandDown = 1;

	public const int CommandUp = 2;

	public void Reset()
	{
		fingerId = -1;
	}

	public override bool HandleInput(TUIInput input)
	{
		if (input.inputType == TUIInputType.Began)
		{
			if (PtInControl(input.position))
			{
				fingerId = input.fingerId;
				PostEvent(this, 1, 0f, 0f, null);
				return true;
			}
			return false;
		}
		if (input.fingerId == fingerId)
		{
			if (input.inputType == TUIInputType.Ended)
			{
				fingerId = -1;
				if (PtInControl(input.position))
				{
					PostEvent(this, 2, 0f, 0f, null);
				}
			}
			return true;
		}
		return false;
	}
}
