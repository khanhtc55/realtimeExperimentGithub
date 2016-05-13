
namespace nFury.Utils.Scheduling
{
  public class Timer
  {
    public uint Id;
    public uint Delay;
    public bool Repeat;
    public TimerDelegate Callback;
    public object Cookie;
    private uint timeFire;

    public uint TimeFire
    {
      get
      {
        return this.timeFire;
      }
      set
      {
        this.timeFire = value;
      }
    }

    public Timer(uint id, uint delay, bool repeat, TimerDelegate callback, object cookie)
    {
      this.Id = id;
      this.Delay = delay;
      this.Repeat = repeat;
      this.Callback = callback;
      this.Cookie = cookie;
      this.timeFire = 0U;
    }

    public void DecTimeFire(uint delta)
    {
      this.timeFire -= delta;
    }

    public uint IncTimeFireByDelay()
    {
      return this.timeFire += this.Delay;
    }
  }
}
