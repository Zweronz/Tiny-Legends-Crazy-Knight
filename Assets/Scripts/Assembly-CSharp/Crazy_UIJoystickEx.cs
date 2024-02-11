using UnityEngine;

public class Crazy_UIJoystickEx : Crazy_UIJoystick
{
	public GameObject frame_d;

	public bool pressed;

	private bool reset;

	public new void Start()
	{
		base.Start();
		UpdateFrame();
	}

	public void Update()
	{
		if (!Application.isMobilePlatform && name == "MoveButton")
		{
			DoMovePC();
		}
	}

	protected void UpdateFrame()
	{
		HideFrame();
		ShowFrame();
	}

	private void HideFrame()
	{
		if ((bool)frame)
		{
			frame.active = false;
		}
		if ((bool)frame_d)
		{
			frame_d.active = false;
		}
	}

	private void ShowFrame()
	{
		if (pressed)
		{
			if ((bool)frame_d)
			{
				frame_d.active = true;
			}
		}
		else if ((bool)frame)
		{
			frame.active = true;
		}
	}

	public override bool HandleInput(TUIInput input)
	{
		if (!Application.isMobilePlatform && name == "MoveButton")
		{
			return false;
		}
		if (input.inputType == TUIInputType.Began)
		{
			if (PtInControl(input.position))
			{
				fingerId = input.fingerId;
				DoReset();
				PostEvent(this, 1, 0f, 0f, null);
				Vector2 vector = DoMove(input.position);
				PostEvent(this, 2, vector.x, vector.y, null);
				pressed = true;
				UpdateFrame();
				return true;
			}
			return false;
		}
		if (input.fingerId != fingerId)
		{
			return false;
		}
		if (input.inputType == TUIInputType.Moved)
		{
			Vector2 vector2 = DoMove(input.position);
			PostEvent(this, 2, vector2.x, vector2.y, null);
			return true;
		}
		if (input.inputType == TUIInputType.Ended)
		{
			fingerId = -1;
			DoReset();
			PostEvent(this, 3, 0f, 0f, null);
			pressed = false;
			UpdateFrame();
			return true;
		}
		return false;
	}

	private void DoReset()
	{
		frame.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, frame.transform.position.z);
		frame_d.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, frame_d.transform.position.z);
	}

	private Vector2 DoMove(Vector2 position)
	{
		Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.y);
		Vector2 vector2 = position - vector;
		float magnitude = vector2.magnitude;
		float value = (magnitude - min) / (max - min);
		value = Mathf.Clamp(value, 0f, 1f);
		Vector2 vector3 = value * vector2.normalized;
		Vector2 vector4 = vector + vector3 * max;
		frame.transform.position = new Vector3(vector4.x, vector4.y, frame.transform.position.z);
		frame_d.transform.position = new Vector3(vector4.x, vector4.y, frame_d.transform.position.z);
		return vector3;
	}

	private void DoMovePC()
	{
		Vector2 move = new Vector2(Input.GetKey(KeyCode.A) ? -max : Input.GetKey(KeyCode.D) ? max : 0,Input.GetKey(KeyCode.S) ? -max : Input.GetKey(KeyCode.W) ? max : 0);

		if (move.x == 0 && move.y == 0 && !reset)
		{
			reset = true;
			Reset();
			PostEvent(this, 3, 0f, 0f, null);
			UpdateFrame();
			return;
		}
		else
		{
			if (reset)
			{
				PostEvent(this, 1, 0f, 0f, null);
				PostEvent(this, 2, move.x, move.y, null);
				UpdateFrame();
			}
			reset = false;
			frame.transform.position = new Vector3(move.x, move.y, frame.transform.position.z);
			frame_d.transform.position = new Vector3(move.x, move.y, frame_d.transform.position.z);
			PostEvent(this, 2, move.x, move.y, null);
		}
	}

	public void Reset()
	{
		fingerId = -1;
		DoReset();
		PostEvent(this, 3, 0f, 0f, null);
		pressed = false;
		UpdateFrame();
	}
}
