using System;

namespace FlappyBird.OpenGL
{
  public class LockedSurfaceException
    : ApplicationException
  {
    public LockedSurfaceException(string message)
      : base(message)
    {
    }
  }
}