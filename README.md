# Safe Event

<p align="center">
	<img alt="GitHub package.json version" src ="https://img.shields.io/github/package-json/v/Thundernerd/Unity3D-SafeEvent" />
	<a href="https://github.com/Thundernerd/Unity3D-SafeEvent/issues">
		<img alt="GitHub issues" src ="https://img.shields.io/github/issues/Thundernerd/Unity3D-SafeEvent" />
	</a>
	<a href="https://github.com/Thundernerd/Unity3D-SafeEvent/pulls">
		<img alt="GitHub pull requests" src ="https://img.shields.io/github/issues-pr/Thundernerd/Unity3D-SafeEvent" />
	</a>
	<a href="https://github.com/Thundernerd/Unity3D-SafeEvent/blob/master/LICENSE.md">
		<img alt="GitHub license" src ="https://img.shields.io/github/license/Thundernerd/Unity3D-SafeEvent" />
	</a>
	<img alt="GitHub last commit" src ="https://img.shields.io/github/last-commit/Thundernerd/Unity3D-SafeEvent" />
</p>

An event class that has extra checks to help prevent mistakes. 

It will notify you when you've subscribed more than once with the same callback. In case your callback lies within a MonoBehaviour it will also detect if that behaviour and/or the GameObject has been destroyed and will let you know if you forgot to unsubscribe from the event.  

## Installation
1. The package is available on the [openupm registry](https://openupm.com). You can install it via [openupm-cli](https://github.com/openupm/openupm-cli).
```
openupm add net.tnrd.safeevent
```

2. Installing through a [Unity Package](http://package-installer.glitch.me/v1/installer/package.openupm.com/net.tnrd.safeevent?registry=https://package.openupm.com) created by the [Package Installer Creator](https://package-installer.glitch.me) from [Needle](https://needle.tools)

[<img src="https://img.shields.io/badge/-Download-success?style=for-the-badge"/>](http://package-installer.glitch.me/v1/installer/package.openupm.com/net.tnrd.safeevent?registry=https://package.openupm.com)

## Usage

There are three versions of Safe Event that you can use. One without any parameters, one with a single parameter, and one with two parameters.

### Definition

The definition is the same as you would define an Action

```c#
private SafeEvent onFooEvent;
```

```c#
private SafeEvent<int> onFooEvent;
```

```c#
private SafeEvent<int, string> onFooEvent;
```

### Subscriptions

Subscribing and unsubscribing can be done in two ways.

One way is through the Subscribe and Unsubscribe methods

```c#
private void SubscribeToEvent()
{
    onFooEvent.Subscribe(ExecuteOnEvent);
}

private void ExecuteOnEvent()
{
    [...]
}

private void UnsubscribeFromEvent()
{
    onFooEvent.Unsubscribe(ExecuteOnEvent);
}
```

And the other is through the use of operators

```c#
private void SubscribeToEvent()
{
    onFooEvent += ExecuteOnEvent;
}

private void ExecuteOnEvent()
{
    [...]
}

private void UnsubscribeFromEvent()
{
    onFooEvent -= ExecuteOnEvent;
}
```

### Invocation

Invoking the event is the same as you would invoke a regular event

```c#
private SafeEvent onFooEvent;

[...]

private void InvokeEvent()
{
    onFooEvent.Invoke();
}
```

```c#
private SafeEvent<int> onFooEvent;

[...]

private void InvokeEvent()
{
    onFooEvent.Invoke(123);
}
```

```c#
private SafeEvent<int, string> onFooEvent;

[...]

private void InvokeEvent()
{
    onFooEvent.Invoke(123, "abcde");
}
```

### Exposing

Now that you've set up your Safe Event you most likely want to be able to use it from a different place.

The recommended way of doing this is by exposing an event property as shown below, instead of directly exposing the Safe Event

```c#
private SafeEvent<int> onFooEvent;

public event Action<int> OnFooEvent
{
    add => onFooEvent.Subscribe(value);
    remove => onFooEvent.Unsubscribe(value);
}
```

The benefit of this is that to the outside it looks and behaves like a normal event even though under the hood it has the benefits of the Safe Event.

## Support
**Safe Event** is a small and open-source utility that I hope helps other people. It is by no means necessary but if you feel generous you can support me by donating.

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/J3J11GEYY)

## Contributions
Pull requests are welcomed. Please feel free to fix any issues you find, or add new features.

