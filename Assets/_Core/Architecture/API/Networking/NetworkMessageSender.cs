using Architecture.API.Events;
using Architecture.API.Managers.Program;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.API.Edit;
using UnityEngine;
using static Architecture.API.Networking.NetworkJsonClasses;

namespace Architecture.API.Networking
{
    public static class NetworkMessageSender
    {
        private static string s_ServerURI = "https://homework.mocart.io/api/products";

        private static HttpRequestsTool s_RequestsTool = new HttpRequestsTool();

        /// <summary>
        /// send server request for products to display and returns results through event OnProductsReceived
        /// </summary>
        public static void GetProducts()
        {
            MainThreadEventDispatcher.Instance().Enqueue(s_RequestsTool.SendGetRequest<ProductsData>(s_ServerURI, ProgramEvents.OnProductsReceived.ToString(), ""));
        }

        public static void UpdateProduct(string name, float price)
        {
            string endPoint = "/" + name; // tried to find the uri for this action and failed, need to ask backend team

            /*
            MainThreadEventDispatcher.Instance().Enqueue(s_RequestsTool.SendPutRequest<ProductUpdateResponseData>(s_ServerURI + endPoint,
                new ProductUpdateRequestData(name, price).ToJson(),
                EditEvents.EditGotResponse.ToString(), EditEvents.EditFail.ToString()));
            */

            EventDispatcher<ProductUpdateResponseData>.Raise(EditEvents.EditGotResponse.ToString(), new ProductUpdateResponseData(true));
        }
    }
}