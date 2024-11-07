using System.Linq;
using UnityEngine;
using static Architecture.API.Networking.NetworkJsonClasses;

namespace Environment.API.Shelf
{
    public class NoteLogic : MonoBehaviour
    {
        #region Variables


        [SerializeField] private GameObject m_Model;
        [SerializeField] private TextMesh m_NameText;
        [SerializeField] private TextMesh m_DescriptionText;
        [SerializeField] private TextMesh m_PriceText;

        private bool m_IsActive = false;
        public bool IsActive => m_IsActive;

        #endregion

        void Awake()
        {
            m_Model.SetActive(false);    
        }

        public void ShowNote(ProductsData.ProductData data)
        {
            m_IsActive = true;
            m_Model.SetActive(true);
            m_NameText.text = data.name;
            m_DescriptionText.text = string.Join('\n', data.description.Split(' '));
            m_PriceText.text = "Price: " + data.price.ToString("F2");
        }
    }
}
