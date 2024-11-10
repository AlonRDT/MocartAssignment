using Architecture.API.Events;
using Architecture.API.Managers.Program;
using NUnit.Framework;
using System.Collections.Generic;
using UI.API.Edit;
using UnityEngine;
using static Architecture.API.Managers.Program.ProgramManager;
using static Architecture.API.Networking.NetworkJsonClasses;

namespace Environment.API.Shelf
{
    public class ShelfLogic : MonoBehaviour
    {
        #region Variables

        [SerializeField] private List<NoteLogic> m_Notes = new List<NoteLogic>();

        #endregion

        #region Ctor/Dtor

        void Awake()
        {
            EventDispatcher<ProductRefinedData>.Register(ProgramEvents.ShowNote.ToString(), showNote);
            EventDispatcher<int>.Register(EditEvents.RefreshProductData.ToString(), refreshProductData);
            EventCallback<int, Vector3>.Register(EditEvents.GetButtonPosition.ToString(), getButtonPosition);
        }

        void OnDestroy()
        {
            EventDispatcher<ProductRefinedData>.Unregister(ProgramEvents.ShowNote.ToString(), showNote);
            EventDispatcher<int>.Register(EditEvents.RefreshProductData.ToString(), refreshProductData);
            EventCallback<int, Vector3>.Unregister(EditEvents.GetButtonPosition.ToString(), getButtonPosition);
        }

        #endregion

        #region Note Managment

        /// <summary>
        /// displays note with data received from server
        /// </summary>
        /// <param name="data"> product data received from server </param>
        private void showNote(ProductRefinedData data)
        {
            m_Notes[data.Index].ShowNote(data);
        }

        /// <summary>
        /// returns world position of note by index
        /// </summary>
        /// <param name="noteIndex"> index of target note </param>
        /// <returns></returns>
        private Vector3 getButtonPosition(int noteIndex)
        {
            return m_Notes[noteIndex].transform.position;
        }

        /// <summary>
        /// update the visualization of the data aftar it was edited
        /// </summary>
        /// <param name="index"></param>
        private void refreshProductData(int index)
        {
            m_Notes[index].RefreshData();
        }

        #endregion
    }
}