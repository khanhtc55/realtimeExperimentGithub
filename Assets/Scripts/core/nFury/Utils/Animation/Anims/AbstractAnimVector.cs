
using UnityEngine;

namespace nFury.Utils.Animation.Anims
{
  public abstract class AbstractAnimVector : Anim
  {
    protected Vector3 Vector;

    public Vector3 Start { get; set; }

    public Vector3 End { get; set; }

    public Vector3 Delta { get; set; }

    public Easing.EasingDelegate EaseFunctionX { get; set; }

    public Easing.EasingDelegate EaseFunctionY { get; set; }

    public Easing.EasingDelegate EaseFunctionZ { get; set; }

    public override Easing.EasingDelegate EaseFunction
    {
      get
      {
        return base.EaseFunction;
      }
      set
      {
        base.EaseFunction = value;
        this.EaseFunctionX = value;
        this.EaseFunctionY = value;
        this.EaseFunctionZ = value;
      }
    }

    public AbstractAnimVector()
    {
      this.Vector = Vector3.zero;
    }

    public override void OnUpdate(float dt)
    {
      this.Vector.x = this.EaseFunctionX(this.Age, this.Start.x, this.Delta.x, this.Duration);
      this.Vector.y = this.EaseFunctionY(this.Age, this.Start.y, this.Delta.y, this.Duration);
      this.Vector.z = this.EaseFunctionZ(this.Age, this.Start.z, this.Delta.z, this.Duration);
    }
  }
}
