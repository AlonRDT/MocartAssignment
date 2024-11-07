using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Architecture.API.Managers
{
    public class ManagersSpawner : MonoBehaviour
    {
        /// <summary>
        /// spawns all manager inherited objects
        /// </summary>
        void Awake()
        {
            foreach (Type type in
                Assembly.GetAssembly(typeof(Manager)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Manager))))
            {
                GameObject newManager = new GameObject(type.Name);
                newManager.transform.SetParent(transform);
                newManager.AddComponent(type);
            }
        }
    }
}
