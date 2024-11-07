using Architecture.API.Events;
using System.Threading.Tasks;
using UnityEngine;

namespace Architecture.API.Networking
{
    public static class NetworkMessageSender
    {
        private static string s_ServerURI = "https://homework.mocart.io/api/products";

        private static HttpRequestsTool s_RequestsTool = new HttpRequestsTool();

        public static void GetProducts()
        {
            MainThreadEventDispatcher.Instance().Enqueue(s_RequestsTool.SendGetRequest(s_ServerURI, ""));
        }
    }
}