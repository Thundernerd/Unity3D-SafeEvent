using NUnit.Framework;
using TNRD.Events;

public class SafeEventTests
{
    private void Dummy()
    {
        // Left blank on purpose
    }

    [Test]
    public void Add_Subscription()
    {
        SafeEvent sf;

        Assert.AreEqual(0, sf.Count);
        sf.Subscribe(Dummy);
        Assert.AreEqual(1, sf.Count);
    }

    [Test]
    public void Add_Subscription_Alt()
    {
        SafeEvent sf;

        Assert.AreEqual(0, sf.Count);
        sf += Dummy;
        Assert.AreEqual(1, sf.Count);
    }

    [Test]
    public void Remove_Subscription()
    {
        SafeEvent sf;
        sf.Subscribe(Dummy);

        Assert.AreEqual(1, sf.Count);
        sf.Unsubscribe(Dummy);
        Assert.AreEqual(0, sf.Count);
    }

    [Test]
    public void Remove_Subscription_Alt()
    {
        SafeEvent sf;
        sf += Dummy;

        Assert.AreEqual(1, sf.Count);
        sf -= Dummy;
        Assert.AreEqual(0, sf.Count);
    }

    [Test]
    public void Clear_Subscriptions()
    {
        SafeEvent sf;
        sf.Subscribe(Dummy);

        Assert.AreEqual(1, sf.Count);
        sf.RemoveAllSubscriptions();
        Assert.AreEqual(0, sf.Count);
    }

    [Test]
    public void Double_Subscription_Exception()
    {
        SafeEvent sf;
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
        SafeEvent sf;

        bool invoked = false;

        sf.Subscribe(() => { invoked = true; });
        sf.Invoke();

        Assert.IsTrue(invoked);
    }
}
