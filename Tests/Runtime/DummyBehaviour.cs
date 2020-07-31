using UnityEngine;

public class DummyBehaviour : MonoBehaviour
{
    public bool Invoked { get; private set; }

    public int Obj { get; private set; }

    public int Arg1 { get; private set; }
    public int Arg2 { get; private set; }

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
}
