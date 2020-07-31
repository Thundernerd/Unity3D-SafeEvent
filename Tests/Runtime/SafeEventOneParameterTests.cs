using System.Collections;
using NUnit.Framework;
using TNRD.Events;
using UnityEngine;
using UnityEngine.TestTools;

public class SafeEventOneParameterTests
{
    [Test]
    public void Invoke_On_Behaviour()
    {
        var holder = new GameObject();
        var dummyBehaviour = holder.AddComponent<DummyBehaviour>();

        SafeEvent<int> sf;
        sf.Subscribe(dummyBehaviour.Dummy);

        Assert.AreEqual(0, dummyBehaviour.Obj);
        sf.Invoke(1);
        Assert.AreEqual(1, dummyBehaviour.Obj);

        Object.Destroy(holder);
    }

    [UnityTest]
    public IEnumerator Invoke_On_Destroyed_Behaviour()
    {
        var holder = new GameObject();
        var dummyBehaviour = holder.AddComponent<DummyBehaviour>();

        SafeEvent<int> sf;
        sf.Subscribe(dummyBehaviour.Dummy);

        Object.Destroy(dummyBehaviour);
        yield return null;

        SubscriptionIsNullException exception = null;
        try
        {
            sf.Invoke(1);
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

        SafeEvent<int> sf;
        sf.Subscribe(dummyBehaviour.Dummy);

        Object.Destroy(dummyBehaviour.gameObject);
        yield return null;

        SubscriptionIsNullException exception = null;
        try
        {
            sf.Invoke(1);
        }
        catch (SubscriptionIsNullException e)
        {
            exception = e;
        }

        Assert.IsNotNull(exception);
        Object.Destroy(holder);
    }
}
