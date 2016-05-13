

using UnityEngine;

namespace nFury.Utils.Animation.Anims
{
  public class AnimPosition : AbstractAnimVector
  {
    public Transform Target { get; set; }

    public AnimPosition(Transform target, float duration, Vector3 endPos)
    {
      this.Target = target;
      this.Duration = duration;
      this.End = endPos;
    }

    public AnimPosition(GameObject target, float duration, Vector3 endPos)
      : this(target.transform, duration, endPos)
    {
    }

    public override void OnBegin()
    {
      this.Start = this.Target.localPosition;
      this.Delta = this.End - this.Start;
    }

    public override void OnUpdate(float dt)
    {
      base.OnUpdate(dt);
      this.Target.localPosition = this.Vector;
    }
  }
}
