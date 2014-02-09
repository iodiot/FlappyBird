using System.Drawing;

namespace FlappyBird.Actors
{
  public class SkyLineActor : Actor
  {
    private readonly Sprite skylineSprite;

    public SkyLineActor(Core core) : base(core, 0, 0)
    {
      width = 143;
      height = 255;

      skylineSprite = new Sprite(new Rectangle(0, 0, width, height));
    }

    public override void Render()
    {
      base.Render();

      core.Renderer.RenderSpriteS(skylineSprite, 0, 0, Color.White);
    }
  }
}
