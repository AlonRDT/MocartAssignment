using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Architecture.API.Events
{
    #region Async Callback in and out

    public static class EventCallbackAsync<TParam, TOut>
    {
        public delegate Task<R> AsyncCallback<T, R>(T param);

        private static Dictionary<string, AsyncCallback<TParam, TOut>> eventDictionary =
            new Dictionary<string, AsyncCallback<TParam, TOut>>();

        /// <summary>
        /// Subscribe to an event by name, event will get argument and return output generically and async
        /// </summary>
        /// <param name="name"> name by wich to notify event </param>
        /// <param name="listener"> the delegate to call when event is notified </param>
        public static void Register(string name, AsyncCallback<TParam, TOut> listener)
        {
            AsyncCallback<TParam, TOut> thisEvent;
            if (eventDictionary.TryGetValue(name, out thisEvent))
            {
                thisEvent += listener;
                eventDictionary[name] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                eventDictionary.Add(name, thisEvent);
            }
        }

        /// <summary>
        /// Unsubscribe from an event by name
        /// </summary>
        /// <param name="name"> name to dissasociate event from </param>
        /// <param name="listener"> the callback that will be disassosiated </param>
        public static void Unregister(string name, AsyncCallback<TParam, TOut> listener)
        {
            if (eventDictionary.ContainsKey(name))
                eventDictionary[name] -= listener;
        }

        /// <summary>
        /// call the methods assosiated with given event name and pass their output into given action async
        /// </summary>
        /// <param name="name"> name by which to find target methods </param>
        /// <param name="param"> the argument to give target methods </param>
        /// <param name="action"> the callback to which methods pass their output </param>
        public static void Raise(string name, TParam param, Action<TOut> action)
        {
            if (eventDictionary.ContainsKey(name))
            {
                Task.Run(async () =>
                {
                    action?.Invoke(await eventDictionary[name](param));
                });
            }
        }
    }

    #endregion

    #region Async Callback only out

    public static class EventCallbackAsync<TOut>
    {
        public delegate Task<T> AsyncCallback<T>();

        private static Dictionary<string, AsyncCallback<TOut>> eventDictionary =
            new Dictionary<string, AsyncCallback<TOut>>();


        /// <summary>
        /// Subscribe to an event by name, event will get argument and return output generically and async
        /// </summary>
        /// <param name="name"> name by wich to notify event </param>
        /// <param name="listener"> the delegate to call when event is notified </param>
        public static void Register(string name, AsyncCallback<TOut> listener)
        {
            AsyncCallback<TOut> thisEvent;
            if (eventDictionary.TryGetValue(name, out thisEvent))
            {
                thisEvent += listener;
                eventDictionary[name] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                eventDictionary.Add(name, thisEvent);
            }
        }

        /// <summary>
        /// Unsubscribe from an event by name
        /// </summary>
        /// <param name="name"> name to dissasociate event from </param>
        /// <param name="listener"> the callback that will be disassosiated </param>
        public static void Unregister(string name, AsyncCallback<TOut> listener)
        {
            if (eventDictionary.ContainsKey(name))
                eventDictionary[name] -= listener;
        }

        /// <summary>
        /// call the methods assosiated with given event name and pass their output into given action async
        /// </summary>
        /// <param name="name"> name by which to find target methods </param>
        /// <param name="action"> the callback to which methods pass their output </param>
        public static void Raise(string name, Action<TOut> action)
        {
            if (eventDictionary.ContainsKey(name))
            {
                Task.Run(async () =>
                {
                    action?.Invoke(await eventDictionary[name]());
                });
            }
        }
    }

    #endregion
}
