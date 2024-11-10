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
        #region Refined Data Classes

        public class ProductRefinedData
        {
            public int Index;
            public string Name;
            public string Description;
            public float Price;

            public ProductRefinedData(int index, ProductsData.ProductData data)
            {
                Index = index;
                Name = data.name; 
                Description = data.description; 
                Price = data.price;
            }
        }

        #endregion

        #region Ctor/Dtor

        void Awake()
        {
            EventDispatcher<ProductsData>.Register(ProgramEvents.OnProductsReceived.ToString(), onProductsReceived);
        }

        void OnDestroy()
        {
            EventDispatcher<ProductsData>.Unregister(ProgramEvents.OnProductsReceived.ToString(), onProductsReceived);
        }

        #endregion

        #region Product Management

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
            for (int i = 0; i < products.products.Count; i++)
            {
                EventDispatcher<ProductRefinedData>.Raise(ProgramEvents.ShowNote.ToString(),
                    new ProductRefinedData(i, products.products[i]));
            }
        }

        #endregion
    }
}