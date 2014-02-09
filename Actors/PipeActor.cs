using System.Drawing;

namespace FlappyBird.Actors
{
  public class PipeActor : Actor
  {
    private readonly Sprite pipeSprite;

    public PipeActor(Core core, int x, int y, bool inverted) : base(core, x, y)
    {
      width = 26;

      if (inverted)
      {
        height = 135;
        pipeSprite = new Sprite(new Rectangle(302, 0, width, height));
      }
      else
      {
        height = 200 - y;
        pipeSprite = new Sprite(new Rectangle(330, 0, width, height));
      }
    }

    public override void Update()
    {
      base.Update();

      x -= 1;
    }

    public override void Render()
    {
      base.Render();

      core.Renderer.RenderSpriteS(pipeSprite, x, y, Color.White);
    }
  }
}
