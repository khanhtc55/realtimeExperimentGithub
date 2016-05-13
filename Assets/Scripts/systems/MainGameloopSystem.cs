using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artemis.System;
using nFury.Utils.Core;
using Artemis;
using rot.main.datamanager;

public class MainGameloopSystem : EntitySystemWithTime
{
    public Dictionary<int, Entity> roleIdToEntity = new Dictionary<int, Entity>();
    public List<List<FrameData>> frameDataQueue;
    public float futureTimeAdvance = .7f;
    public int frameInputDelayToEffect;

    int framePerSecond = 12;
    float customDeltaTime;
    float countdown;

    bool isGameStarted = false;
    float timer = 0;
    int curGameFrameIndex = -1;

    enum GameloopState { CreateCharacters, runningAI, endgame };
    GameloopState curGameloopState;
    Entity redObject, blueObject;
    float redVelocity = 5;
    float blueVelocity = 2f;
    float changeDirCountdown = .5f;

    public MainGameloopSystem(bool subscribeSimTime)
        : base(subscribeSimTime)
    {
        Service.Set<MainGameloopSystem>(this);
        customDeltaTime = 1f / framePerSecond;
        frameInputDelayToEffect = Mathf.FloorToInt((futureTimeAdvance + .2f) / customDeltaTime) + 1;
    }

    public override void Process(float deltaTime)
    {
        if (!isGameStarted) return;

        timer += deltaTime;
        countdown -= deltaTime;
        if (timer > 0 && countdown <= 0)
        {
            
            curGameFrameIndex++;
            Debug.Log("3333333 sending frame from gameloop, frame " + curGameFrameIndex + " time " + (timer + countdown));
            EntityFrameData[] datas = ProcessGameloop(customDeltaTime, curGameFrameIndex);
            FrameData latestFrame = new FrameData(curGameFrameIndex, 
                curGameloopState == GameloopState.endgame? -1 : timer + countdown, 
                datas);
            countdown += customDeltaTime;



            for (int i = 0; i < frameDataQueue.Count; i++)
            {
                frameDataQueue[i].Add(latestFrame);
                Service.Get<SignalManager>().sendUpdateSnapshotSignal.Dispatch(new UpdateSnapshotData(i, frameDataQueue[i]));
            }


        }
    }

    public void StartIn(float delay)
    {
        isGameStarted = true;
        timer = -delay;
        countdown = delay;
    }

    public void ResetGame()
    {
        isGameStarted = false;
        curGameFrameIndex = -1;
        roleIdToEntity.Clear();
        frameDataQueue = new List<List<FrameData>>();
        frameDataQueue.Add(new List<FrameData>());
        frameDataQueue.Add(new List<FrameData>());

        curGameloopState = GameloopState.CreateCharacters;
    }


