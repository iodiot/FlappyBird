using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlappyBird.Actors;
using OpenTK.Graphics;
using OpenTK.Input;

namespace FlappyBird.States
{
  public class ActionState : State
  {
    private readonly SkyLineActor skyLineActor;
    private readonly GroundActor groundActor;
    private readonly List<PipeActor> pipeActors;
    private readonly BirdActor birdActor;
    private readonly ScoreActor scoreActor;

    private int nextPipeTick, ticks;

    public ActionState(Core core, BirdActor birdActor, GroundActor groundActor, ScoreActor scoreActor) : base(core)
    {
      skyLineActor = new SkyLineActor(core);
      this.groundActor = new GroundActor(core, groundActor.x);
      this.birdActor = new PlayerBirdActor(core, birdActor.x, birdActor.y);
      pipeActors = new List<PipeActor>();
      this.scoreActor = scoreActor;

      this.birdActor.OnKeyPress(Key.Space);

      nextPipeTick = 100;
      ticks = 0;
    }

    private int GetScore()
    {
      var score = 0;

      foreach (var pipeActor in pipeActors)
      {
        if (birdActor.x > pipeActor.x)
        {
          ++score;
        }
      }

      return score/2;
    }

    private void CheckCollisions()
    {
      // Against pipes
      var birdRect = new Rectangle(birdActor.x, birdActor.y, birdActor.width, birdActor.height);
      foreach (var pipeActor in pipeActors)
      {
        var pipeRect = new Rectangle(pipeActor.x, pipeActor.y, pipeActor.width, pipeActor.height);

        if (birdRect.IntersectsWith(pipeRect))
        {
          core.StateManager.SetState(new GameOverState(core, birdActor, groundActor, pipeActors, scoreActor));
        }
      }

      // Against ground
      if (birdActor.y + birdActor.height >= groundActor.y - 5)
      {
        birdActor.OnGroundCollision();
        core.StateManager.SetState(new GameOverState(core, birdActor, groundActor, pipeActors, scoreActor));
      }
    }

    public override void OnKeyPress(Key key)
    {
      base.OnKeyPress(key);

      if (key == Key.Space)
      {
        birdActor.OnKeyPress(key);
      }
    }

    public override void Update()
    {
      base.Update();

      skyLineActor.Update();
      birdActor.Update();
      groundActor.Update();

      if (ticks == nextPipeTick)
      {
        var dy = core.GetRandom(70) - 35;

        var z = (Consts.ScreenHeight - groundActor.height)/2 + 30  + dy;

        pipeActors.Add(new PipeActor(core, Consts.ScreenWidth, z - 50 - 135, true));
        pipeActors.Add(new PipeActor(core, Consts.ScreenWidth, z, false));
        
        nextPipeTick += 50 + core.GetRandom(50);
      }

      foreach (var pipeActor in pipeActors)
      {
        pipeActor.Update();
      }

      CheckCollisions();

      scoreActor.score = GetScore();

      ++ticks;
    }

    public override void Render()
    {
      base.Render();

      skyLineActor.Render();

      foreach (var pipeActor in pipeActors)
      {
        pipeActor.Render();
      }

      birdActor.Render();

      groundActor.Render();
      
      scoreActor.Render();
    }
  }
}
