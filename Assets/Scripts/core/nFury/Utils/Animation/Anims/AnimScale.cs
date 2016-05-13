
using UnityEngine;

namespace nFury.Utils.Animation.Anims
{
  public class AnimScale : AbstractAnimVector
  {
    public Transform Target { get; set; }

    public AnimScale(Transform target, float duration, Vector3 endScale)
    {
      this.Target = target;
      this.Duration = duration;
      this.End = endScale;
    }

    public AnimScale(GameObject target, float duration, Vector3 endScale)
      : this(target.transform, duration, endScale)
    {
    }

    public override void OnBegin()
    {
      this.Start = this.Target.localScale;
      this.Delta = this.End - this.Start;
    }

    public override void OnUpdate(float dt)
    {
      base.OnUpdate(dt);
      this.Target.localScale = this.Vector;
    }

    public void SetEndPos(Vector3 endScale)
    {
      this.Delta = endScale - this.Start;
    }
  }
}
