using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artemis.System;
using Artemis;
using nFury.Utils.Core;
using rot.main.datamanager;

public class VisualSystem : EntitySystemWithTime
{
    List<FrameData> gameFrames = new List<FrameData>();
    Dictionary<int, Entity> roleIdToEntity = new Dictionary<int, Entity>();

    public bool isGameStarted = false;
    float timer = 0;
    public int curGameFrameIndex = -1;

    public int playerId;

    public VisualSystem(bool subscribeSimTime)
        : base(subscribeSimTime)
    {
        Service.Set<VisualSystem>(this);
        ResetGame();
    }

    public override void Process(float deltaTime)
    {
        if (!isGameStarted) return;

        timer += deltaTime;
        if (timer >= 0 && gameFrames.Count>0)
        {
            while (curGameFrameIndex < 0 ||
                (curGameFrameIndex < gameFrames.Count - 1 && gameFrames[curGameFrameIndex + 1].time <= timer)) curGameFrameIndex++;

            if (curGameFrameIndex < 0) return;
            if (gameFrames[curGameFrameIndex].time < 0)
            {
                DisplayGameover();
                Debug.Log("555555555 rendering gameover");
                return;
            }
            Debug.Log("4444444 rendering frame " + curGameFrameIndex + " time " + gameFrames[curGameFrameIndex].time);

            FrameData startFrame = gameFrames[curGameFrameIndex];
            InitializeNewObjectInFrame(startFrame);

            if (curGameFrameIndex == gameFrames.Count - 1 || gameFrames[curGameFrameIndex + 1].datas == null) return;
            FrameData endFrame = gameFrames[curGameFrameIndex + 1];
            InitializeNewObjectInFrame(endFrame);

            InterpolateBetweenFrame(startFrame, endFrame);
        }
    }

    public void StartIn(float delay)
    {
        isGameStarted = true;
        timer = -delay;
    }

    public void ResetGame()
    {
        gameFrames.Clear();
        roleIdToEntity.Clear();
        isGameStarted = false;
        curGameFrameIndex = -1;
    }

    public void ReviewGame()
    {
        isGameStarted = true;
        timer = -1;
        curGameFrameIndex = -1;
    }

    public void InitializeNewObjectInFrame(FrameData frameData)
    {
        if (frameData.datas == null) return;
        for (int i = 0; i < frameData.datas.Length; i++)
        {
            if (!roleIdToEntity.ContainsKey(frameData.datas[i].roleId))
            {
                Debug.Log("7777777777777777777 create entity");
                Entity e = Service.Get<EntityWorld>().CreateEntity();
                roleIdToEntity.Add(frameData.datas[i].roleId, e);
            }

            Entity entity = roleIdToEntity[frameData.datas[i].roleId];
            if (!entity.HasComponent<ViewComponent>()) CreateViewComponentForEntity(entity, frameData.datas[i]);
        }
    }

    public void CreateViewComponentForEntity(Entity e, EntityFrameData entityFrameData)
    {
        
        string characterPath = "";
        if (entityFrameData.roleId == 0) characterPath = "PlayerRed";
        else characterPath = "PlayerBlue";
        Debug.Log("88888888888888 create character: " + characterPath);

        e.AddComponent(new ViewComponent(characterPath, entityFrameData.posision, entityFrameData.forward));
    }

    public void InterpolateBetweenFrame(FrameData firstFrame, FrameData endFrame)
    {
        for (int i = 0; i < firstFrame.datas.Length; i++)
        {
            Entity e = roleIdToEntity[firstFrame.datas[i].roleId];
            ViewComponent view = e.GetComponent<ViewComponent>();
            view.lastPos = firstFrame.datas[i].posision;
            view.lastForward = firstFrame.datas[i].forward;
            view.lastFrame = firstFrame.frameId;
        }

        float fracJourney = (timer - firstFrame.time) / (endFrame.time - firstFrame.time);

        for (int i = 0; i < endFrame.datas.Length; i++)
        {
            Entity e = roleIdToEntity[endFrame.datas[i].roleId];
            ViewComponent view = e.GetComponent<ViewComponent>();
            if (view.lastFrame < firstFrame.frameId)
            {
                view.lastFrame = firstFrame.frameId;
                view.lastPos = view.go.transform.position;
                view.lastForward = view.go.transform.forward;
            }

            view.SetPosition(Vector3.Lerp(view.lastPos, endFrame.datas[i].posision, fracJourney));
            view.SetForward(Vector3.RotateTowards(view.go.transform.forward, endFrame.datas[i].forward, 12, 0));
        }
    }

    public void DisplayGameover()
    {
        isGameStarted = false;
    }

    public void AddFrameData(List<FrameData> frameDataQueue)
    {
        int lastFrameId = -1;
        int countBefore = gameFrames.Count;

        if(gameFrames.Count>0)
            lastFrameId = gameFrames[gameFrames.Count - 1].frameId;
        if (frameDataQueue[frameDataQueue.Count - 1].frameId < lastFrameId) return;

        for (int i = 0; i < frameDataQueue.Count; i++)
            if (frameDataQueue[i].frameId > lastFrameId) gameFrames.Add(frameDataQueue[i]);
            else gameFrames[countBefore - (lastFrameId - frameDataQueue[i].frameId + 1)] = frameDataQueue[i];

        Service.Get<SignalManager>().sendClientAckSignal.Dispatch(new ClientAckData(playerId, frameDataQueue[frameDataQueue.Count - 1].frameId));
    }

    public FrameData GetFrameData(int frameIndex)
    {
        for (int i = gameFrames.Count - 1; i >= 0; i--)
        {
            if (gameFrames[i].frameId < frameIndex) return null;
            if (gameFrames[i].frameId == frameIndex) return gameFrames[i];
        }
        return null;
    }
}

public class FrameData
{
    public int frameId;
    public float time;
    public EntityFrameData[] datas;

    public FrameData(int frameId, float time, params EntityFrameData[] entityFrameDatas)
    {
        this.frameId = frameId;
        this.time = time;
        this.datas = entityFrameDatas;
    }

}

public class EntityFrameData
{
    public int roleId;
    public Vector3 posision;
    public Vector3 forward;

    public EntityFrameData(int roleid, Vector3 pos, Vector3 forward)
    {
        this.roleId = roleid;
        this.posision = pos;
        this.forward = forward;
    }
}

public class ClientAckData
{
    public int playerId;
    public int ackFrameId;

    public ClientAckData(int playerId, int ackFrameId)
    {
        this.playerId = playerId;
        this.ackFrameId = ackFrameId;
    }
}
