using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace FlappyBird.OpenGL
{
  internal class InternalSurface2
    : Surface
  {
    private readonly byte[] data;
    private readonly int texId;

    public InternalSurface2(int w, int h)
      : base(w, h)
    {
      texId = GL.GenTexture();
      data = new byte[w*h*4];

      GL.BindTexture(TextureTarget.Texture2D, texId);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);

      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);

      GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Four, this.w, this.h, 0, PixelFormat.Rgba,
                    PixelType.UnsignedByte, data);

      isLocked = false;
    }

    public override void Dispose()
    {
      GL.DeleteTexture(texId);
    }

    internal override void Blit(Rectangle sRect, Rectangle dRect)
    {
      if (isLocked)
      {
        throw new LockedSurfaceException("Attempting to blit a locked surface");
      }

      GL.PushMatrix();

      GL.BindTexture(TextureTarget.Texture2D, texId);
      GL.Begin(BeginMode.Quads);
      GL.TexCoord2(sRect.Left/(float) w, sRect.Top/(float) h);
      GL.Vertex2(dRect.Left, dRect.Top);
      GL.TexCoord2(sRect.Right/(float) w, sRect.Top/(float) h);
      GL.Vertex2(dRect.Right, dRect.Top);
      GL.TexCoord2(sRect.Right/(float) w, sRect.Bottom/(float) h);
      GL.Vertex2(dRect.Right, dRect.Bottom);
      GL.TexCoord2(sRect.Left/(float) w, sRect.Bottom/(float) h);
      GL.Vertex2(dRect.Left, dRect.Bottom);

      GL.End();

      GL.PopMatrix();
    }

    public override void Lock()
    {
      if (isLocked)
      {
        throw new LockedSurfaceException("Attempting to lock an already locked surface");
      }

      isLocked = true;
    }

    public override void Unlock()
    {
      if (!isLocked)
      {
        throw new LockedSurfaceException("Attempting to unlock a free surface");
      }

      GL.BindTexture(TextureTarget.Texture2D, texId);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);

      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);

      GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Four, w, h, 0, PixelFormat.Rgba,
                    PixelType.UnsignedByte, data);

      isLocked = false;
    }

    public override Color ReadPixel(int x, int y)
    {
      var offset = (y*w + x)*4;
      return Color.FromArgb(data[offset + 3], data[offset + 0], data[offset + 1], data[offset + 2]);
    }

    public override void WritePixel(int x, int y, Color c)
    {
      if (!isLocked)
      {
        throw new LockedSurfaceException("Trying to write to an unlocked surface");
      }

      var offset = (y*w + x)*4;
      data[offset + 0] = c.R;
      data[offset + 1] = c.G;
      data[offset + 2] = c.B;
      data[offset + 3] = c.A;
    }
  }
}