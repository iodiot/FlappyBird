using System.Drawing;

namespace FlappyBird.Actors
{
  public class BirdActor : Actor
  {
    private readonly Sprite[] birdSprites;
    private int currentSpriteN;
    
    public double rotateDegrees;
    public bool isDead;

    public BirdActor(Core core, int x, int y) : base(core, x, y)
    {
      width = 17;
      height = 12;

      birdSprites = new Sprite[4];

      birdSprites[0] = new Sprite(new Rectangle(264, 64, width, height));
      birdSprites[1] = new Sprite(new Rectangle(264, 90, width, height));
      birdSprites[2] = new Sprite(new Rectangle(223, 124, width, height));
      birdSprites[3] = new Sprite(new Rectangle(264, 90, width, height));

      currentSpriteN = 0;

      rotateDegrees = 0.0;

      isDead = false;
    }

    public virtual void OnGroundCollision()
    {
    }

    public override void Update()
    {
      base.Update();

      if (!isDead)
      {
        currentSpriteN = (core.GetTicks()/5)%birdSprites.Length;
      }
      else
      {
        currentSpriteN = 0;
      }
    }

    public override void Render()
    {
      base.Render();

      core.Renderer.RenderSpriteS(birdSprites[currentSpriteN], x, y, Color.White, 1, rotateDegrees);
    }
  }
}
