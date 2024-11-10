using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.API.Networking
{
    public class NetworkJsonClasses
    {
        #region Product

        // products get response body
        [Serializable]
        public class ProductsData
        {
            [Serializable]
            public class ProductData
            {
                public string name;
                public string description;
                public float price;
            }
            public List<ProductData> products;
        }

        // products update request body
        [Serializable]
        public class ProductUpdateRequestData
        {
            public string name;
            public float price;

            public ProductUpdateRequestData(string name, float price)
            {
                this.name = name;
                this.price = price;
            }

            public string ToJson()
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
        }

        // assumed product response body, need to get design from backend team
        [Serializable]
        public class ProductUpdateResponseData
        {
            public bool success;

            public ProductUpdateResponseData(bool success)
            {
                this.success = success;
            }
        }

        #endregion
    }
}
