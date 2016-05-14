using UnityEngine;
using System.Collections;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using rot.command;
using nFury.Utils.Core;
using rot.main.datamanager;

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

	void ProcessUpdateSnapshot (ISFSObject param)
	{
//		UpdateSnapshotData data = new UpdateSnapshotData();
	}

	void ProcessUserInputSignal (ISFSObject param)
	{
		Service.Get<SignalManager>().receiveUserInputSignal.Dispatch();
	}
}
