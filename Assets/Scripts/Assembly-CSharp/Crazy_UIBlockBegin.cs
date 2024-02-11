public class Crazy_UIBlockBegin : TUIControlImpl
{
	public const int CommandBegin = 1;

	protected int fingerId = -1;

	public override bool HandleInput(TUIInput input)
	{
		if (input.inputType == TUIInputType.Began)
		{
			if (PtInControl(input.position))
			{
				return true;
			}
			return false;
		}
		return false;
	}
}
