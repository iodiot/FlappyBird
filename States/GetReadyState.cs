using System.Drawing;
using FlappyBird.Actors;
using OpenTK.Input;

namespace FlappyBird.States
{
  public class GetReadyState : State
  {
    private readonly SkyLineActor skyLineActor;
    private readonly GroundActor groundActor;
    private readonly FreeBirdActor birdActor;
    private readonly ScoreActor scoreActor;

    private readonly Sprite getReadySprite;

    public GetReadyState(Core core) : base(core)
    {
      skyLineActor = new SkyLineActor(core);
      groundActor = new GroundActor(core);
      birdActor = new FreeBirdActor(core, 30, Consts.ScreenHeight / 2 - 10);
      scoreActor = new ScoreActor(core, Consts.ScreenWidth / 2, 25);

      getReadySprite = new Sprite(new Rectangle(146, 221, 87, 22));
    }

    public override void OnKeyPress(Key key)
    {
      base.OnKeyPress(key);

      if (key == Key.Space)
      {
        core.StateManager.SetState(new ActionState(core, birdActor, groundActor, scoreActor));
      }

      birdActor.OnKeyPress(key);
    }

    public override void Update()
    {
      base.Update();

      birdActor.Update();
      groundActor.Update();
    }

    public override void Render()
    {
      base.Render();

      skyLineActor.Render();
      birdActor.Render();
      groundActor.Render();

      core.Renderer.RenderSpriteS(getReadySprite, Consts.ScreenWidth/2 - getReadySprite.Width/2, 60, Color.White);

      scoreActor.Render();
    }
  }
}
