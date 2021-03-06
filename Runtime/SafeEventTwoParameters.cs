﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace TNRD.Events
{
    public struct SafeEvent<T, T2>
    {
        private HashSet<Action<T, T2>> subscriptions;

        public int Count => subscriptions?.Count ?? 0;

        public void RemoveAllSubscriptions()
        {
            subscriptions?.Clear();
        }

        public void Invoke(T arg1, T2 arg2)
        {
            if (subscriptions == null)
                return;

            HashSet<Action<T, T2>> subscriptionsCopy = new HashSet<Action<T, T2>>(subscriptions);

            foreach (Action<T, T2> subscription in subscriptionsCopy)
            {
#if UNITY_EDITOR || DEBUG
                ThrowIfInvalidSubscription(subscription);
#endif
                subscription?.Invoke(arg1, arg2);
            }
        }

        private static void ThrowIfInvalidSubscription(Action<T, T2> subscription)
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

        public void Subscribe(Action<T, T2> action)
        {
            if (subscriptions == null)
            {
                subscriptions = new HashSet<Action<T, T2>>();
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

        public void Unsubscribe(Action<T, T2> action)
        {
            subscriptions?.Remove(action);
        }

        public static SafeEvent<T, T2> operator +(SafeEvent<T, T2> safeEvent, Action<T, T2> action)
        {
            safeEvent.Subscribe(action);
            return safeEvent;
        }

        public static SafeEvent<T, T2> operator -(SafeEvent<T, T2> safeEvent, Action<T, T2> action)
        {
            safeEvent.Unsubscribe(action);
            return safeEvent;
        }
    }
}
