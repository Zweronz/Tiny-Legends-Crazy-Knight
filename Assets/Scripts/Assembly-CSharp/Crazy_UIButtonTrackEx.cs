using UnityEngine;

public class Crazy_UIButtonTrackEx : Crazy_UIButtonTrack
{
	protected bool move;

	public GameObject moveObject;

	public override bool HandleInput(TUIInput input)
	{
		if (input.inputType == TUIInputType.Began)
		{
			if (PtInControl(input.position))
			{
				fingerId = input.fingerId;
				fingerPosition = input.position;
				touch = true;
				move = false;
				PostEvent(this, 1, 0f, 0f, null);
				MoveObjectBegin();
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
			float num = input.position.x - fingerPosition.x;
			float num2 = input.position.y - fingerPosition.y;
			fingerPosition = input.position;
			if (Mathf.Abs(num) >= 1f || Mathf.Abs(num2) >= 1f)
			{
				move = true;
			}
			PostEvent(this, 2, num, num2, null);
			MoveObjectMove();
		}
		else if (input.inputType == TUIInputType.Ended)
		{
			fingerId = -1;
			fingerPosition = Vector2.zero;
			touch = false;
			PostEvent(this, 3, 0f, 0f, null);
			if (move)
			{
				move = false;
				return true;
			}
			return false;
		}
		return false;
	}

	protected void MoveObjectBegin()
	{
		if ((bool)moveObject)
		{
			moveObject.SendMessage("OnMoveBegin", SendMessageOptions.DontRequireReceiver);
		}
	}

	protected void MoveObjectMove()
	{
		if ((bool)moveObject)
		{
			moveObject.SendMessage("OnMoving", SendMessageOptions.DontRequireReceiver);
		}
	}

	protected void MoveObjectEnd()
	{
		if ((bool)moveObject)
		{
			moveObject.SendMessage("OnMoveEnd", SendMessageOptions.DontRequireReceiver);
		}
	}
}
