public class TransformInfo
{
	public NetworkTransform trans;

	public string objectId;

	public TransformInfo(NetworkTransform _trans, double time, double ping, string _objectId)
	{
		trans = _trans;
		trans.TimeStamp = time;
		trans.AveragePing = ping;
		objectId = _objectId;
	}

	public TransformInfo(NetworkTransform _trans, string _objectId)
	{
		trans = _trans;
		objectId = _objectId;
	}

	public TransformInfo()
	{
	}

	public void SetTimeStamp(double time)
	{
		if (trans != null)
		{
			trans.TimeStamp = time;
		}
	}
}
