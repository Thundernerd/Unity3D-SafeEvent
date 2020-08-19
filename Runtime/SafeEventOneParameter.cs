using System;
using System.Collections.Generic;
using UnityEngine;

namespace TNRD.Events
{
    public struct SafeEvent<T>
    {
        private HashSet<Action<T>> subscriptions;

        public int Count => subscriptions?.Count ?? 0;

        public void RemoveAllSubscriptions()
        {
            subscriptions?.Clear();
        }

        public void Invoke(T obj)
        {
            if (subscriptions == null)
                return;

            foreach (var subscription in subscriptions)
            {
#if UNITY_EDITOR || DEBUG
                ThrowIfInvalidSubscription(subscription);
#endif
                subscription?.Invoke(obj);
            }
        }

        private static void ThrowIfInvalidSubscription(Action<T> subscription)
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

        public void Subscribe(Action<T> action)
        {
            if (subscriptions == null)
            {
                subscriptions = new HashSet<Action<T>>();
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

        public void Unsubscribe(Action<T> action)
        {
            subscriptions?.Remove(action);
        }

        public static SafeEvent<T> operator +(SafeEvent<T> safeEvent, Action<T> action)
        {
            safeEvent.Subscribe(action);
            return safeEvent;
        }

        public static SafeEvent<T> operator -(SafeEvent<T> safeEvent, Action<T> action)
        {
            safeEvent.Unsubscribe(action);
            return safeEvent;
        }
    }
}
