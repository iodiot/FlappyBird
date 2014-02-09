using OpenTK.Input;

namespace FlappyBird.Actors
{
  public class Actor
  {
    protected Core core;

    public int x, y, width, height;

    public Actor(Core core, int x, int y)
    {
      this.core = core;

      this.x = x;
      this.y = y;

      width = 0;
      height = 0;
    }

    public virtual void OnKeyPress(Key key)
    {
      
    }

    public virtual void Update()
    {
      
    }

    public virtual void Render()
    {
      
    }
  }
}
