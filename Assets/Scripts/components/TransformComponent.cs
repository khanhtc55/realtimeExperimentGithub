using UnityEngine;
using System.Collections;
using strange;
using Artemis;
using Artemis.Interface;

public class TransformComponent : IComponent {

    public Vector3 position;
    public Vector3 forward;
    public int jumpTriggerFrame = -1;
    public float landingCountdown = 0;

    public TransformComponent(Vector3 pos, Vector3 forward)
    {
        this.position = pos;
        this.forward = forward;
    }

    public TransformComponent() { }

}
