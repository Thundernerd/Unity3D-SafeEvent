using System;
using TNRD.Events;
using UnityEngine;

public class DummyBehaviour : MonoBehaviour
{
    public bool Invoked { get; private set; }

    public int Obj { get; private set; }

    public int Arg1 { get; private set; }
    public int Arg2 { get; private set; }

    private SafeEvent onExposedEvent;

    public event Action OnExposedEvent
    {
        add => onExposedEvent.Subscribe(value);
        remove => onExposedEvent.Unsubscribe(value);
    }

    public void Dummy()
    {
        Invoked = true;
    }

    public void Dummy(int obj)
    {
        Obj = obj;
    }

    public void Dummy(int arg1, int arg2)
    {
        Arg1 = arg1;
        Arg2 = arg2;
    }

    public void DispatchOnExposedEvent()
    {
        onExposedEvent.Invoke();
    }
}
