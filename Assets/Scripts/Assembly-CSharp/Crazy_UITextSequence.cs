using UnityEngine;

[RequireComponent(typeof(TUIMeshText))]
public class Crazy_UITextSequence : TUIIgnoreTimeScale
{
	private TUIMeshText m_text;

	private string _text;

	private float _time;

	private int count;

	private int cur_count;

	private float dtime;

	private bool onauto;

	private float _dtime;

	private void Start()
	{
		m_text = base.gameObject.GetComponent<TUIMeshText>();
		m_text.text = string.Empty;
		m_text.UpdateMesh();
	}

	public void SetText(float lasttime, string text, float time)
	{
		_text = text;
		_time = lasttime;
		count = text.Length;
		cur_count = 0;
		onauto = true;
		dtime = time / (float)count;
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
		if (_dtime - _time >= dtime)
		{
			_dtime -= dtime;
			cur_count++;
			if (cur_count < count)
			{
				m_text.text = _text.Substring(0, cur_count);
				m_text.UpdateMesh();
			}
			else if (cur_count == count)
			{
				m_text.text = _text;
				m_text.UpdateMesh();
				onauto = false;
			}
		}
	}

	public void End()
	{
		if (_text != null)
		{
			m_text.text = _text;
			m_text.UpdateMesh();
			onauto = false;
		}
	}
}
