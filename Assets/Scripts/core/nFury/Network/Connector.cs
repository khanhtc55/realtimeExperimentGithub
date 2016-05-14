using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using System.Collections.Generic;
using Sfs2X.Requests;
using core.nFury.Network;

public class Connector : MonoBehaviour
{
    public delegate void OnConnectionHandler(bool success);

    public delegate void OnConnectionLostHanlder(string error);

    public delegate void OnLoginHandler(User user, ISFSObject data);

    public delegate void OnLoginErrorHandler(int errorCode, string errorMsg);

    public delegate void OnRoomJoinHandler(Room room);

    public delegate void OnUserEnterRoomHandler(Room room, User user);

    public delegate void OnUserExitRoomHandler(Room room, User user);

    public delegate void OnUserUpdateVariablesHandler(User user, ArrayList changedVars);

    public delegate void OnExtensionResponseHandler(string cmd, int sourceRoom, ISFSObject param, long packetId);

    public delegate void OnSendRequestHandler(string cmd);

    public event OnConnectionHandler OnConnectionEvent;
    public event OnConnectionLostHanlder OnConnectionLostEvent;
    public event OnLoginHandler OnLoginEvent;
    public event OnLoginErrorHandler OnLoginErrorEvent;
    public event OnRoomJoinHandler OnRoomJoinEvent;
    public event OnUserEnterRoomHandler OnUserEnterRoomEvent;
    public event OnUserExitRoomHandler OnUserExitRoomEvent;
    public event OnUserUpdateVariablesHandler OnUserUpdateVariablesEvent;
    public event OnExtensionResponseHandler OnExtensionResponseEvent;
    public event OnSendRequestHandler OnSendRequestEvent;

    public string serverName = "192.168.1.190";
    public int serverPort = 9933;
    public bool debug = false;
    public string zone = "RotZone";

    protected SmartFox smartFox;
    private bool shuttingDown = false;

    private void Awake()
    {
        if (Application.isWebPlayer || Application.isEditor)
        {
            if (!Security.PrefetchSocketPolicy(serverName, serverPort, 500))
            {
//				Debug.LogError("Security Exception. Policy file load failed!");
            }
        }

        //If this is the first time we've been here, keep the Lobby around
        //even when we load another scene, this will remain with all its data
        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void Init()
    {
        smartFox = new SmartFox(debug);
        smartFox.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        smartFox.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        smartFox.AddEventListener(SFSEvent.LOGIN, OnLogin);
        smartFox.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        smartFox.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
        smartFox.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
        smartFox.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
        smartFox.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, OnUserUpdateVariables);
        smartFox.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
    }

    public void Disconect()
    {
        smartFox.Disconnect();
		Init();
    }
    protected void Connect()
    {
        smartFox.Connect(serverName, serverPort);
    }

    public void SendSystemRequest(IRequest request)
    {
        if (smartFox.IsConnected)
        {
            smartFox.Send(request);
        }
        else
        {
            Debug.LogError("Must connect to server before send message");
        }
    }

    public void SendExtensionRequest(BaseReq req)
    {
        if (smartFox.IsConnected)
        {
            OnSendRequestEvent.Invoke(req.CmdId);
            smartFox.Send(new ExtensionRequest(req.CmdId, req.GetParams(), smartFox.LastJoinedRoom));
        }
        else
        {
            Debug.LogError("Must connect to server before send message");
        }
    }

	public Dictionary<string, object> MySelfProperties()
	{
		return smartFox.MySelf.Properties;
	}

    // Update is called once per frame
    private void Update()
    {
        if (smartFox != null)
        {
            smartFox.ProcessEvents();
        }
    }

    private void OnApplicationQuit()
    {
        shuttingDown = true;

        if (smartFox.IsConnected)
        {
            smartFox.Disconnect();
        }
    }

    private void OnConnection(BaseEvent evt)
    {
        if (OnConnectionEvent != null)
        {
            DebugLog("OnConnection");
            bool success = (bool) evt.Params["success"];
			if(!success) Init();
            OnConnectionEvent.Invoke(success);
        }
    }

    private void OnConnectionLost(BaseEvent evt)
    {
        if (OnConnectionLostEvent != null)
        {
            DebugLog("OnConnectionLost");
            OnConnectionLostEvent.Invoke(evt.Params["reason"] as string);
        }
		Init();
    }

    private void OnLogin(BaseEvent evt)
    {
        User user = evt.Params["user"] as User;
        ISFSObject data = evt.Params["data"] as ISFSObject;
        if (OnLoginEvent != null)
        {
            DebugLog("OnLogin");
            OnLoginEvent.Invoke(user, data);
        }
    }

    private void OnLoginError(BaseEvent evt)
    {
        string error = evt.Params["errorMessage"] as string;
        int errorCode = int.Parse(evt.Params["errorCode"].ToString());
		Init();
        if (OnLoginErrorEvent != null)
        {
            OnLoginErrorEvent.Invoke(errorCode, error);
        }
    }

    private void OnRoomJoin(BaseEvent evt)
    {
        DebugLog("OnRoomJoin");
        Room room = evt.Params["room"] as Room;
        if (OnRoomJoinEvent != null)
        {
            OnRoomJoinEvent(room);
        }
    }

    private void OnUserEnterRoom(BaseEvent evt)
    {
        Room room = (Room) evt.Params["room"];
        User user = (User) evt.Params["user"];
        if (OnUserEnterRoomEvent != null)
        {
            OnUserEnterRoomEvent(room, user);
        }
    }

    private void OnUserExitRoom(BaseEvent evt)
    {
        Room room = (Room) evt.Params["room"];
        User user = (User) evt.Params["user"];
        if (OnUserExitRoomEvent != null)
        {
            OnUserExitRoomEvent(room, user);
        }
    }

    private void OnUserUpdateVariables(BaseEvent evt)
    {
        User user = (User) evt.Params["user"];
        ArrayList changedVars = (ArrayList) evt.Params["changedVars"];
        if (OnUserUpdateVariablesEvent != null)
        {
            OnUserUpdateVariablesEvent(user, changedVars);
        }
    }

    private void OnExtensionResponse(BaseEvent evt)
    {
        DebugLog("extension response");
        string cmd = (string) evt.Params["cmd"];
        int sourceRoom = evt.Params.Contains("sourceRoom") ? (int) evt.Params["sourceRoom"] : -1;
        ISFSObject param = (ISFSObject) evt.Params["params"];
        long packetId = evt.Params.Contains("packetId") ? (long) evt.Params["packetId"] : -1;
        if (OnExtensionResponseEvent != null)
        {
            OnExtensionResponseEvent(cmd, sourceRoom, param, packetId);
        }
    }

    public void DebugLog(string text)
    {
        if (debug) Debug.Log(text);
    }

    public bool IsConnected()
    {
        return smartFox.IsConnected;
    }
}