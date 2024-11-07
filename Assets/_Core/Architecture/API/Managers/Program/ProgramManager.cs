using Architecture.API.Events;
using Architecture.API.Networking;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Architecture.API.Networking.NetworkJsonClasses;

namespace Architecture.API.Managers.Program
{
    public class ProgramManager : Manager
    {
        void Awake()
        {
            EventDispatcher<ProductsData>.Register(ProgramEvents.OnProductsReceived.ToString(), onProductsReceived);
        }

        void Start()
        {
            NetworkMessageSender.GetProducts();
        }

        /// <summary>
        /// displays products from server on the shelf
        /// </summary>
        /// <param name="products"> products data received from server </param>
        private void onProductsReceived(ProductsData products)
        {
            foreach (var product in products.products)
            {
                Debug.Log($"name: {product.name}, desc: {product.description}, price: {product.price}");
            }
        }
    }
}