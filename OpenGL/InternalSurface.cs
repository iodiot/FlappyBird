using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace FlappyBird.OpenGL
{
  internal class InternalSurface
    : Surface
  {
    private readonly int chunkHeight;
    private readonly SurfaceChunk[] chunkList;
    private readonly int chunkWidth;
    private readonly int[] textureList;
    private readonly int xChunks;
    private readonly int yChunks;
    
    public InternalSurface(int width, int height)
      : base(width, height)
    {
      chunkWidth = 512;
      chunkHeight = 512;

      w = width;
      h = height;

      if (width < 512)
      {
        chunkWidth = 256;

        if (width < 256)
        {
          chunkWidth = 128;

          if (width < 128)
          {
            chunkWidth = 64;
          }
        }
      }

      if (height < 512)
      {
        chunkHeight = 256;

        if (height < 256)
        {
          chunkHeight = 128;

          if (height < 128)
          {
            chunkHeight = 64;
          }
        }
      }

      xChunks = (int) Math.Ceiling(((width))/((double) (chunkWidth))) + 1;
      yChunks = (int) Math.Ceiling(((height))/((double) (chunkHeight))) + 1;

      chunkList = new SurfaceChunk[xChunks*yChunks];

      textureList = new int[xChunks*yChunks];

      GL.GenTextures((xChunks*yChunks), textureList);

      for (int i = 0; i < (xChunks*yChunks); i += 1)
      {
        chunkList[i] = SurfaceChunk.Build(chunkWidth, chunkHeight, textureList[i]);
      }

      isLocked = false;
    }

    public override void Dispose()
    {
      if (!isDisposed)
      {
        try
        {
          GL.DeleteTextures((xChunks*yChunks), textureList);
        }
        catch (AccessViolationException e)
        {
#if DEBUG
                    System.Console.WriteLine(e);
                    System.Console.WriteLine("OpenGL context expired before Dispose called");
                    System.Console.Read();
#endif
        }
        finally
        {
          GC.SuppressFinalize(this);
          isDisposed = true;
        }
      }
    }

    ~InternalSurface()
    {
      Dispose();
    }

    public override void Lock()
    {
      if (isLocked)
      {
        throw new LockedSurfaceException("Attempting to lock a locked surface");
      }
      
      isLocked = true;
    }

    public override void Unlock()
    {
      if (!isLocked)
      {
        throw new LockedSurfaceException("Attempting to unlock an unlocked surface.");
      }

      for (var i = 0; i < (xChunks * yChunks); i += 1)
      {
        GL.BindTexture(TextureTarget.Texture2D, chunkList[i].texID);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Four, chunkList[i].w, chunkList[i].h, 0,
                      PixelFormat.Rgba, PixelType.UnsignedByte, chunkList[i].data);
      }

      isLocked = false;
    }

    public override Color ReadPixel(int x, int y)
    {
      int xChunk = x/chunkWidth;
      int yChunk = y/chunkHeight;

      int xOff = x%chunkWidth;
      int yOff = y%chunkHeight;

      var c = new Color();
      var tChunk = chunkList[xChunk + (yChunk*xChunks)];
      var pixOffset = (yOff*tChunk.w + xOff)*4;

      c = Color.FromArgb(tChunk.data[pixOffset + 3], tChunk.data[pixOffset + 0], tChunk.data[pixOffset + 1],
                         tChunk.data[pixOffset + 2]);

      return c;
    }

    public override void WritePixel(int x, int y, Color c)
    {
      if (!isLocked)
        throw new LockedSurfaceException("Attempting to write to a not locked surface.");
      else
      {
        int xChunk = x/chunkWidth;
        int yChunk = y/chunkHeight;

        int xOff = x%chunkWidth;
        int yOff = y%chunkHeight;

        SurfaceChunk tChunk = chunkList[xChunk + (yChunk*xChunks)];
        int pixOffset = (yOff*tChunk.w + xOff)*4;

        tChunk.data[pixOffset + 0] = c.R;
        tChunk.data[pixOffset + 1] = c.G;
        tChunk.data[pixOffset + 2] = c.B;
        tChunk.data[pixOffset + 3] = c.A;
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="sRectangle"></param>
    /// <param name="dRectangle"></param>
    internal override void Blit(Rectangle sRectangle, Rectangle dRectangle)
    {
      if (isLocked)
        throw new LockedSurfaceException("Attempting to blit a locked surface.");
      else
      {
        GL.PushMatrix();

        double xAspect = (dRectangle.Width)/((double) sRectangle.Width);
        double yAspect = (dRectangle.Height)/((double) sRectangle.Height);

        GL.Translate(dRectangle.X, dRectangle.Y, 0.0);
        GL.Scale(xAspect, yAspect, 1.0);

        int edgeLeft = 0, edgeRight = 0, edgeTop = 0, edgeBottom = 0;

        edgeLeft = sRectangle.X/chunkWidth;
        edgeRight = (sRectangle.X + sRectangle.Width)/chunkWidth;

        edgeTop = sRectangle.Y/chunkHeight;
        edgeBottom = (sRectangle.Y + sRectangle.Height)/chunkHeight;

        double u1 = 0.0, v1 = 0.0, u2 = 0.0, v2 = 0.0;

        double sx = 0.0, sy = 0.0, sw = 0.0, sh = 0.0;

        double xAccum = 0.0;
        double yAccum = 0.0;

        for (int i = edgeTop; i <= edgeBottom; i += 1)
        {
          v1 = 0.0;
          v2 = 1.0;
          sy = yAccum;
          sh = (chunkHeight - 1);

          if (i == edgeTop)
          {
            v1 = ((sRectangle.Y%chunkHeight))/((double) chunkHeight);
            sh = (chunkHeight) - (double) (sRectangle.Y%chunkHeight);
          }


          if (i == edgeBottom)
          {
            v2 = ((double) ((sRectangle.Y + sRectangle.Height)%chunkHeight))/(chunkHeight);
            sh = ((sRectangle.Y + sRectangle.Height)%chunkHeight);
          }

          if (edgeTop == edgeBottom)
            sh = sRectangle.Height;

          for (int j = edgeLeft; j <= edgeRight; j += 1)
          {
            u1 = 0.0;
            u2 = 1.0;

            sx = xAccum;
            sw = (chunkWidth - 1);


            if (j == edgeLeft)
            {
              u1 = ((sRectangle.X%chunkWidth))/((double) chunkWidth);
              sw = (chunkWidth) - (double) (sRectangle.X%chunkWidth);
            }

            if (j == edgeRight)
            {
              u2 = ((double) ((sRectangle.X + sRectangle.Width)%chunkWidth))/(chunkWidth);
              sw = ((sRectangle.X + sRectangle.Width)%chunkWidth);
            }

            if (edgeRight == edgeLeft)
            {
              sw = sRectangle.Width;
            }


            GL.BindTexture(TextureTarget.Texture2D, textureList[(i*xChunks) + j]);

            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(u1, v1);
            GL.Vertex2(sx, sy);
            GL.TexCoord2(u2, v1);
            GL.Vertex2(sx + sw, sy);
            GL.TexCoord2(u2, v2);
            GL.Vertex2(sx + sw, sy + sh);
            GL.TexCoord2(u1, v2);
            GL.Vertex2(sx, sy + sh);

            GL.End();


            xAccum += sw;
          }


          xAccum = 0;
          yAccum += sh;
        }

        GL.PopMatrix();
      }
    }

    /// <summary>
    /// </summary>
    protected struct SurfaceChunk
    {
      public byte[] data;
      public int h;
      public int texID;
      public int w;

      /// <summary>
      /// </summary>
      /// <param name="width"></param>
      /// <param name="height"></param>
      /// <param name="tex"></param>
      /// <returns></returns>
      public static SurfaceChunk Build(int width, int height, int tex)
      {
        SurfaceChunk tChunk;

        tChunk.w = width;
        tChunk.h = height;
        tChunk.texID = tex;
        tChunk.data = new byte[4*width*height];

        return tChunk;
      }
    }
  }
}