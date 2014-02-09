using System.Collections.Generic;
using System.Drawing;
using FlappyBird.Actors;
using OpenTK.Input;

namespace FlappyBird.States
{
  public class GameOverState : State
  {
    private readonly SkyLineActor skyLineActor;
    private readonly GroundActor groundActor;
    private BirdActor birdActor;
    private readonly List<PipeActor> pipeActors;
    private readonly Sprite gameOverSprite;
    private readonly ScoreActor scoreActor;
    private readonly StatisticsActor statisticsActor;

    private int stressTtl;

    public GameOverState(Core core, BirdActor birdActor, GroundActor groundActor, List<PipeActor> pipeActors, ScoreActor scoreActor) : base(core)
    {
      gameOverSprite = new Sprite(new Rectangle(146, 199, 94, 19));

      skyLineActor = new SkyLineActor(core);
      this.groundActor = groundActor;
      this.pipeActors = pipeActors;
      this.birdActor = birdActor;
      this.scoreActor = scoreActor;

      if (scoreActor.score > Consts.MaxScore)
      {
        Consts.MaxScore = scoreActor.score;
      }

      statisticsActor = new StatisticsActor(core, scoreActor.score);

      stressTtl = 11;
    }

    public void CheckCollisions()
    {
      // Against ground
      if (birdActor.y + birdActor.height >= groundActor.y- 5)
      {
        birdActor.OnGroundCollision();
      }
    }

    public override void OnKeyPress(Key key)
    {
      base.OnKeyPress(key);

      if (stressTtl > 0)
      {
        return;
      }

      if (key == Key.Space)
      {
        core.StateManager.SetState(new GetReadyState(core));
      }
    }

    public override void Update()
    {
      base.Update();

      if (stressTtl > 0)
      {
        --stressTtl;
        return;
      }

      if (!birdActor.isDead)
      {
        birdActor.Update();
        CheckCollisions();
      }
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

      if (stressTtl == 0)
      {
        core.Renderer.RenderSpriteS(gameOverSprite, Consts.ScreenWidth / 2 - gameOverSprite.Width / 2, 60, Color.White);   
        
        statisticsActor.Render();
      }
      else
      {
        scoreActor.Render();
      }

      // Post-effect
      if (stressTtl > 0)
      {
        if (stressTtl % 2 == 0)
        {
          core.Renderer.Fill(Color.White);
        }
      }
    }
  }
}
