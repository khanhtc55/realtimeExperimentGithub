

using UnityEngine;

namespace nFury.Utils.Animation.Anims
{
  public class AnimColor : Anim
  {
    private Color start;
    private Color end;
    private Color delta;
    private Material material;

    public AnimColor(Material m, float duration, Color startColor, Color endColor)
    {
      this.material = m;
      this.start = startColor;
      this.end = endColor;
      this.Duration = duration;
    }

    public override void OnBegin()
    {
      this.delta = new Color(this.end.r - this.start.r, this.end.g - this.start.g, this.end.b - this.start.b, this.end.a - this.start.a);
    }

    public override void OnUpdate(float dt)
    {
      this.material.color = new Color(this.EaseFunction(this.Age, this.start.r, this.delta.r, this.Duration), this.EaseFunction(this.Age, this.start.g, this.delta.g, this.Duration), this.EaseFunction(this.Age, this.start.b, this.delta.b, this.Duration), this.EaseFunction(this.Age, this.start.a, this.delta.a, this.Duration));
    }
  }
}
