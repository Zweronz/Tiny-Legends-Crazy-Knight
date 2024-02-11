using UnityEngine;

[RequireComponent(typeof(TUIMeshText))]
public class Crazy_UITextNumberRollSendMessage : Crazy_UITextNumberRoll
{
	public string beginmethodname;

	public string endmethodname;

	private bool isfirst;

	public override void SetText(float lasttime, int text, float time)
	{
		base.SetText(lasttime, text, time);
		isfirst = true;
	}

	private void Update()
	{
		float num = UpdateRealTimeDelta();
		if (!onauto)
		{
			return;
		}
		_dtime += num;
		if (!(_dtime - _time >= 0f))
		{
			return;
		}
		if (_dtime - _time >= dtime)
		{
			SendMessage(endmethodname, SendMessageOptions.DontRequireReceiver);
			m_text.text = _text.ToString();
			m_text.UpdateMesh();
			onauto = false;
		}
		else
		{
			if (isfirst)
			{
				SendMessage(beginmethodname, SendMessageOptions.DontRequireReceiver);
			}
			m_text.text = ((int)((float)_text * (_dtime - _time) / dtime)).ToString();
			m_text.UpdateMesh();
		}
		if (isfirst)
		{
			isfirst = false;
		}
	}

	public override void End()
	{
		base.End();
		if (!isfirst)
		{
			SendMessage(endmethodname, SendMessageOptions.DontRequireReceiver);
		}
	}
}
