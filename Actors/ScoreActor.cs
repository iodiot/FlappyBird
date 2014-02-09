using System.Drawing;

namespace FlappyBird.Actors
{
  public class ScoreActor : Actor
  {
    private readonly Sprite[] digitSprites;

    public int score;

    public ScoreActor(Core core, int x, int y, int score = 0) : base(core, x, y)
    {
      width = 7;
      height = 10;

      digitSprites = new Sprite[10];
      digitSprites[0] = new Sprite(new Rectangle(288, 100, width, height));
      digitSprites[1] = new Sprite(new Rectangle(291, 118, width, height));
      digitSprites[2] = new Sprite(new Rectangle(289, 134, width, height));
      digitSprites[3] = new Sprite(new Rectangle(289, 150, width, height));
      digitSprites[4] = new Sprite(new Rectangle(287, 173, width, height));
      digitSprites[5] = new Sprite(new Rectangle(287, 185, width, height));
      digitSprites[6] = new Sprite(new Rectangle(165, 245, width, height));
      digitSprites[7] = new Sprite(new Rectangle(175, 245, width, height));
      digitSprites[8] = new Sprite(new Rectangle(185, 245, width, height));
      digitSprites[9] = new Sprite(new Rectangle(195, 245, width, height));

      this.score = score;
    }

    public override void Render()
    {
      base.Render();

      var s = score.ToString();

      for (var i = 0; i < s.Length; ++i)
      {
        core.Renderer.RenderSpriteS(digitSprites[s[i] - '0'], x + width * i - (s.Length - 1) * width, y, Color.White);
      }
    }
  }
}
