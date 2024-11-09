using Architecture.API.Events;
using Architecture.API.Managers.Program;
using System.Collections.Generic;
using UI.API.Edit.Buttons;
using UI.API.Edit.Panel;
using UnityEngine;
using UnityEngine.UI;
using static Architecture.API.Managers.Program.ProgramManager;
using static Architecture.API.Networking.NetworkJsonClasses;

namespace UI.API.Edit
{
    public class EditFrontLogic : MonoBehaviour
    {
        [SerializeField] private List<EnterEditButtonLogic> m_EnterEditButtons = new List<EnterEditButtonLogic>();
        [SerializeField] private EditPanelLogic m_EditPanel;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            EventDispatcher<ProductRefinedData>.Register(ProgramEvents.ShowNote.ToString(), showEditButton);
            EventDispatcher<ProductRefinedData>.Register(EditEvents.EditProduct.ToString(), enterEditMode);

            foreach (EnterEditButtonLogic button in m_EnterEditButtons)
            {
                button.gameObject.SetActive(false);
            }

            m_EditPanel.gameObject.SetActive(false);
        }

        private void showEditButton(ProductRefinedData data)
        {
            m_EnterEditButtons[data.Index].Init(data);
        }

        private void enterEditMode(ProductRefinedData data)
        {
            m_EditPanel.EnterEditMode(data);
        }
    }
}
