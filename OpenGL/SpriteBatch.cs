using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace FlappyBird.OpenGL
{
  public class SpriteBatch
  {
    private readonly int h, w;

    public SpriteBatch(Size gw)
    {
      w = gw.Width;
      h = gw.Height;
    }

    public void SetCamera(Rectangle r)
    {
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadIdentity();

      GL.Ortho(r.X, r.X + r.Width, r.Y + r.Height, r.Y, -1, 1);

      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadIdentity();
    }

    public void SetWindow(Rectangle r)
    {
      GL.Viewport(r.X, h - (r.Y + r.Height), r.Width, r.Height);
    }

    public void Clear(Color c)
    {
      GL.ClearColor(c.R/255.0f, c.G/255.0f, c.B/255.0f, c.A/255.0f);

      GL.Clear(ClearBufferMask.ColorBufferBit);
    }

    public void SetBlend(Color c)
    {
      GL.Color4(c.R, c.G, c.B, c.A);
    }

    public void ResetBlend()
    {
      GL.Color4(255, 255, 255, 255);
    }

    public void BlitRectangle(Rectangle r)
    {
      GL.Disable(EnableCap.Texture2D);

      GL.Begin(BeginMode.Quads);

      GL.Vertex2(r.X, r.Y);
      GL.Vertex2(r.X + r.Width, r.Y);
      GL.Vertex2(r.X + r.Width, r.Y + r.Height);
      GL.Vertex2(r.X, r.Y + r.Height);

      GL.End();

      GL.Enable(EnableCap.Texture2D);
    }

    public void Translate(int x, int y)
    {
      GL.Translate(x, y, 0.0);
    }

    public void Scale(int xPercent, int yPercent)
    {
      GL.Scale(xPercent/100.0, yPercent/100.0, 1.0);
    }

    public void Rotate(double degrees)
    {
      GL.Rotate(degrees, 0.0, 0.0, 1.0);
    }

    public void Push()
    {
      GL.PushMatrix();
    }

    public void Pop()
    {
      GL.PopMatrix();
    }

    public void LoadIdentity()
    {
      GL.LoadIdentity();
    }

    public Surface CreateSurface(Rectangle r)
    {
      var temp = CreateSurface(r.Width, r.Height);
      int yInvert;

      byte[] tBuff = new byte[r.Width*r.Height*4];
      int pos;
      Color c;
      yInvert = r.Height;

      GL.ReadBuffer(ReadBufferMode.Back);
      GL.Flush();

      GL.ReadPixels(r.X, h - (r.Y + r.Height), r.Width, r.Height, PixelFormat.Rgba, PixelType.UnsignedByte, tBuff);

      temp.Lock();

      for (var y = 0; y < r.Height; y += 1)
      {
        yInvert -= 1;
        for (var x = 0; x < r.Width; x += 1)
        {
          pos = ((yInvert*r.Width) + x)*4;
          c = Color.FromArgb(255, tBuff[pos + 0], tBuff[pos + 1], tBuff[pos + 2]);

          temp.WritePixel(x, y, c);
        }
      }

      temp.Unlock();

      return temp;
    }

    public static Surface CreateSurface(string path)
    {
      var b = new Bitmap(path);
      var s = CreateSurface(b.Width, b.Height);
      s.Lock();
      for (var x = 0; x < b.Width; x++)
      {
        for (var y = 0; y < b.Height; y++)
        {
          var c = b.GetPixel(x, y);

          if (c.R == 255 && c.G == 0 && c.B == 255)
          {
            s.WritePixel(x, y, Color.FromArgb(0, 0, 0, 0));
          }
          else
          {
            s.WritePixel(x, y, c);
          }
        }
      }
      s.Unlock();
      return s;
    }

    public static Surface CreateSurface(int w, int h)
    {
      var major = int.Parse(GL.GetString(StringName.Version).Split('.')[0]);
      var minor = int.Parse(GL.GetString(StringName.Version).Split('.')[1]);
      Surface s;

      if ((major == 1 && minor >= 4) || major > 1)
      {
        s = new InternalSurface2(w, h);
      }
      else
      {
        s = new InternalSurface(w, h);
      }

      return s;
    }

    public static void DestroySurface(Surface s)
    {
      s.Dispose();
    }

    public void Begin()
    {
      GL.Flush();

      GL.PushAttrib(AttribMask.AllAttribBits);

      GL.MatrixMode(MatrixMode.Color);
      GL.PushMatrix();
      GL.LoadIdentity();

      GL.MatrixMode(MatrixMode.Texture);
      GL.PushMatrix();
      GL.LoadIdentity();

      GL.MatrixMode(MatrixMode.Projection);
      GL.PushMatrix();
      GL.LoadIdentity();

      GL.MatrixMode(MatrixMode.Modelview);
      GL.PushMatrix();
      GL.LoadIdentity();

      SetWindow(new Rectangle(0, 0, w, h));
      SetCamera(new Rectangle(0, 0, w, h));

      GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

      GL.Enable(EnableCap.Blend);
      GL.Enable(EnableCap.Texture2D);

      GL.Disable(EnableCap.DepthTest);
      GL.Disable(EnableCap.CullFace);
    }

    public void End()
    {
      GL.Flush();

      GL.MatrixMode(MatrixMode.Texture);
      GL.PopMatrix();

      GL.MatrixMode(MatrixMode.Projection);
      GL.PopMatrix();

      GL.MatrixMode(MatrixMode.Modelview);
      GL.PopMatrix();

      GL.MatrixMode(MatrixMode.Color);
      GL.PopMatrix();

      GL.PopAttrib();
    }

    public void Draw(Surface s)
    {
      s.Blit(new Rectangle(0, 0, s.Size.Width, s.Size.Height));
    }

    public void Draw(Surface s, int x, int y)
    {
      s.Blit(new Rectangle(x, y, s.Size.Width, s.Size.Height));
    }

    public void Draw(Surface s, Rectangle dest)
    {
      s.Blit(dest);
    }

    public void Draw(Surface s, Rectangle srs, Rectangle dest)
    {
      s.Blit(srs, dest);
    }
  }
}