
using System.Collections.Generic;

namespace nFury.Utils.Scheduling
{
  public class TimerList
  {
    public List<Timer> Timers;

    public TimerList()
    {
      this.Timers = new List<Timer>();
    }

    public virtual int Add(Timer timer)
    {
      uint timeFire = timer.TimeFire;
      int index = 0;
      for (int count = this.Timers.Count; index < count; ++index)
      {
        if (timeFire < this.Timers[index].TimeFire)
        {
          this.Timers.Insert(index, timer);
          return index;
        }
      }
      this.Timers.Add(timer);
      return this.Timers.Count - 1;
    }

    public int ReprioritizeFirst()
    {
      Timer timer1 = this.Timers[0];
      uint timeFire = timer1.TimeFire;
      int index = 1;
      for (int count = this.Timers.Count; index < count; ++index)
      {
        Timer timer2 = this.Timers[index];
        if (timeFire < timer2.TimeFire)
          return index - 1;
        this.Timers[index] = timer1;
        this.Timers[index - 1] = timer2;
      }
      return this.Timers.Count - 1;
    }

    public void Rebase(uint amount)
    {
      int index = 0;
      for (int count = this.Timers.Count; index < count; ++index)
        this.Timers[index].DecTimeFire(amount);
    }
  }
}
