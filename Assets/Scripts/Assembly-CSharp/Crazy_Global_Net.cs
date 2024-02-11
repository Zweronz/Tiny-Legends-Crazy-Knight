using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TNetSdk;
using UnityEngine;

public class Crazy_Global_Net
{
	protected static int innermonsterid;

	[CompilerGenerated]
	private static Comparison<TNetUser> _003C_003Ef__am_0024cache1;

	public static bool IsRoomHost(TNetRoom room, int usid)
	{
		Debug.Log(string.Concat("RoomMaster:", room.RoomMaster, "RoomMasterID:", room.RoomMasterID));
		return usid == room.RoomMasterID;
	}

	public static PlayerSettingInfo GetUserVariable(TNetUser user)
	{
		if (user.GetVariable(TNetUserVarType.PlayerSetting) != null)
		{
			return NetworkClassToObject.PlayerSettingFromJsonObject(user.GetVariable(TNetUserVarType.PlayerSetting));
		}
		return null;
	}

	public static List<TNetUser> RoomAliveUserAsc(List<TNetUser> user, TNetRoom room)
	{
		List<TNetUser> list = new List<TNetUser>();
		foreach (TNetUser item in user)
		{
			if (GetUserVariable(item) != null && GetUserVariable(item).inroom && item.IsJoinedInRoom(room))
			{
				list.Add(item);
			}
		}
		if (_003C_003Ef__am_0024cache1 == null)
		{
			_003C_003Ef__am_0024cache1 = _003CRoomAliveUserAsc_003Em__10;
		}
		list.Sort(_003C_003Ef__am_0024cache1);
		return list;
	}

	public static void SynchronizeMonsterId(int id)
	{
		innermonsterid = id;
	}

	public static int GetRandomMonsterId()
	{
		innermonsterid++;
		return innermonsterid;
	}

	[CompilerGenerated]
	private static int _003CRoomAliveUserAsc_003Em__10(TNetUser a, TNetUser b)
	{
		return a.Id.CompareTo(b.Id);
	}
}