    public EntityFrameData[] ProcessGameloop(float deltaTime, int currentGameFrameIndex)
    {
        EntityFrameData[] entityFrameDatas;
        TransformComponent redTrans;
        TransformComponent blueTrans;

        switch (curGameloopState)
        {
            case GameloopState.CreateCharacters:
                redObject = entityWorld.CreateEntity();
                redObject.AddComponent(new TransformComponent(new Vector3(-5, 0, 0), Vector3.right));
                redTrans = redObject.GetComponent<TransformComponent>();

                blueObject = entityWorld.CreateEntity();
                blueObject.AddComponent(new TransformComponent(new Vector3(5, 0, 0), Vector3.left));
                blueTrans = blueObject.GetComponent<TransformComponent>();

                entityFrameDatas = new EntityFrameData[2];
                entityFrameDatas[0] = new EntityFrameData(0, redTrans.position, redTrans.forward);
                entityFrameDatas[1] = new EntityFrameData(1, blueTrans.position, blueTrans.forward);
                curGameloopState = GameloopState.runningAI;
                return entityFrameDatas;
                
            case GameloopState.runningAI:
                redTrans = redObject.GetComponent<TransformComponent>();
                blueTrans = blueObject.GetComponent<TransformComponent>();

                //check collision
                if (squaredDistance(redTrans.position, blueTrans.position) < 1)
                {
                    curGameloopState = GameloopState.endgame;
                    isGameStarted = false;
                    return null;
                }

                //red object turn
                changeDirCountdown -= deltaTime;
                if (changeDirCountdown <= 0)
                {
                    changeDirCountdown = Random.Range(0.8f, 2.0f);
                    Vector2 dir = Random.insideUnitCircle.normalized;
                    redTrans.forward = new Vector3(dir.x, 0, dir.y);
                }

                //update legit pos of red
                Vector3 change = redTrans.forward * redVelocity * deltaTime;
                if (redTrans.position.x + change.x < -10 || redTrans.position.x + change.x > 10)
                {
                    change.x = -change.x;
                    redTrans.forward.x = -redTrans.forward.x;
                }
                if (redTrans.position.z + change.z < -10 || redTrans.position.z + change.z > 10)
                {
                    change.z = -change.z;
                    redTrans.forward.z = -redTrans.forward.z;
                }

                //update pos of red
                redTrans.position += change;
                if (currentGameFrameIndex == redTrans.jumpTriggerFrame) redTrans.position.y = 1.5f;
                if (redTrans.position.y >= 1)
                {
                    redTrans.landingCountdown -= deltaTime;
                    if (redTrans.landingCountdown <= 0) redTrans.position.y = 0;
                }


                //update pos of blue
                blueTrans.forward = (redTrans.position - blueTrans.position);
                blueTrans.forward.y = 0;
                blueTrans.forward = blueTrans.forward.normalized;
                blueTrans.position += blueTrans.forward * blueVelocity * deltaTime;
                if (currentGameFrameIndex == blueTrans.jumpTriggerFrame) blueTrans.position.y = 1.5f;
                if (blueTrans.position.y >= 1)
                {
                    blueTrans.landingCountdown -= deltaTime;
                    if (blueTrans.landingCountdown <= 0) blueTrans.position.y = 0;
                }

                //return data
                entityFrameDatas = new EntityFrameData[2];
                entityFrameDatas[0] = new EntityFrameData(0, redTrans.position, redTrans.forward);
                entityFrameDatas[1] = new EntityFrameData(1, blueTrans.position, blueTrans.forward);
                return entityFrameDatas;

            case GameloopState.endgame:
                break;
        }

        return null;
    }

    public void OnReceiveClientAckData(ClientAckData clientAckData)
    {
        int playerId = clientAckData.playerId;
        List<FrameData> datas = frameDataQueue[playerId];
        while (datas.Count > 0 && datas[0].frameId <= clientAckData.ackFrameId) datas.RemoveAt(0);
    }

    public float squaredDistance(Vector3 a, Vector3 b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.z - b.z) * (a.z - b.z);
    }


    public void OnReceiveUserInput(int playerId, int inputFrame)
    {
        int effectFrame = inputFrame + frameInputDelayToEffect;
        TransformComponent trans;
        if (playerId == 0) trans = redObject.GetComponent<TransformComponent>();
        else trans = blueObject.GetComponent<TransformComponent>();

        trans.jumpTriggerFrame = effectFrame;
        trans.landingCountdown = 1.5f;

        //rewrite frames
        if (effectFrame <= curGameFrameIndex)
        {
            for (int i = 0; i < frameDataQueue.Count; i++)
                while (frameDataQueue[i].Count > 0 &&
                    frameDataQueue[i][frameDataQueue[i].Count - 1].frameId >= effectFrame)
                    frameDataQueue.RemoveAt(frameDataQueue[i].Count - 1);

            TransformComponent redTrans = redObject.GetComponent<TransformComponent>();
            TransformComponent blueTrans = blueObject.GetComponent<TransformComponent>();
            FrameData frameData = Service.Get<VisualSystem>().GetFrameData(effectFrame - 1);
            redTrans.position = frameData.datas[0].posision;
            redTrans.forward = frameData.datas[0].forward;
            blueTrans.position = frameData.datas[1].posision;
            blueTrans.forward = frameData.datas[1].forward;

            for (int i = effectFrame; i <= curGameFrameIndex; i++)
            {
                EntityFrameData[] datas = ProcessGameloop(customDeltaTime, i);
                FrameData latestFrame = new FrameData(curGameFrameIndex, i * customDeltaTime, datas);
                for (int k = 0; k < frameDataQueue.Count; k++)
                    frameDataQueue[k].Add(latestFrame);
            }

            for (int i = 0; i < frameDataQueue.Count; i++)
                Service.Get<SignalManager>().sendUpdateSnapshotSignal.Dispatch(new UpdateSnapshotData(i, frameDataQueue[i]));
            
        }
    }
}

public class UpdateSnapshotData
{
    public int playerId;
    public List<FrameData> snapshots;

    public UpdateSnapshotData(int playerId, List<FrameData> snapshots)
    {
        this.playerId = playerId;
        this.snapshots = snapshots;
    }
}
