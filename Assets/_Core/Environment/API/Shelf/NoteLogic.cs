using System.Linq;
using UnityEngine;
using static Architecture.API.Managers.Program.ProgramManager;
using static Architecture.API.Networking.NetworkJsonClasses;
using static UnityEngine.Analytics.IAnalytic;

namespace Environment.API.Shelf
{
    public class NoteLogic : MonoBehaviour
    {
        #region Variables

        [SerializeField] private GameObject m_Model;
        [SerializeField] private TextMesh m_NameText;
        [SerializeField] private TextMesh m_DescriptionText;
        [SerializeField] private TextMesh m_PriceText;

        private ProductRefinedData m_Data;

        #endregion

        #region Ctor/Dtor

        void Awake()
        {
            m_Model.SetActive(false);    
        }

        #endregion

        #region Data Presentation

        /// <summary>
        /// make note visible in scene and display product data received from server
        /// </summary>
        /// <param name="data"> product data to display </param>
        public void ShowNote(ProductRefinedData data)
        {
            m_Model.SetActive(true);
            m_Data = data;
            RefreshData();
        }

        /// <summary>
        /// show most current data saved in the data object
        /// </summary>
        public void RefreshData()
        {
            m_NameText.text = m_Data.Name;
            m_DescriptionText.text = string.Join('\n', m_Data.Description.Split(' '));
            m_PriceText.text = "Price: " + m_Data.Price.ToString("F2");
        }

        #endregion
    }
}
