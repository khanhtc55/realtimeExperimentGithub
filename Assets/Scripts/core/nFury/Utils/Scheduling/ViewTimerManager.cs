
using nFury.Utils.Core;
using System;

namespace nFury.Utils.Scheduling
{
  public class ViewTimerManager : TimerManager, IViewFrameTimeObserver
  {
    public ViewTimerManager()
    {
      Service.Set<ViewTimerManager>(this);
      Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver((IViewFrameTimeObserver) this);
    }

    public uint CreateViewTimer(float delay, bool repeat, TimerDelegate callback, object cookie)
    {
      delay *= 1000f;
      if ((double) delay < 0.0 || (double) delay >= 4294967296.0)
        throw new Exception(string.Format("Timer delay {0} is out of range", (object) delay));
      else
        return this.CreateTimer((uint) delay, repeat, callback, cookie);
    }

    public void KillViewTimer(uint id)
    {
      this.KillTimer(id);
    }

    public void OnViewFrameTime(float dt)
    {
      this.OnDeltaTime((uint) ((double) dt * 1000.0));
    }
  }
}
