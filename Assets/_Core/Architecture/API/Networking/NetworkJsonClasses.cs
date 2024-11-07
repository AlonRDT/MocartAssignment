using NUnit.Framework;
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
    }
}
