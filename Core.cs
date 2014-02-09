using System;
using System.Drawing;
using FlappyBird.OpenGL;
using FlappyBird.States;
using OpenTK.Input;

namespace FlappyBird
{
  public sealed class Core
  {
    private readonly Renderer renderer;
    private readonly SpriteBatch spriteBatch;
    private readonly Random random;
    private readonly StateManager stateManager;

    private int ticks;

    public Renderer Renderer { get { return renderer; } }
    public StateManager StateManager { get { return stateManager; }}

    public Core(SpriteBatch spriteBatch, Size viewport)
    {
      random = new Random();
      
      viewport.Height /= Consts.ScreenScale;
      viewport.Width /= Consts.ScreenScale;

      this.spriteBatch = spriteBatch;
      renderer = new Renderer(this, spriteBatch, viewport.Width, viewport.Height);

      stateManager = new StateManager();
      stateManager.SetState(new MenuState(this));;

      ticks = 0;
    }

    public int GetTicks()
    {
      return ticks;
    }

    public void OnKeyPress(Key key)
    {
      stateManager.OnKeyPress(key);
    }

    public void Update(int ticks)
    {
      this.ticks = ticks;

      stateManager.Update();
    }

    public void Render()
    {
      spriteBatch.Clear(Color.White);

      spriteBatch.Begin();

      spriteBatch.Push();
      spriteBatch.Scale(Consts.ScreenScale * 100, Consts.ScreenScale * 100);
      stateManager.Render();
      spriteBatch.Pop();

      spriteBatch.End();
    }

    public int GetRandom(int max)
    {
      return random.Next(max);
    }
  }
}
