using System;
using UnityEngine;

public class Crazy_ModelAnimation : TrinitiModelAnimation
{
	[Serializable]
	public class ModelAnimation_InvokeData
	{
		public string ScriptToInvoke;

		public string MethodInvokeName;

		public float MethodInvokeTime;
	}

	[Serializable]
	public class ModelAnimation_AnimationInvokeData
	{
		public string animationname;

		public ModelAnimation_InvokeData[] invokedata;
	}

	public ModelAnimation_AnimationInvokeData[] AnimationInvokeData;

	protected void OnInvoke(string name)
	{
		for (int i = 0; i < AnimationInvokeData.Length; i++)
		{
			if (name == AnimationInvokeData[i].animationname)
			{
				for (int j = 0; j < AnimationInvokeData[i].invokedata.Length; j++)
				{
					ModelAnimation_InvokeData modelAnimation_InvokeData = AnimationInvokeData[i].invokedata[j];
					MonoBehaviour monoBehaviour = base.gameObject.GetComponent(modelAnimation_InvokeData.ScriptToInvoke) as MonoBehaviour;
					monoBehaviour.Invoke(modelAnimation_InvokeData.MethodInvokeName, modelAnimation_InvokeData.MethodInvokeTime);
				}
			}
		}
	}

	public void Play(string name, WrapMode mode, float fSpeed)
	{
		Play(name, mode, fSpeed, false);
	}
}
