using Architecture.API.Events;
using Architecture.API.Managers;
using Architecture.API.Networking;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Architecture.API.Managers.Program.ProgramManager;
using static Architecture.API.Networking.NetworkJsonClasses;

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
        [SerializeField] private GameObject m_EditParent;
        [SerializeField] private TMP_InputField m_NameInputField;
        [SerializeField] private TMP_InputField m_PriceInputField;
        [SerializeField] private Button m_SaveButton;
        [SerializeField] private Button m_CancelButton;
        [SerializeField] private float m_SizePercentageOfScreenHeight = 10f; // Percentage of screen height for both width and height

        [Header("Result Components")]
        [SerializeField] private GameObject m_ResultParent;
        [SerializeField] private TMP_Text m_ResultText;
        [SerializeField] private Button m_RetryButton;
        [SerializeField] private Button m_BackButton;

        private ProductRefinedData m_CurrentData;
        private Sequence m_Sequence;

        #endregion

        #region Ctor/Dtor

        private void Awake()
        {
            EventDispatcher<ProductUpdateResponseData>.Register(EditEvents.EditGotResponse.ToString(), gotEditResponse);
            EventDispatcher.Register(EditEvents.EditFail.ToString(), editRequestFailed);

            setPanelSize();
            ScreenManager.OnScreenSizeChange += setPanelSize;
            MovePanel(TargetPosition.Above, true);
        }

        void OnDestroy()
        {
            EventDispatcher<ProductUpdateResponseData>.Unregister(EditEvents.EditGotResponse.ToString(), gotEditResponse);
            EventDispatcher.Unregister(EditEvents.EditFail.ToString(), editRequestFailed);
        }

        private void setPanelSize()
        {
            float widthMinimum = Screen.width / 1.2f; // make sure that if height is bigger than width than size has a minimum based on width percentage

            // Get the screen height
            float screenHeight = Screen.height;

            // Calculate the target height based on the screen height percentage
            float targetHeight = screenHeight * (m_SizePercentageOfScreenHeight / 100f);

            // Calculate the scale factor needed to fit the target height while keeping the aspect ratio
            float scaleFactor = targetHeight / m_RectTransform.sizeDelta.y;

            if (scaleFactor * m_RectTransform.sizeDelta.x > widthMinimum)
            {
                scaleFactor = widthMinimum / m_RectTransform.sizeDelta.x;
            }

            m_RectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        }

        #endregion

        #region Transitions

        public void EnterEditMode(ProductRefinedData data)
        {
            gameObject.SetActive(true);
            m_EditParent.SetActive(true);
            m_ResultParent.SetActive(false);
            m_CurrentData = data;
            m_NameInputField.text = data.Name;
            m_PriceInputField.text = data.Price.ToString("F2");
            MovePanel(TargetPosition.Center);
        }

        private void transitionToResult(bool success)
        {
            setButtonsInteractability(true);
            m_EditParent.SetActive(false);
            m_ResultParent.SetActive(true);
            m_RetryButton.gameObject.SetActive(!success);
            m_ResultText.text = success == true ? "Update Succeeded" : "Update Failed";

            if (success == true)
            {
                m_CurrentData.Name = m_NameInputField.text;

                if (float.TryParse(m_PriceInputField.text, out m_CurrentData.Price) == false)
                {
                    Debug.LogError("Couldnt parse new price from string, make sure price input field content type is decimal number");
                }

                EventDispatcher<int>.Raise(EditEvents.RefreshProductData.ToString(), m_CurrentData.Index);
            }
        }

        #endregion

        #region Movement

        public void MovePanel(TargetPosition targetPosition, bool immidiate = false)
        {
            setButtonsInteractability(false);

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

                m_Sequence.Append(m_RectTransform.DOAnchorPosY(endPosition.y, 1)).OnComplete(() => onEndMovement(targetPosition));
            }
        }

        private void onEndMovement(TargetPosition targetPosition)
        {
            if (targetPosition == TargetPosition.Center)
            {
                setButtonsInteractability(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        #endregion

        #region Buttons

        private void setButtonsInteractability(bool interactable)
        {
            m_SaveButton.interactable = interactable;
            m_RetryButton.interactable = interactable;
            m_BackButton.interactable = interactable;
            m_CancelButton.interactable = interactable;
        }

        public void OnClickSave()
        {
            setButtonsInteractability(false);

            float price;
            if (float.TryParse(m_PriceInputField.text, out price) == false)
            {
                Debug.LogError("Couldnt parse new price from string, make sure price input field content type is decimal number");
                setButtonsInteractability(true);
            }
            else
            {
                NetworkMessageSender.UpdateProduct(m_NameInputField.text, price);
            }
        }

        public void OnClickCancel()
        {
            MovePanel(TargetPosition.Above);
        }

        #endregion

        #region Response Handling

        private void gotEditResponse(ProductUpdateResponseData response)
        {
            transitionToResult(response.success);
        }

        private void editRequestFailed()
        {
            transitionToResult(false);
        }

        #endregion
    }
}
