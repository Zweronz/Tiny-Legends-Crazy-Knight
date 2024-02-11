using UnityEngine;

[RequireComponent(typeof(TUIMeshText))]
public class Crazy_UITextNumberRoll : TUIIgnoreTimeScale
{
	protected TUIMeshText m_text;

	protected int _text = -1;

	protected float _time;

	protected float dtime;

	protected bool onauto;

	protected float _dtime;

	private void Start()
	{
		m_text = base.gameObject.GetComponent<TUIMeshText>();
		m_text.text = string.Empty;
		m_text.UpdateMesh();
	}

	public virtual void SetText(float lasttime, int text, float time)
	{
		_text = text;
		_time = lasttime;
		onauto = true;
		dtime = time;
		_dtime = 0f;
		m_text.text = string.Empty;
		m_text.UpdateMesh();
	}

	private void Update()
	{
		float num = UpdateRealTimeDelta();
		if (!onauto)
		{
			return;
		}
		_dtime += num;
		if (_dtime - _time >= 0f)
		{
			if (_dtime - _time >= dtime)
			{
				m_text.text = _text.ToString();
				m_text.UpdateMesh();
				onauto = false;
			}
			else
			{
				m_text.text = ((int)((float)_text * (_dtime - _time) / dtime)).ToString();
				m_text.UpdateMesh();
			}
		}
	}

	public virtual void End()
	{
		if (_text != -1)
		{
			m_text.text = _text.ToString();
			m_text.UpdateMesh();
			onauto = false;
		}
	}
}
