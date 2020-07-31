using System;
using System.Collections.Generic;
using UnityEngine;

namespace TNRD.Events
{
    public struct SafeEvent : IEquatable<SafeEvent>
    {
        private HashSet<Action> subscriptions;

        public int Count => subscriptions?.Count ?? 0;

        public void RemoveAllSubscriptions()
        {
            subscriptions.Clear();
        }

        public void Invoke()
        {
            foreach (var subscription in subscriptions)
            {
#if UNITY_EDITOR || DEBUG
                ThrowIfInvalidSubscription(subscription);
#endif
                subscription?.Invoke();
            }
        }

        private static void ThrowIfInvalidSubscription(Action subscription)
        {
            if (subscription?.Target == null)
            {
                throw new SubscriptionIsNullException();
            }

            if (subscription.Target is Component component && !component)
            {
                throw new SubscriptionIsNullException();
            }

            if (subscription.Target is GameObject gameObject && !gameObject)
            {
                throw new SubscriptionIsNullException();
            }
        }

        public void Subscribe(Action action)
        {
            if (subscriptions == null)
            {
                subscriptions = new HashSet<Action>();
            }

            if (subscriptions.Contains(action))
            {
#if UNITY_EDITOR || DEBUG
                throw new DuplicateSubscriptionException();
#endif
                return;
            }

            subscriptions.Add(action);
        }

        public void Unsubscribe(Action action)
        {
            subscriptions?.Remove(action);
        }

        public static SafeEvent operator +(SafeEvent safeEvent, Action action)
        {
            safeEvent.Subscribe(action);
            return safeEvent;
        }

        public static SafeEvent operator -(SafeEvent safeEvent, Action action)
        {
            safeEvent.Unsubscribe(action);
            return safeEvent;
        }

        public bool Equals(SafeEvent other)
        {
            return Equals(subscriptions, other.subscriptions);
        }

        public override bool Equals(object obj)
        {
            return obj is SafeEvent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (subscriptions != null ? subscriptions.GetHashCode() : 0);
        }
    }
}
