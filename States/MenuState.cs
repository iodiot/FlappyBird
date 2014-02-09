using System.Drawing;
using FlappyBird.Actors;
using OpenTK.Input;

namespace FlappyBird.States
{
  public class MenuState : State
  {
    private readonly SkyLineActor skyLineActor;
    private readonly GroundActor groundActor;
    private readonly BirdActor birdActor;
    private readonly Sprite titleSprite;

    public MenuState(Core core) : base(core)
    {
      skyLineActor = new SkyLineActor(core);
      groundActor = new GroundActor(core);
      birdActor = new FreeBirdActor(core, Consts.ScreenWidth / 2 - 10, Consts.ScreenHeight / 2 - 25);

      titleSprite = new Sprite(new Rectangle(146, 173, 96, 22));
    }

    public override void OnKeyPress(Key key)
    {
      base.OnKeyPress(key);

      if (key == Key.Space)
      {
        core.StateManager.SetState(new GetReadyState(core));
      }
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

      core.Renderer.RenderSpriteS(titleSprite, Consts.ScreenWidth / 2 - titleSprite.Width / 2, Consts.ScreenHeight / 2 - 75, Color.White);
    }
  }
}
