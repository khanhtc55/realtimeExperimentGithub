
using nFury.Utils.Core;


namespace nFury.Utils.Scheduling
{
  public class SimTimerManager : TimerManager, ISimTimeObserver
  {
    public SimTimerManager()
    {
      Service.Set<SimTimerManager>(this);
      Service.Get<SimTimeEngine>().RegisterSimTimeObserver((ISimTimeObserver) this);
    }

    public uint CreateSimTimer(uint delay, bool repeat, TimerDelegate callback, object cookie)
    {
      return this.CreateTimer(delay, repeat, callback, cookie);
    }

    public void KillSimTimer(uint id)
    {
      this.KillTimer(id);
    }

    public void OnSimTime(uint dt)
    {
      this.OnDeltaTime(dt);
    }
  }
}
