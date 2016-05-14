using UnityEngine;
using System.Collections;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using rot.command;
using nFury.Utils.Core;
using rot.main.datamanager;
using core.nFury.Network;

public class RealtimeTestConnector : Connector {
	public const string FirstSetupCmd = "FirstSetupCmd";
	public const string UpdateSnapshotSignal = "UpdateSnapshotSignal";
	public const string UserInputSignal = "UserInputSignal";

	public RealtimeTestConnector ()
	{
		OnConnectionEvent += HandleOnConnectionEvent;
		OnLoginEvent += HandleOnLoginEvent;
		OnLoginErrorEvent += HandleOnLoginErrorEvent;
		OnConnectionLostEvent += HandleOnConnectionLostEvent;
		OnExtensionResponseEvent += HandleOnExtensionResponseEvent;
	}

	void Start()
	{
	}

	public void DoConnect()
	{
		Connect();
	}

	void HandleOnConnectionEvent (bool success)
	{
		Debug.Log("Connect " + success);
		if(success)
		{
			ISFSObject obj = new SFSObject();
			SendSystemRequest(new LoginRequest(SystemInfo.deviceUniqueIdentifier, "", zone, obj));
		}
	}

	void HandleOnLoginEvent (Sfs2X.Entities.User user, Sfs2X.Entities.Data.ISFSObject data)
	{
	}

	void HandleOnLoginErrorEvent (int errorCode, string errorMsg)
	{
	}

	void HandleOnConnectionLostEvent (string error)
	{
	}

	void HandleOnExtensionResponseEvent (string cmd, int sourceRoom, Sfs2X.Entities.Data.ISFSObject param, long packetId)
	{
		if (param == null) {
			this.DebugLog (GetType () + " HandleOnExtensionResponseEvent: 'param' is null, init new one to prevent null binding");
			param = new SFSObject ();
		}

		switch(cmd)
		{
		case FirstSetupCmd:
			Process1stSetupCmd(param);
			break;
		case UpdateSnapshotSignal:
			ProcessUpdateSnapshot(param);
			break;
		case UserInputSignal:
			ProcessUserInputSignal(param);
			break;
		}
	}

	void Process1stSetupCmd (ISFSObject param)
	{
		FirstSetupData data = new FirstSetupData(param.GetFloat("visualStartIn"),
		                                         param.GetBool("isPlayingServerRole"));
		Service.Get<SignalManager>().sendFirstSetupSignal.Dispatch(data);
	}

	public void SendUpdateSnapshot(int playerId, FrameData frameData)
	{
		ISFSObject param = new SFSObject();
		param.PutInt("playerId",playerId);
		param.PutInt("frameId", frameData.frameId);
		param.PutFloat("time", frameData.time);
		SFSArray arr = new SFSArray();
		for(int i = 0; i < frameData.datas.Length; ++i)
		{
			ISFSObject obj = new SFSObject();
			obj.PutInt("roleId", frameData.datas[i].roleId);
			ISFSObject posData = new SFSObject();
			posData.PutFloat("x", frameData.datas[i].posision.x);
			posData.PutFloat("y", frameData.datas[i].posision.y);
			posData.PutFloat("z", frameData.datas[i].posision.z);
			obj.PutSFSObject("posision", posData);

			ISFSObject forwardData = new SFSObject();
			forwardData.PutFloat("x", frameData.datas[i].forward.x);
			forwardData.PutFloat("y", frameData.datas[i].forward.y);
			forwardData.PutFloat("z", frameData.datas[i].forward.z);
			obj.PutSFSObject("forward", forwardData);
			arr.AddSFSObject(obj);
		}
		param.PutSFSArray("datas", arr);
		smartFox.Send (new ExtensionRequest(UpdateSnapshotSignal, param, smartFox.LastJoinedRoom));
	}

	void ProcessUpdateSnapshot (ISFSObject param)
	{
		int playerId = param.GetInt("playerId");
		int frameId = param.GetInt("frameId");
		float time = param.GetFloat("time");
		int roleId;
		Vector3 posision;
		Vector3 forward;
		SFSArray arr = (SFSArray)param.GetSFSArray("datas");
		EntityFrameData[] entities = new EntityFrameData[arr.Size()];
		for(int i = 0; i < entities.Length; ++i)
		{
			ISFSObject obj = arr.GetSFSObject(i);
			roleId = obj.GetInt("roleId");
			ISFSObject posData = obj.GetSFSObject("posision");
			posision.x = posData.GetFloat("x");
			posision.y = posData.GetFloat("y");
			posision.z = posData.GetFloat("z");

			ISFSObject forwardData = obj.GetSFSObject("forward");
			forward.x = forwardData.GetFloat("x");
			forward.y = forwardData.GetFloat("y");
			forward.z = forwardData.GetFloat("z");
			entities[i] = new EntityFrameData(roleId, posision, forward);
		}

 		FrameData frameData = new FrameData(frameId, time, entities);
		UpdateSnapshotData data = new UpdateSnapshotData(playerId, frameData);
		Service.Get<SignalManager>().receiveUpdateSnapshotSignal.Dispatch(data);
	}

	public void SendUserInputSignal (int userInputFrame)
	{
		ISFSObject param = new SFSObject();
		param.PutInt("userInputFrame", userInputFrame);
		smartFox.Send (new ExtensionRequest(UserInputSignal, param, smartFox.LastJoinedRoom));
	}


	void ProcessUserInputSignal (ISFSObject param)
	{
		int userInputFrame = param.GetInt("userInputFrame");
		Service.Get<SignalManager>().receiveUserInputSignal.Dispatch(userInputFrame);
	}
}
