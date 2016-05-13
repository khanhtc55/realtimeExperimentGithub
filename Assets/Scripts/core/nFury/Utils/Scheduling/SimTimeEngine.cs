using nFury.Utils.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nFury.Utils.Scheduling
{
  public class SimTimeEngine
  {
    private const int MAX_SIM_FRAMES_LAG_BEFORE_RESET = 30;
    private const uint ROLLOVER_MILLISECONDS = 60000U;
    private const float ROLLOVER_SECONDS = 60f;
    private const float LOW_SIM_TIME_PER_FRAME = 0.0055f;
    private uint timePerFrame;
    private uint maxLag;
    private uint timeLast;
    private uint timeAccumulator;
    private float rolloverNext;
    private List<ISimTimeObserver> observers;
    private MutableIterator miter;
    private float scale;
    private float xscale;

    public SimTimeEngine(uint timePerFrame)
    {
      Service.Set<SimTimeEngine>(this);
      this.timePerFrame = timePerFrame;
      this.ScaleTime(1f);
      this.timeLast = this.Now();
      this.timeAccumulator = 0U;
      this.rolloverNext = 0.0f;
      this.observers = new List<ISimTimeObserver>();
      this.miter = new MutableIterator();
    }

    public void RegisterSimTimeObserver(ISimTimeObserver observer)
    {
      if (observer == null || this.observers.IndexOf(observer) >= 0)
        return;
      this.observers.Add(observer);
    }

    public void UnregisterSimTimeObserver(ISimTimeObserver observer)
    {
      int num = this.observers.IndexOf(observer);
      if (num < 0)
        return;
      this.observers.RemoveAt(num);
      this.miter.OnRemove(num);
    }

    public void UnregisterAll()
    {
      this.observers.Clear();
      this.miter.Reset();
    }

    public void ScaleTime(float scale)
    {
      this.scale = scale;
    }

    public bool IsPaused()
    {
      return (int) this.scale == 0;
    }



    public void OnUpdate()
    {
		uint curTime = this.Now();
		uint scaledTimePerFrame = (uint)(timePerFrame * this.scale);
		while(timeLast < curTime)
		{
			this.miter.Init((ICollection) this.observers);
			while (this.miter.Active())
			{
				this.observers[this.miter.Index].OnSimTime(scaledTimePerFrame);
				this.miter.Next();
			}
			this.miter.Reset();
			timeLast += scaledTimePerFrame;
		}
    }

    private uint Now()
    {
		float num = Time.time ;
      	return (uint) Mathf.Floor(num * 1000f);
    }
  }
}
