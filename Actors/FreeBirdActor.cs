using System;

namespace FlappyBird.Actors
{
  public class FreeBirdActor : BirdActor
  {
    private int originalY;

    public FreeBirdActor(Core core, int x, int y) : base(core, x, y)
    {
      originalY = y;
    }

    public override void Update()
    {
      base.Update();

      y = originalY + (int)(3.0f * Math.Sin(core.GetTicks() * 0.2f));
    }
  }
}
