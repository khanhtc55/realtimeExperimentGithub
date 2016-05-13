using UnityEngine;
using System.Collections;
using Artemis.System;

public class TestSystem : EntitySystemWithTime
{

    float delay = 5;
    float countdown = 1;
    int count;

    public TestSystem(bool subscribeSimTime)
        : base(subscribeSimTime)
    {

    }

    public override void Process(float deltaTime)
    {
        countdown -= deltaTime;
        if (countdown < 0)
        {
            Debug.Log("1111111 countdown " + count);
            countdown = delay;
            count++;
        }
    }

}
