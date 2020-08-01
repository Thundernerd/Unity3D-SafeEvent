using System.Collections;
using NUnit.Framework;
using TNRD.Events;
using UnityEngine;
using UnityEngine.TestTools;

public class SafeEventTests
{
    [Test]
    public void Invoke_On_Behaviour()
    {
        var holder = new GameObject();
        var dummyBehaviour = holder.AddComponent<DummyBehaviour>();

        SafeEvent sf;
        sf.Subscribe(dummyBehaviour.Dummy);
        sf.Invoke();

        Assert.IsTrue(dummyBehaviour.Invoked);
        Object.Destroy(holder);
    }

    [UnityTest]
    public IEnumerator Invoke_On_Destroyed_Behaviour()
    {
        var holder = new GameObject();
        var dummyBehaviour = holder.AddComponent<DummyBehaviour>();

        SafeEvent sf;
        sf.Subscribe(dummyBehaviour.Dummy);

        Object.Destroy(dummyBehaviour);
        yield return null;

        SubscriptionIsNullException exception = null;
        try
        {
            sf.Invoke();
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
        var holder = new GameObject();
        var dummyBehaviour = holder.AddComponent<DummyBehaviour>();

        SafeEvent sf;
        sf.Subscribe(dummyBehaviour.Dummy);

        Object.Destroy(dummyBehaviour.gameObject);
        yield return null;

        SubscriptionIsNullException exception = null;
        try
        {
            sf.Invoke();
        }
        catch (SubscriptionIsNullException e)
        {
            exception = e;
        }

        Assert.IsNotNull(exception);
        Object.Destroy(holder);
    }

    [UnityTest]
    public IEnumerator Dispatch_Exposed_Action()
    {
        var holder = new GameObject();
        var dummyBehaviour = holder.AddComponent<DummyBehaviour>();
        yield return null;

        bool invoked = false;

        dummyBehaviour.OnExposedEvent += () =>
        {
            invoked = true;
        };

        Assert.IsFalse(invoked);
        dummyBehaviour.DispatchOnExposedEvent();
        Assert.IsTrue(invoked);

        Object.Destroy(holder);
    }
}
