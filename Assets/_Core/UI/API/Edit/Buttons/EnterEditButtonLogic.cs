using Architecture.API.Events;
using Architecture.API.Managers;
using UnityEngine;
using static Architecture.API.Managers.Program.ProgramManager;
using static Architecture.API.Networking.NetworkJsonClasses;

namespace UI.API.Edit.Buttons
{
    public class EnterEditButtonLogic : MonoBehaviour
    {
        #region Variables

        private RectTransform m_RectTransform; // Reference to the UI element's RectTransform
        [SerializeField] private float m_SizePercentageOfScreenHeight = 10f; // Percentage of screen height for both width and height

        private ProductRefinedData m_Data;

        #endregion

        #region Ctor/Dtor

        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
        }

        void OnDestroy()
        {
            ScreenManager.OnScreenSizeChangeSecond -= setButtonSizeAndPosition;
        }

        #endregion

        #region Initialization

        /// <summary>
        /// initilize button, shows in correct place and hold data
        /// </summary>
        /// <param name="index"> button index correlates to note index </param>
        /// <param name="data"> data of product of index </param>
        public void Init(ProductRefinedData data)
        {
            gameObject.SetActive(true);
            m_Data = data;
            setButtonSizeAndPosition();
            ScreenManager.OnScreenSizeChangeSecond += setButtonSizeAndPosition;
        }

        /// <summary>
        /// Method responsoble to keep button right size even if screen size changes
        /// </summary>
        private void setButtonSizeAndPosition()
        {
            EventCallback<int, Vector3>.Raise(EditEvents.GetButtonPosition.ToString(), m_Data.Index, setPosition);

            // Get the screen height
            float screenHeight = Screen.height;

            // Calculate the target height based on the screen height percentage
            float targetHeight = screenHeight * (m_SizePercentageOfScreenHeight / 100f);

            // Calculate the scale factor needed to fit the target height while keeping the aspect ratio
            float scaleFactor = targetHeight / m_RectTransform.sizeDelta.y;

            if(scaleFactor * m_RectTransform.sizeDelta.x > Screen.width / 6)
            {
                scaleFactor = (Screen.width / 6) / m_RectTransform.sizeDelta.x;
            }

            m_RectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        }

        /// <summary>
        /// set ui position on screen by world position
        /// </summary>
        /// <param name="position"> world position by which to place ui </param>
        private void setPosition(Vector3 position)
        {
            ScreenManager.SetPositonByWorldPos(m_RectTransform, position - new Vector3(0, 0.25f, 0));
        }

        #endregion

        /// <summary>
        /// When Clicking button notifies program to enter edit mode of target product
        /// </summary>
        public void OnClick()
        {
            EventDispatcher<ProductRefinedData>.Raise(EditEvents.EditProduct.ToString(), m_Data);
        }
    }
}
