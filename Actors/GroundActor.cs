using System.Drawing;

namespace FlappyBird.Actors
{
  public class GroundActor : Actor
  {
    private readonly Sprite groundSprite;

    public GroundActor(Core core, int x = 0) : base(core, x, 0)
    {
      groundSprite = new Sprite(new Rectangle(146, 0, 153, 55));

      y = Consts.ScreenHeight - groundSprite.Height;

      width = Consts.ScreenWidth;
      height = groundSprite.Height;
    }

    public override void Update()
    {
      base.Update();

      var n = groundSprite.Width - Consts.ScreenWidth - 3;
      x = 2 * core.GetTicks() % n;
    }

    public override void Render()
    {
      base.Render();

      core.Renderer.RenderSpriteS(groundSprite, -x, y, Color.White);
    }
  }
}
