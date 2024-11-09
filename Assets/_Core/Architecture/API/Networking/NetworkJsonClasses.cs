using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.API.Networking
{
    public class NetworkJsonClasses
    {
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

        [Serializable]
        public class ProductUpdateResponseData
        {
            public bool success;

            public ProductUpdateResponseData(bool success)
            {
                this.success = success;
            }
        }
    }
}
