using OpenTK.Input;

namespace FlappyBird.States
{
  public class StateManager
  {
    private State currentState;

    public StateManager()
    {
      currentState = null;
    }

    public void SetState(State state)
    {
      currentState = state;
    }

    public void OnKeyPress(Key key)
    {
      currentState.OnKeyPress(key);
    }

    public void Update()
    {
      currentState.Update();
    }

    public void Render()
    {
      currentState.Render();
    }
  }
}
