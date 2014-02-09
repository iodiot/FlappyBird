using System;
using System.Drawing;
using FlappyBird.OpenGL;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace FlappyBird
{
  public sealed class Game : GameWindow
  {
    private readonly SpriteBatch spriteBatch;
    private readonly Core core;

    private int ticks;

    public Game()
      : base(Consts.ScreenWidth * Consts.ScreenScale, Consts.ScreenHeight * Consts.ScreenScale, GraphicsMode.Default, "Flappy bird")
    {
      ticks = 0;
      
      spriteBatch = new SpriteBatch(ClientSize);
      core = new Core(spriteBatch, new Size(Consts.ScreenWidth * Consts.ScreenScale, Consts.ScreenHeight * Consts.ScreenScale));
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);

      GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);

      core.Update(ticks++);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      core.Render();

      SwapBuffers();
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
      core.OnKeyPress(e.Key);
    }

    [STAThread]
    private static void Main()
    {
      // The 'using' idiom guarantees proper resource cleanup.
      // We request 30 UpdateFrame events per second, and unlimited
      // RenderFrame events (as fast as the computer can handle).
      using (var game = new Game())
      {
        game.Run(30.0);
      }
    }
  }
}