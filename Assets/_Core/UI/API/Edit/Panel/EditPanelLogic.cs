using Architecture.API.Events;
using Architecture.API.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Architecture.API.Managers.Program.ProgramManager;

namespace UI.API.Edit.Panel
{
    public class EditPanelLogic : MonoBehaviour
    {
        #region Variables

        public enum TargetPosition
        {
            Above,
            Center
        }

        [Header("General Components")]
        [SerializeField] private RectTransform m_RectTransform; // Reference to the UI element's RectTransform

        [Header("Edit Components")]
        [SerializeField] private TMP_InputField m_NameInputField;
        [SerializeField] private TMP_InputField m_PriceInputField;
        [SerializeField] private Button m_SaveButton;
        [SerializeField] private Button m_CancelButton;
        [SerializeField] private float m_SizePercentageOfScreenHeight = 10f; // Percentage of screen height for both width and height
        
        [Header("Result Components")]
        [SerializeField] private TMP_Text m_ResultText;
        [SerializeField] private Button m_RetryButton;
        [SerializeField] private Button m_BackButton;

        private Vector2 m_OriginalSize; // Store the original size of the RectTransform
        private ProductRefinedData m_CurrentData;
        private Sequence m_Sequence;

        #endregion

        private void Awake()
        {
            m_OriginalSize = m_RectTransform.sizeDelta;
            setPanelSize();
            ScreenManager.OnScreenSizeChange += setPanelSize;
            MovePanel(TargetPosition.Above, true);
        }

        public void EnterEditMode(ProductRefinedData data)
        {
            gameObject.SetActive(true);
            m_CurrentData = data;
            m_NameInputField.text = data.Name;
            m_PriceInputField.text = data.Price.ToString("F2");
            MovePanel(TargetPosition.Center);
        }

        private void setPanelSize()
        {
            // Get the screen height
            float screenHeight = Screen.height;

            // Calculate the target height based on the screen height percentage
            float targetHeight = screenHeight * (m_SizePercentageOfScreenHeight / 100f);

            // Calculate the scale factor needed to fit the target height while keeping the aspect ratio
            float scaleFactor = targetHeight / m_OriginalSize.y;

            m_RectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        }

        #region Movement

        public void MovePanel(TargetPosition targetPosition, bool immidiate = false)
        {
            m_CancelButton.interactable = false;
            m_SaveButton.interactable = false;

            Vector3 endPosition = Vector3.zero;
            if (targetPosition == TargetPosition.Above)
            {
                endPosition = new Vector3(0, 1, 0) * (Screen.height / 2 + m_RectTransform.sizeDelta.y * m_RectTransform.localScale.y);
            }

            if (immidiate == true)
            {
                m_RectTransform.anchoredPosition = endPosition;
            }
            else
            {
                m_Sequence = DOTween.Sequence();

                m_Sequence.Append(m_RectTransform.DOAnchorPosY(endPosition.y, 2)).OnComplete(() => onEndMovement(targetPosition));
            }
        }

        private void onEndMovement(TargetPosition targetPosition)
        {
            if(targetPosition == TargetPosition.Center)
            {
                m_CancelButton.interactable = true;
                m_SaveButton.interactable = true;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        #endregion

        #region Button Methods

        public void OnClickSave()
        {
            m_CurrentData.Name = m_NameInputField.text;

            if(float.TryParse(m_PriceInputField.text, out m_CurrentData.Price) == false)
            {
                Debug.LogError("Couldnt parse new price from string, make sure price input field content type is decimal number");
            }

            EventDispatcher<int>.Raise(EditEvents.RefreshProductData.ToString(), m_CurrentData.Index);
            MovePanel(TargetPosition.Above);
        }

        public void OnClickCancel()
        {
            MovePanel(TargetPosition.Above);
        }

        #endregion
    }
}
