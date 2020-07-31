using NUnit.Framework;
using TNRD.Events;

public class SafeEventTwoParametersTests
{
    private void Dummy(int arg1, int arg2)
    {
    }

    [Test]
    public void Add_Subscription()
    {
        SafeEvent<int, int> sf;

        Assert.AreEqual(0, sf.Count);
        sf.Subscribe(Dummy);
        Assert.AreEqual(1, sf.Count);
    }

    [Test]
    public void Add_Subscription_Alt()
    {
        SafeEvent<int, int> sf;

        Assert.AreEqual(0, sf.Count);
        sf += Dummy;
        Assert.AreEqual(1, sf.Count);
    }

    [Test]
    public void Remove_Subscription()
    {
        SafeEvent<int, int> sf;
        sf.Subscribe(Dummy);

        Assert.AreEqual(1, sf.Count);
        sf.Unsubscribe(Dummy);
        Assert.AreEqual(0, sf.Count);
    }

    [Test]
    public void Remove_Subscription_Alt()
    {
        SafeEvent<int, int> sf;
        sf += Dummy;

        Assert.AreEqual(1, sf.Count);
        sf -= Dummy;
        Assert.AreEqual(0, sf.Count);
    }

    [Test]
    public void Clear_Subscriptions()
    {
        SafeEvent<int, int> sf;
        sf.Subscribe(Dummy);

        Assert.AreEqual(1, sf.Count);
        sf.RemoveAllSubscriptions();
        Assert.AreEqual(0, sf.Count);
    }

    [Test]
    public void Double_Subscription_Exception()
    {
        SafeEvent<int, int> sf;
        sf.Subscribe(Dummy);

        DuplicateSubscriptionException ex = null;

        try
        {
            sf.Subscribe(Dummy);
        }
        catch (DuplicateSubscriptionException exception)
        {
            ex = exception;
        }

        Assert.IsNotNull(ex);
    }

    [Test]
    public void Normal_Invocation()
    {
        SafeEvent<int, int> sf;

        int arg1 = 0, arg2 = 0;

        sf.Subscribe((x, y) =>
        {
            arg1 = x;
            arg2 = y;
        });

        Assert.AreEqual(0, arg1);
        Assert.AreEqual(0, arg2);

        sf.Invoke(1, 2);

        Assert.AreEqual(1, arg1);
        Assert.AreEqual(2, arg2);
    }
}
