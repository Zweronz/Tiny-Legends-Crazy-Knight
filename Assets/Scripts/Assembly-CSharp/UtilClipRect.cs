using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UtilClipRect : MonoBehaviour
{
	public Camera ClipCamera;

	public int ClipLayer;

	public int ClipDepth;

	public Rect ClipRect;

	protected float ClipCenterX;

	protected float ClipCenterY;

	protected float ClipRate;

	protected Camera newCamera;

	public bool Static;

	[CompilerGenerated]
	private static Converter<Component, Transform> _003C_003Ef__am_0024cache9;

	private void Start()
	{
		ClipCenterX = (ClipRect.x + ClipRect.xMax) / 2f;
		ClipCenterY = (ClipRect.y + ClipRect.yMax) / 2f;
		newCamera = UnityEngine.Object.Instantiate(ClipCamera) as Camera;
		newCamera.name = "ClipCamera";
		newCamera.transform.parent = ClipCamera.transform.parent;
		newCamera.cullingMask = 1 << ClipLayer;
		newCamera.depth = ClipDepth;
		newCamera.transform.localPosition = ClipCamera.transform.localPosition + new Vector3(ClipCenterX, ClipCenterY, 0f);
		ClipRate = ClipRect.width / 480f;
		newCamera.rect = new Rect((ClipRect.x + 240f) / 480f, (ClipRect.y + 160f) / 320f, ClipRate, ClipRate);
		UpdateClip();
	}

	private void Update()
	{
		if (!Static)
		{
			UpdateClip();
		}
	}

	private void UpdateClip()
	{
		newCamera.orthographicSize = ClipCamera.orthographicSize * ClipRate;
		List<Component> list = new List<Component>(base.gameObject.GetComponentsInChildren(typeof(Transform)));
		if (_003C_003Ef__am_0024cache9 == null)
		{
			_003C_003Ef__am_0024cache9 = _003CUpdateClip_003Em__11;
		}
		List<Transform> list2 = list.ConvertAll(_003C_003Ef__am_0024cache9);
		foreach (Transform item in list2)
		{
			item.gameObject.layer = ClipLayer;
		}
	}

	[CompilerGenerated]
	private static Transform _003CUpdateClip_003Em__11(Component c)
	{
		return (Transform)c;
	}
}
