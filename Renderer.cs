using System;
using System.Drawing;
using FlappyBird.OpenGL;

namespace FlappyBird
{
  public sealed class Renderer
  {
    private readonly Core core;
    private readonly SpriteBatch spriteBatch;
    private readonly Surface surface;

    public int ScreenWidth { get; private set; }
    public int ScreenHeight { get; private set; }

    public Renderer(Core core, SpriteBatch spriteBatch, int screenWidth, int screenHeight)
    {
      this.core = core;
      this.spriteBatch = spriteBatch;

      ScreenWidth = screenWidth;
      ScreenHeight = screenHeight;

      surface = SpriteBatch.CreateSurface("Content\\SpriteSheet.png");
    }

    #region Common

    public void Fill(Color color)
    {
      spriteBatch.Clear(color);
    }

    public static Rectangle GetTileRect(int x, int y, int tileWidth, int tileHeight)
    {
      return new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);
    }

    #endregion

    public void RenderSpriteS(Sprite sprite, Rectangle srs, Rectangle dest, Color tint, int scale = 1, double degrees = 0.0)
    {
      spriteBatch.Push();

      if (Math.Abs(degrees) > Consts.Eps)
      {
        spriteBatch.Translate(dest.X, dest.Y);
        spriteBatch.Rotate(degrees);
        spriteBatch.Translate(-dest.X, -dest.Y);
      }

      spriteBatch.SetBlend(tint);
      spriteBatch.Scale(scale * 100, scale * 100);

      spriteBatch.Draw(surface, srs, dest);
      spriteBatch.Pop();
    }

    public void RenderSpriteS(Sprite sprite, Rectangle dest, Color tint, int scale = 1, double degrees = 0.0)
    {
      RenderSpriteS(sprite, sprite.Rect, dest, tint, scale);
    }

    public void RenderSpriteS(Sprite sprite, int x, int y, Color tint, int scale = 1, double degrees = 0.0)
    {
      RenderSpriteS(sprite, sprite.Rect, new Rectangle(x, y, sprite.Width, sprite.Height), tint, scale, degrees);
    }
  }
}
