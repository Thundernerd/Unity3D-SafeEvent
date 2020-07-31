using NUnit.Framework;
using TNRD.Events;

public class SafeEventOneParameterTests
{
    private void Dummy(int arg)
    {
        // Left blank on purpose
    }

    [Test]
    public void Add_Subscription()
    {
        SafeEvent<int> sf;

        Assert.AreEqual(0, sf.Count);
        sf.Subscribe(Dummy);
        Assert.AreEqual(1, sf.Count);
    }

    [Test]
    public void Add_Subscription_Alt()
    {
        SafeEvent<int> sf;

        Assert.AreEqual(0, sf.Count);
        sf += Dummy;
        Assert.AreEqual(1, sf.Count);
    }

    [Test]
    public void Remove_Subscription()
    {
        SafeEvent<int> sf;
        sf.Subscribe(Dummy);

        Assert.AreEqual(1, sf.Count);
        sf.Unsubscribe(Dummy);
        Assert.AreEqual(0, sf.Count);
    }

    [Test]
    public void Remove_Subscription_Alt()
    {
        SafeEvent<int> sf;
        sf += Dummy;

        Assert.AreEqual(1, sf.Count);
        sf -= Dummy;
        Assert.AreEqual(0, sf.Count);
    }

    [Test]
    public void Clear_Subscriptions()
    {
        SafeEvent<int> sf;
        sf.Subscribe(Dummy);

        Assert.AreEqual(1, sf.Count);
        sf.RemoveAllSubscriptions();
        Assert.AreEqual(0, sf.Count);
    }

    [Test]
    public void Double_Subscription_Exception()
    {
        SafeEvent<int> sf;
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
        SafeEvent<int> sf;

        int result = 0;

        sf.Subscribe(x => { result = x; });
        Assert.AreEqual(0, result);
        sf.Invoke(1);
        Assert.AreEqual(1, result);
    }
}
