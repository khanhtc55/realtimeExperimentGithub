
using nFury.Utils.Animation.Anims;
using nFury.Utils.Core;
using nFury.Utils.Scheduling;

using System.Collections.Generic;

namespace nFury.Utils.Animation
{
  public class AnimController : IViewFrameTimeObserver
  {
    private List<Anim> anims;

    public AnimController()
    {
      Service.Set<AnimController>(this);
      this.anims = new List<Anim>();
    }

    public void Animate(Anim anim)
    {
      this.CompleteAnim(anim);
      if ((double) anim.Delay > 0.0)
      {
        anim.DelayTimer = Service.Get<ViewTimerManager>().CreateViewTimer(anim.Delay, false, new TimerDelegate(this.OnDelayComplete), (object) anim);
      }
      else
      {
        anim.DelayTimer = 0U;
        this.InternalAnimate(anim);
      }
    }

    private void InternalAnimate(Anim anim)
    {
      this.anims.Add(anim);
      if (this.anims.Count == 1)
        Service.Get<ViewTimeEngine>().RegisterFrameTimeObserver((IViewFrameTimeObserver) this);
      anim.Begin();
    }

    private void OnDelayComplete(uint id, object cookie)
    {
      Anim anim = cookie as Anim;
      anim.DelayTimer = 0U;
      this.InternalAnimate(anim);
    }

    private void CancelDelayedAnim(Anim anim)
    {
      Service.Get<ViewTimerManager>().KillViewTimer(anim.DelayTimer);
      anim.DelayTimer = 0U;
    }

    private void InternalCompleteAnim(Anim anim)
    {
      anim.Complete();
      if (this.anims.Count != 0)
        return;
      Service.Get<ViewTimeEngine>().UnregisterFrameTimeObserver((IViewFrameTimeObserver) this);
    }

    public void CompleteAnim(Anim anim)
    {
      if ((int) anim.DelayTimer != 0)
        this.CancelDelayedAnim(anim);
      if (!anim.Playing)
        return;
      this.anims.Remove(anim);
      this.InternalCompleteAnim(anim);
    }

    private void CompleteAnimWithIndex(Anim anim, int index)
    {
      if ((int) anim.DelayTimer != 0)
        this.CancelDelayedAnim(anim);
      if (!anim.Playing)
        return;
      this.anims.RemoveAt(index);
      this.InternalCompleteAnim(anim);
    }

    public void OnViewFrameTime(float dt)
    {
      int count = this.anims.Count;
      while (count-- != 0)
      {
        Anim anim = this.anims[count];
        anim.Tick(dt);
        bool flag = (double) anim.Age >= (double) anim.Duration;
        if (flag)
          anim.Age = anim.Duration;
        anim.Update(dt);
        if (flag)
          this.CompleteAnimWithIndex(anim, count);
      }
    }
  }
}
