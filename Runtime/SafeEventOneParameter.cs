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

            HashSet<Action<T>> subscriptionsCopy = new HashSet<Action<T>>(subscriptions);

            foreach (Action<T> subscription in subscriptionsCopy)
            {
#if UNITY_EDITOR || DEBUG
                ThrowIfInvalidSubscription(subscription);
#endif
                subscription?.Invoke(obj);
            }
        }

        private static void ThrowIfInvalidSubscription(Action<T> subscription)
        {
            if (subscription == null)
            {
                throw new SubscriptionIsNullException();
            }

            switch (subscription.Target)
            {
                case null when !subscription.Method.IsStatic:
                    throw new SubscriptionIsNullException();
                case Component component when !component:
                    throw new SubscriptionIsNullException();
                case GameObject gameObject when !gameObject:
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
#pragma warning disable 162
                return;
#pragma warning restore 162
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
