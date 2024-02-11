using System;
using System.Collections;
using System.Collections.Generic;
using TNetSdk;
using UnityEngine;

public class RoomControl : MonoBehaviour
{
	public GameObject gameControl;

	protected string cur_group_id;

	public int MaxUser;

	private bool gameBegin;

	private bool isperleaveroom;

	private void FixedUpdate()
	{
		try
		{
			if (TNetManager.Connection != null && !gameBegin)
			{
				TNetManager.Connection.Update(Time.fixedDeltaTime);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	protected void RegisterTNetCallBacks()
	{
		try
		{
			CGUI.Log(GetType().ToString() + ":RegisterTNetCallBacks");
			TNetManager.Connection.AddEventListener(TNetEventSystem.CONNECTION_KILLED, OnConnectionLost);
			TNetManager.Connection.AddEventListener(TNetEventSystem.CONNECTION_TIMEOUT, OnConnectionLost);
			TNetManager.Connection.AddEventListener(TNetEventSystem.REVERSE_HEART_WAITING, OnHeartWaiting);
			TNetManager.Connection.AddEventListener(TNetEventSystem.REVERSE_HEART_RENEW, OnHeartRenew);
			TNetManager.Connection.AddEventListener(TNetEventSystem.REVERSE_HEART_TIMEOUT, OnHeartTimeout);
			TNetManager.Connection.AddEventListener(TNetEventRoom.GET_ROOM_LIST, OnGetRoomList);
			TNetManager.Connection.AddEventListener(TNetEventRoom.ROOM_CREATION, OnRoomCreation);
			TNetManager.Connection.AddEventListener(TNetEventRoom.ROOM_JOIN, OnJoinRoom);
			TNetManager.Connection.AddEventListener(TNetEventRoom.USER_VARIABLES_UPDATE, OnUserVariablesUpdate);
			TNetManager.Connection.AddEventListener(TNetEventRoom.USER_ENTER_ROOM, OnUserEnterRoom);
			TNetManager.Connection.AddEventListener(TNetEventRoom.USER_EXIT_ROOM, OnUserLeaveRoom);
			TNetManager.Connection.AddEventListener(TNetEventRoom.ROOM_START, OnRoomStart);
			TNetManager.Connection.AddEventListener(TNetEventRoom.ROOM_MASTER_CHANGE, OnHostChange);
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void UnregisterTNetCallBacks()
	{
		try
		{
			if (TNetManager.Connection != null)
			{
				CGUI.Log(GetType().ToString() + ":UnregisterTNetCallBacks");
				TNetManager.Connection.RemoveAllEventListeners();
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	protected void OnLeaveScene()
	{
		try
		{
			TNetManager.ManualDisconnect();
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void Awake()
	{
		try
		{
			Application.runInBackground = true;
			ResetGlobalData();
			if (TNetManager.Connection != null)
			{
				RegisterTNetCallBacks();
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void Start()
	{
		Invoke("CheckGameBegin", 3f);
	}

	private void Update()
	{
	}

	public void CheckGameBegin()
	{
		try
		{
			if (TNetManager.Connection == null)
			{
				return;
			}
			TNetRoom curRoom = TNetManager.Connection.CurRoom;
			if (curRoom == null || !Crazy_Global_Net.IsRoomHost(curRoom, TNetManager.Connection.Myself.Id) || curRoom.UserCount < curRoom.MaxUsers)
			{
				return;
			}
			bool flag = true;
			foreach (TNetUser user in curRoom.UserList)
			{
				PlayerSettingInfo userVariable = Crazy_Global_Net.GetUserVariable(user);
				if (userVariable == null || !userVariable.isprepare)
				{
					flag = false;
				}
			}
			if (flag)
			{
				OnGameBegin();
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
		finally
		{
			Invoke("CheckGameBegin", 3f);
		}
	}

	private void ResetGlobalData()
	{
		try
		{
			Crazy_GlobalData_Net.Instance.Reset();
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void OnApplicationPause(bool pause)
	{
		try
		{
			if (pause)
			{
				TNetManager.ManualDisconnect();
				return;
			}
			TNetManager.ManualDisconnect();
			Hashtable hashtable = new Hashtable();
			hashtable.Add("Reason", "ManualDisconnect");
			FlurryPlugin.logEvent("Online_Disconnect", hashtable);
			Application.LoadLevel("CrazyConnectionLost");
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void OnHeartWaiting(TNetEventData evt)
	{
		CGUI.Log("OnHeartWaiting");
	}

	public void OnHeartRenew(TNetEventData evt)
	{
		CGUI.Log("OnHeartRenew");
	}

	public void OnHeartTimeout(TNetEventData evt)
	{
		try
		{
			CGUI.Log("OnHeartTimeout");
			TNetManager.ConnectionLost();
			Hashtable hashtable = new Hashtable();
			hashtable.Add("Reason", "HeartTimeout");
			FlurryPlugin.logEvent("Online_Disconnect", hashtable);
			Application.LoadLevel("CrazyConnectionLost");
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	protected void SendDragRoomList(int groupid)
	{
		try
		{
			if (TNetManager.Connection != null)
			{
				CGUI.Log(GetType().ToString() + ":DragRoomListGroupId:" + groupid);
				TNetManager.Connection.Send(new GetRoomListRequest(groupid, 0, 10, RoomDragListCmd.ListType.not_full_not_game));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	protected void CreateGameRoom()
	{
		try
		{
			if (TNetManager.Connection != null)
			{
				string text = TNetManager.Connection.Myself.Name + "'s Room";
				CGUI.Log(GetType().ToString() + ":CreateGameRoom:" + text);
				TNetManager.Connection.Send(new CreateRoomRequest(text, string.Empty, int.Parse(cur_group_id), MaxUser, RoomCreateCmd.RoomType.limit, RoomCreateCmd.RoomSwitchMasterType.Auto, string.Empty));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	protected void SendJoinRoom(int roomid, string pwd)
	{
		try
		{
			if (TNetManager.Connection != null)
			{
				Debug.Log(GetType().ToString() + ":JoinRoom:" + roomid);
				CGUI.Log(GetType().ToString() + ":JoinRoom:" + roomid);
				TNetManager.Connection.Send(new JoinRoomRequest(roomid, pwd));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void OnLeaveGameRoom()
	{
		try
		{
			if (TNetManager.Connection != null)
			{
				Debug.Log("Send LeaveRoomRequest");
				CGUI.Log("Send LeaveRoomRequest");
				TNetManager.Connection.Send(new LeaveRoomRequest());
				isperleaveroom = true;
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	protected void JoinRoomSuccess(TNetRoom room)
	{
		Debug.Log("JoinRoomSuccess:" + room.Name);
		try
		{
			CGUI.Log("JoinRoomSuccess:" + room.Name);
			UpdateUserVariables(new PlayerSettingInfo(Crazy_PlayerClass.None, -1, false, true, true));
			if (gameControl != null)
			{
				gameControl.SendMessage("UserCountChange", room.UserCount + "/" + room.MaxUsers);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	protected void JoinRoomFailed()
	{
		try
		{
			CGUI.Log("JoinRoomFailed");
			ReJoinGameRoom();
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	protected void ReJoinGameRoom()
	{
		try
		{
			JoinGameRoom(cur_group_id);
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void OnJoinRoom(TNetEventData evt)
	{
		try
		{
			if ((int)evt.data["result"] == 0)
			{
				JoinRoomSuccess((TNetRoom)evt.data["room"]);
			}
			else
			{
				JoinRoomFailed();
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void OnGetRoomList(TNetEventData evt)
	{
		try
		{
			Debug.Log("OnGetRoomList");
			List<TNetRoom> list = (List<TNetRoom>)evt.data["roomList"];
			if (list.Count == 0)
			{
				CreateGameRoom();
			}
			else
			{
				SendJoinRoom(list[0].Id, string.Empty);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void OnRoomCreation(TNetEventData evt)
	{
		try
		{
			if ((int)evt.data["result"] == 0)
			{
				RoomCreationSuccess((ushort)evt.data["roomId"]);
			}
			else
			{
				RoomCreationFailed();
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	protected void RoomCreationSuccess(int roomid)
	{
		CGUI.Log("RoomCreationSuccess");
	}

	protected void RoomCreationFailed()
	{
		try
		{
			CGUI.Log("RoomCreationFailed");
			ReCreateGameRoom();
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	protected void ReCreateGameRoom()
	{
		try
		{
			CreateGameRoom();
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void JoinGameRoom(string groupID)
	{
		try
		{
			cur_group_id = groupID;
			SendDragRoomList(int.Parse(cur_group_id));
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void OnFindGameRoom(Crazy_NetMission netmission)
	{
		try
		{
			MaxUser = netmission.maxcount;
			Crazy_GlobalData_Net.Instance.bossID = netmission.bossid;
			Crazy_GlobalData_Net.Instance.sceneID = netmission.sceneid;
			JoinGameRoom(netmission.groupid);
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void UpdateUserVariables(PlayerSettingInfo info)
	{
		try
		{
			SFSObject sFSObject = NetworkClassToObject.PlayerSettingToJsonObject(info);
			if (TNetManager.Connection != null)
			{
				CGUI.Log("UpdateUserVariables:PlayerSetting:" + sFSObject);
				TNetManager.Connection.Send(new SetUserVariableRequest(TNetUserVarType.PlayerSetting, sFSObject));
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void OnUserVariablesUpdate(TNetEventData evt)
	{
		try
		{
			TNetUser tNetUser = (TNetUser)evt.data["user"];
			TNetUserVarType tNetUserVarType = (TNetUserVarType)(int)evt.data["key"];
			CGUI.Log("OnUserVariablesUpdate User:" + tNetUser.Name + " | key:" + tNetUserVarType);
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void OnUserEnterRoom(TNetEventData evt)
	{
		try
		{
			TNetUser tNetUser = (TNetUser)evt.data["user"];
			Debug.Log("OnUserEnterRoom:" + tNetUser.Name + ":" + tNetUser.Id);
			CGUI.Log("OnUserEnterRoom:" + tNetUser.Name + ":" + tNetUser.Id);
			TNetRoom curRoom = TNetManager.Connection.CurRoom;
			if (gameControl != null)
			{
				gameControl.SendMessage("UserCountChange", curRoom.UserCount + "/" + curRoom.MaxUsers);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void OnUserLeaveRoom(TNetEventData evt)
	{
		try
		{
			TNetUser tNetUser = (TNetUser)evt.data["user"];
			Debug.Log("OnUserLeaveRoom:" + tNetUser.Name);
			CGUI.Log("OnUserLeaveRoom:" + tNetUser.Name);
			if (tNetUser != TNetManager.Connection.Myself)
			{
				TNetRoom curRoom = TNetManager.Connection.CurRoom;
				if (gameControl != null)
				{
					gameControl.SendMessage("UserCountChange", curRoom.UserCount + "/" + curRoom.MaxUsers);
				}
			}
			else
			{
				ResetGlobalData();
				UpdateUserVariables(new PlayerSettingInfo(Crazy_PlayerClass.None, -1, false, false, false));
				isperleaveroom = false;
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void DoExtraHostAction()
	{
	}

	public void OnHostChange(TNetEventData evt)
	{
		try
		{
			TNetUser tNetUser = (TNetUser)evt.data["user"];
			Debug.Log("OnHostChange:" + tNetUser.Name + ":" + tNetUser.Id);
			CGUI.Log("OnHostChange:" + tNetUser.Name + ":" + tNetUser.Id);
			HandleHostChange(tNetUser);
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void HandleHostChange(TNetUser user)
	{
		try
		{
			if (user == TNetManager.Connection.Myself)
			{
				DoExtraHostAction();
				CGUI.Log("NewHost:" + TNetManager.Connection.Myself.Name);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void OnRoomStart(TNetEventData evt)
	{
		if (isperleaveroom)
		{
			Debug.Log("OnRoomStartCancel");
			return;
		}
		try
		{
			int id = (ushort)evt.data["userId"];
			Debug.Log("OnRoomStart:" + TNetManager.Connection.CurRoom.GetUserById(id).Name);
			CGUI.Log("OnRoomStart:" + TNetManager.Connection.CurRoom.GetUserById(id).Name);
			CancelInvoke("CheckGameBegin");
			DoGameBegin();
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void DoGameBegin()
	{
		try
		{
			if (gameControl != null)
			{
				gameControl.SendMessage("OnGame", SendMessageOptions.DontRequireReceiver);
			}
			gameBegin = true;
			UnregisterTNetCallBacks();
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void SendRoomStart()
	{
		try
		{
			if (TNetManager.Connection != null)
			{
				CGUI.Log("SendRoomStart:" + TNetManager.Connection.Myself.Name);
				TNetManager.Connection.Send(new RoomStartRequest());
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void OnConnectionLost(TNetEventData evt)
	{
		try
		{
			CGUI.Log("OnConnectionLost");
			TNetManager.ConnectionLost();
			Hashtable hashtable = new Hashtable();
			hashtable.Add("Reason", "RecieveServerMessage");
			FlurryPlugin.logEvent("Online_Disconnect", hashtable);
			Application.LoadLevel("CrazyConnectionLost");
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	public void OnGameBegin()
	{
		try
		{
			CGUI.Log("OnGameBegin:" + TNetManager.Connection.Myself.Name);
			SendRoomStart();
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}
}
