using System;
using System.Collections.Generic;
using UnityEngine;

namespace TNRD.Events
{
    public struct SafeEvent
    {
        private HashSet<Action> subscriptions;

        public int Count => subscriptions?.Count ?? 0;

        public void RemoveAllSubscriptions()
        {
            subscriptions?.Clear();
        }

        public void Invoke()
        {
            if (subscriptions == null)
                return;

            HashSet<Action> subscriptionsCopy = new HashSet<Action>(subscriptions);

            foreach (Action subscription in subscriptionsCopy)
            {
#if UNITY_EDITOR || DEBUG
                ThrowIfInvalidSubscription(subscription);
#endif
                subscription?.Invoke();
            }
        }

        private static void ThrowIfInvalidSubscription(Action subscription)
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
#pragma warning disable 162
                return;
#pragma warning restore 162
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
    }
}
