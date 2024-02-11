using UnityEngine;

public class Crazy_UIButtonTrack : TUIControlImpl
{
	public const int CommandBegin = 1;

	public const int CommandMove = 2;

	public const int CommandEnd = 3;

	protected int fingerId = -1;

	protected Vector2 fingerPosition = Vector2.zero;

	protected bool touch;

	public new void Start()
	{
	}

	public void Update()
	{
	}

	public override bool HandleInput(TUIInput input)
	{
		if (input.inputType == TUIInputType.Began)
		{
			if (PtInControl(input.position))
			{
				fingerId = input.fingerId;
				fingerPosition = input.position;
				touch = true;
				PostEvent(this, 1, 0f, 0f, null);
				return true;
			}
			return false;
		}
		if (input.fingerId != fingerId)
		{
			return false;
		}
		if (!touch)
		{
			return false;
		}
		if (input.inputType == TUIInputType.Moved)
		{
			float wparam = input.position.x - fingerPosition.x;
			float lparam = input.position.y - fingerPosition.y;
			fingerPosition = input.position;
			PostEvent(this, 2, wparam, lparam, null);
		}
		else if (input.inputType == TUIInputType.Ended)
		{
			fingerId = -1;
			fingerPosition = Vector2.zero;
			touch = false;
			PostEvent(this, 3, 0f, 0f, null);
			return true;
		}
		return false;
	}
}
