using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace FlappyBird.Actors
{
  public class PlayerBirdActor : BirdActor
  {
    private readonly int maxImpulseTtl = 10;
    private int impulseTtl, ticksAfterLastImpulse;


    public PlayerBirdActor(Core core, int x, int y) : base(core, x, y)
    {
      impulseTtl = 0;
      isDead = false;
    }

    public override void OnGroundCollision()
    {
      isDead = true;
      rotateDegrees = 90;
      y = 185;
    }

    public override void OnKeyPress(Key key)
    {
      if (isDead || impulseTtl > 0)
      {
        return;
      }

      base.OnKeyPress(key);

      if (key == Key.Space)
      {
        impulseTtl = maxImpulseTtl;
      }
    }

    private void ApplyImpulse()
    {
      y -= 3;
      rotateDegrees = -35.0 * Math.Sin((double)(maxImpulseTtl - impulseTtl) / (double)maxImpulseTtl * Math.PI);
    }

    public override void Update()
    {
      base.Update();

      if (isDead)
      {
        return;
      }

      if (impulseTtl > 0)
      {
        ApplyImpulse();
        --impulseTtl;
        ticksAfterLastImpulse = 0;
      }
      else
      {
        ++ticksAfterLastImpulse;
      }

      if (ticksAfterLastImpulse > 5)
      {
        if (rotateDegrees <= 90.0)
        {
          rotateDegrees += 5.0;
        }
      }

      ApplyGravity();
    }

    private void ApplyGravity()
    {
      y += 1;

      if (ticksAfterLastImpulse > 5)
      {
        y += ticksAfterLastImpulse/3;
      }
    }
  }
}
