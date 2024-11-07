using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

namespace Architecture.API.Events
{
    #region Dispatcher with input

    public static class EventDispatcher<TEventArgs>
    {
        private static Dictionary<string, Action<TEventArgs>> s_EventDictionary =
            new Dictionary<string, Action<TEventArgs>>();

        /// <summary>
        /// Subscribe to the event dispatcher dictionary by event name to be notified if specified event was raised
        /// </summary>
        /// <param name="name"> the name of the event listener is listening to </param>
        /// <param name="listener"> the event that will be called when event is raised </param>
        public static void Register(string name, Action<TEventArgs> listener)
        {
            Action<TEventArgs> thisEvent;
            if (s_EventDictionary.TryGetValue(name, out thisEvent))
            {
                thisEvent += listener;
                s_EventDictionary[name] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                s_EventDictionary.Add(name, thisEvent);
            }
        }

        /// <summary>
        /// Unsubscribe from listening to event specified by name
        /// </summary>
        /// <param name="name"> the name of the event to stop listening to </param>
        /// <param name="listener"> the event that removed from listening </param>
        public static void Unregister(string name, Action<TEventArgs> listener)
        {
            if (s_EventDictionary.ContainsKey(name))
            {
                s_EventDictionary[name] -= listener;
            }
        }

        /// <summary>
        /// Invokes all events listening to event name with argument corresponding to event
        /// </summary>
        /// <param name="name"> name of event signifying which events to notify </param>
        /// <param name="eventArgs"> argumnet to the events listening </param>
        public static void Raise(string name, TEventArgs eventArgs)
        {
            if (s_EventDictionary.ContainsKey(name))
            {
                s_EventDictionary[name]?.Invoke(eventArgs);
            }
        }

        /// <summary>
        /// Invokes all events listening to event name with argument corresponding to event asynchroniasly
        /// </summary>
        /// <param name="name"> name of event signifying which events to notify </param>
        /// <param name="eventArgs"> argumnet to the events listening </param>
        public static void RaiseAsync(string name, TEventArgs eventArgs)
        {
            if (s_EventDictionary.ContainsKey(name))
            {
                Task.Run(() =>
                {
                    s_EventDictionary[name]?.Invoke(eventArgs);
                });
            }
        }
    }

    #endregion

    #region Dispatcher no input

    public static class EventDispatcher
    {
        private static Dictionary<string, Action> s_EventDictionary =
            new Dictionary<string, Action>();

        /// <summary>
        /// Subscribe to the event dispatcher dictionary by event name to be notified if specified event was raised
        /// </summary>
        /// <param name="name"> the name of the event listener is listening to </param>
        /// <param name="listener"> the event that will be called when event is raised </param>
        public static void Register(string name, Action listener)
        {
            Action thisEvent;
            if (s_EventDictionary.TryGetValue(name, out thisEvent))
            {
                thisEvent += listener;
                s_EventDictionary[name] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                s_EventDictionary.Add(name, thisEvent);
            }
        }

        /// <summary>
        /// Unsubscribe from listening to event specified by name
        /// </summary>
        /// <param name="name"> the name of the event to stop listening to </param>
        /// <param name="listener"> the event that removed from listening </param>
        public static void Unregister(string name, Action listener)
        {
            if (s_EventDictionary.ContainsKey(name))
            {
                s_EventDictionary[name] -= listener;
            }
        }

        /// <summary>
        /// Invokes all events listening to event name with argument corresponding to event
        /// </summary>
        /// <param name="name"> name of event signifying which events to notify </param>
        public static void Raise(string name)
        {
            if (s_EventDictionary.ContainsKey(name))
            {
                s_EventDictionary[name]?.Invoke();
            }
        }

        /// <summary>
        /// Invokes all events listening to event name with argument corresponding to event asynchroniasly
        /// </summary>
        /// <param name="name"> name of event signifying which events to notify </param>
        public static void RaiseAsync(string name)
        {
            if (s_EventDictionary.ContainsKey(name))
            {
                Task.Run(() =>
                {
                    s_EventDictionary[name]?.Invoke();
                });
            }
        }
    }

    #endregion
}
