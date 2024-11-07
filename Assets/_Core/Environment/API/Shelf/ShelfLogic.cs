using Architecture.API.Events;
using Architecture.API.Managers.Program;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Architecture.API.Networking.NetworkJsonClasses;

namespace Environment.API.Shelf
{
    public class ShelfLogic : MonoBehaviour
    {
        #region Variables

        [SerializeField] private List<NoteLogic> m_Notes = new List<NoteLogic>();

        #endregion

        void Awake()
        {
            EventDispatcher<ProductsData.ProductData>.Register(ProgramEvents.ShowNote.ToString(), showNote);
        }

        private void showNote(ProductsData.ProductData data)
        {
            foreach (NoteLogic note in m_Notes)
            {
                if(note.IsActive == false)
                {
                    note.ShowNote(data);
                    break;
                }
            }
        }

        void OnDestroy()
        {
            EventDispatcher<ProductsData.ProductData>.Unregister(ProgramEvents.ShowNote.ToString(), showNote);
        }
    }
}