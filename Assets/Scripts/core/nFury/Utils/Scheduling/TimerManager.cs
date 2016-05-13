
using System;
using System.Collections.Generic;

namespace nFury.Utils.Scheduling
{
  public class TimerManager
  {
    private const uint ONE_DAY = 86400000U;
    private const uint MAX_DELAY = 432000000U;
    private const uint REBASE_TIME = 60000U;
    protected const uint INFINITY = 4294967295U;
    private uint idLast;
    private TimerList timers;
    private uint timeNow;
    private uint timeNext;
    private List<Timer> timersToFire;

    public TimerManager()
    {
      this.idLast = 0U;
      this.timers = new TimerList();
      this.timeNow = 0U;
      this.timeNext = uint.MaxValue;
      this.timersToFire = new List<Timer>();
    }

    protected uint CreateTimer(uint delay, bool repeat, TimerDelegate callback, object cookie)
    {
      if (delay > 432000000U)
        throw new Exception(string.Format("Timer delay {0} exceeds maximum {1}", (object) delay, (object) 432000000));
      if ((int) delay == 0)
        delay = 1U;
      if (callback == null)
        throw new Exception("Null timer callback not supported nor useful");
      uint next = TimerId.GetNext(ref this.idLast);
      Timer timer = new Timer(next, delay, repeat, callback, cookie);
      uint num = this.timeNow + delay;
      timer.TimeFire = num;
      if (this.timers.Add(timer) == 0)
        this.timeNext = num;
      return next;
    }

    protected void KillTimer(uint id)
    {
      List<Timer> list = this.timers.Timers;
      int index = 0;
      for (int count = list.Count; index < count; ++index)
      {
        if ((int) list[index].Id == (int) id)
        {
          list.RemoveAt(index);
          if (index != 0)
            break;
          this.SetTimeNext();
          break;
        }
      }
    }

    public void EnsureTimerKilled(ref uint id)
    {
      if ((int) id == 0)
        return;
      this.KillTimer(id);
      id = 0U;
    }

    private void SetTimeNext()
    {
      this.timeNext = this.timers.Timers.Count != 0 ? this.timers.Timers[0].TimeFire : uint.MaxValue;
    }

    protected void OnDeltaTime(uint dt)
    {
      this.timeNow += dt;
      if (this.timeNow >= 60000U)
      {
        if ((int) this.timeNext == -1)
          this.timeNow -= 60000U;
        else if (this.timeNext >= 60000U)
        {
          this.timeNow -= 60000U;
          this.timeNext -= 60000U;
          this.timers.Rebase(60000U);
        }
      }
      if (this.timeNext > this.timeNow)
        return;
      int index1 = 0;
      int count1 = this.timersToFire.Count;
      List<Timer> list = this.timers.Timers;
      int count2 = list.Count;
      do
      {
        Timer timer = list[0];
        if (timer.TimeFire <= this.timeNow)
        {
          if (index1 < count1)
            this.timersToFire[index1] = timer;
          else
            this.timersToFire.Add(timer);
          ++index1;
          if (timer.Repeat)
          {
            while (timer.IncTimeFireByDelay() <= this.timeNow)
            {
              if (index1 < count1)
                this.timersToFire[index1] = timer;
              else
                this.timersToFire.Add(timer);
              ++index1;
            }
            this.timers.ReprioritizeFirst();
          }
          else
            list.RemoveAt(0);
        }
        else
          break;
      }
      while (--count2 != 0);
      if (index1 <= 0)
        return;
      this.SetTimeNext();
      for (int index2 = 0; index2 < index1; ++index2)
      {
        Timer timer = this.timersToFire[index2];
        this.timersToFire[index2] = (Timer) null;
        timer.Callback(timer.Id, timer.Cookie);
      }
    }
  }
}
