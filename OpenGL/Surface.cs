using System;
using System.Drawing;

namespace FlappyBird.OpenGL
{
  public abstract class Surface : IDisposable
  {
    protected int h, w;
    protected bool isDisposed = false;
    protected bool isLocked;

    public Rectangle Size { get { return new Rectangle(0, 0, w, h); } }

    protected Surface(int width, int height)
    {
      w = width;
      h = height;
    }

    public abstract void Dispose();
    public abstract void Unlock();
    public abstract void Lock();
    public abstract Color ReadPixel(int x, int y);
    public abstract void WritePixel(int x, int y, Color c);

    internal abstract void Blit(Rectangle sRectangle, Rectangle dRectangle);

    internal void Blit(Rectangle dRectangle)
    {
      Blit(new Rectangle(0, 0, w, h), dRectangle);
    }

    internal void Blit(int x, int y)
    {
      Blit(new Rectangle(x, y, w, h));
    }

    public bool Locked()
    {
      return isLocked;
    }

    public void ReplacePixel(Color src, Color dst)
    {
      Lock();

      for (var y = 0; y < h; y += 1)
      {
        for (var x = 0; x < w; x += 1)
        {
          var c = ReadPixel(x, y);
          if ((c.A == src.A) && (c.B == src.B) && (c.G == src.G) && (c.R == src.R))
            WritePixel(x, y, dst);
        }
      }

      Unlock();
    }
  }
}