using System;

namespace FlappyBird.OpenGL
{
  internal class SpriteDrawingException
    : ApplicationException
  {
    public SpriteDrawingException(string message)
      : base(message)
    {
    }
  }
}