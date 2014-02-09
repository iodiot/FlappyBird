using System.Drawing;

namespace FlappyBird
{
  public class Sprite
  {
    public Rectangle Rect { get; private set; }
    public int Width { get { return Rect.Width; } }
    public int Height { get { return Rect.Height; } }

    public Sprite(Rectangle rect)
    {
      Rect = rect;
    }
  }
}