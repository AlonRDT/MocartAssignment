using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Architecture.API.Events
{
    #region callback only out

    public static class EventCallback<T>
    {
        public delegate T Callback();

        private static Dictionary<string, Callback> eventDictionary =
            new Dictionary<string, Callback>();


        /// <summary>
        /// Subscribe to the event dispatcher dictionary by event name to be notified if specified event was raised
        /// </summary>
        /// <param name="name"> the name of the event listener is listening to </param>
        /// <param name="listener"> the event that will be called when event is raised </param>
        public static void Register(string name, Callback listener)
        {
            Callback thisEvent;
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
        /// Unsubscribe from listening to event specified by name
        /// </summary>
        /// <param name="name"> the name of the event to stop listening to </param>
        /// <param name="listener"> the event that removed from listening </param>
        public static void Unregister(string name, Callback listener)
        {
            if (eventDictionary.ContainsKey(name))
                eventDictionary[name] -= listener;
        }

        /// <summary>
        /// call the methods assosiated with given event name and pass their output into given action
        /// </summary>
        /// <param name="name"> name by which to find target methods </param>
        /// <param name="action"> the callback to which methods pass their output </param>
        public static void Raise(string name, Action<T> action)
        {
            if (eventDictionary.ContainsKey(name))
                action?.Invoke(eventDictionary[name].Invoke());
        }
    }

    #endregion

    #region Callback in + out

    public static class EventCallback<Tin, Tout>
    {
        public delegate Tout Callback(Tin param);

        private static Dictionary<string, Callback> eventDictionary =
            new Dictionary<string, Callback>();


        /// <summary>
        /// Subscribe to the event dispatcher dictionary by event name to be notified if specified event was raised
        /// </summary>
        /// <param name="name"> the name of the event listener is listening to </param>
        /// <param name="listener"> the event that will be called when event is raised </param>
        public static void Register(string name, Callback listener)
        {
            Callback thisEvent;
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
        /// Unsubscribe from listening to event specified by name
        /// </summary>
        /// <param name="name"> the name of the event to stop listening to </param>
        /// <param name="listener"> the event that removed from listening </param>
        public static void Unregister(string name, Callback listener)
        {
            if (eventDictionary.ContainsKey(name))
                eventDictionary[name] -= listener;
        }

        /// <summary>
        /// call the methods assosiated with given event name and pass their output into given action
        /// </summary>
        /// <param name="name"> name by which to find target methods </param>
        /// <param name="param"> the argument to give target methods </param>
        /// <param name="action"> the callback to which methods pass their output </param>
        public static void Raise(string name, Tin param, Action<Tout> action)
        {
            if (eventDictionary.ContainsKey(name))
                action?.Invoke(eventDictionary[name].Invoke(param));
        }
    }

    #endregion
}
