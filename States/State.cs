using OpenTK.Input;

namespace FlappyBird.States
{
  public class State
  {
    protected readonly Core core;

    public State(Core core)
    {
      this.core = core;
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
