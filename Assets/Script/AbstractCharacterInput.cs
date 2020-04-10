using System;
using UnityEngine;

public abstract class AbstractCharacterInput : MonoBehaviour
{
    public abstract bool Left { get; }
    public abstract bool Right { get; }
    public abstract bool Up { get; }
    public abstract bool Down { get; }

    public abstract bool LeftDown { get; }
    public abstract bool RightDown { get; }
    public abstract bool UpDown { get; }
    public abstract bool DownDown { get; }

    public abstract bool LeftUp { get; }
    public abstract bool RightUp { get; }
    public abstract bool UpUp { get; }
    public abstract bool DownUp { get; }

    public abstract void Lock();
    public abstract void Unlock();

    // public abstract Vector2 MousePosition { get; }
    // public abstract Vector2 RawMousePosition { get; }
    // public abstract Action<Vector2> MouseMoveEvent { set; }
    // public abstract Action<Vector2> MouseDownEvent { set; }
    // public abstract Action<Vector2> MouseUpEvent { set; }
}