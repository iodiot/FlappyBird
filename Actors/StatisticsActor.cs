using System.Drawing;

namespace FlappyBird.Actors
{
  public class StatisticsActor : Actor
  {
    private readonly Sprite boardSprite;
    private readonly ScoreActor scoreActor, maxScoreActor;

    public StatisticsActor(Core core, int score) : base(core, 0, 0)
    {
      width = 113;
      height = 57;

      x = Consts.ScreenWidth/2 - width/2;
      y = 100;

      boardSprite = new Sprite(new Rectangle(146, 58, width, height));

      scoreActor = new ScoreActor(core, 110, 117, score);
      maxScoreActor = new ScoreActor(core, 110, 137, Consts.MaxScore);
    }

    public override void Render()
    {
      base.Render();

      core.Renderer.RenderSpriteS(boardSprite, x, y, Color.White);

      scoreActor.Render();
      maxScoreActor.Render();
    }
  }
}
