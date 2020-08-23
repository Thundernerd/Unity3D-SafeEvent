using System.Collections;
using NUnit.Framework;
using TNRD.Events;
using UnityEngine;
using UnityEngine.TestTools;

public class SafeEventTwoParametersTests
{
    [Test]
    public void Invoke_On_Behaviour()
    {
        GameObject holder = new GameObject();
        DummyBehaviour dummyBehaviour = holder.AddComponent<DummyBehaviour>();

        SafeEvent<int, int> sf;
        sf.Subscribe(dummyBehaviour.Dummy);

        Assert.AreEqual(0, dummyBehaviour.Arg1);
        Assert.AreEqual(0, dummyBehaviour.Arg2);
        sf.Invoke(1, 2);
        Assert.AreEqual(1, dummyBehaviour.Arg1);
        Assert.AreEqual(2, dummyBehaviour.Arg2);

        Object.Destroy(holder);
    }

    [UnityTest]
    public IEnumerator Invoke_On_Destroyed_Behaviour()
    {
        GameObject holder = new GameObject();
        DummyBehaviour dummyBehaviour = holder.AddComponent<DummyBehaviour>();

        SafeEvent<int, int> sf;
        sf.Subscribe(dummyBehaviour.Dummy);

        Object.Destroy(dummyBehaviour);
        yield return null;

        SubscriptionIsNullException exception = null;
        try
        {
            sf.Invoke(1, 2);
        }
        catch (SubscriptionIsNullException e)
        {
            exception = e;
        }

        Assert.IsNotNull(exception);
        Object.Destroy(holder);
    }

    [UnityTest]
    public IEnumerator Invoke_On_Destroyed_GameObject()
    {
        GameObject holder = new GameObject();
        DummyBehaviour dummyBehaviour = holder.AddComponent<DummyBehaviour>();

        SafeEvent<int, int> sf;
        sf.Subscribe(dummyBehaviour.Dummy);

        Object.Destroy(dummyBehaviour.gameObject);
        yield return null;

        SubscriptionIsNullException exception = null;
        try
        {
            sf.Invoke(1, 2);
        }
        catch (SubscriptionIsNullException e)
        {
            exception = e;
        }

        Assert.IsNotNull(exception);
        Object.Destroy(holder);
    }
}
