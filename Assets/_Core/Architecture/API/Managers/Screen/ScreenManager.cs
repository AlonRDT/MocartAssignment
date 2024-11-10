using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.API.Managers
{
    public class ScreenManager : Manager
    {
        #region Variables

        public static event Action OnScreenSizeChangeFirst; // first actions called every time screen size changes, phone flips,
        public static event Action OnScreenSizeChangeSecond; // second action called every time screen size changes, phone flips,
        // browser is stretched and unity window is flexible
        public static float WorldMaxX; // top most world x value in camera view
        public static float WorldMinX; // bottom most world x value in camera view
        public static float WorldMaxY; // right most world y value in camera view
        public static float WorldMinY; // left most world y value in camera view
        public static Vector3 WorldCenter; // center of camera view

        private Camera m_MainCamera;
        private int m_ScreenWidth; // keeps last screen width to check if screen size changed
        private int m_ScreenHeight; // keeps last screen height to check if screen size changed

        #endregion

        #region Ctor/Dtor

        /// <summary>
        /// initializes component, called on enable to let scripts register to on screen size change event in awake
        /// </summary>
        private void OnEnable()
        {
            m_MainCamera = Camera.main;
            SetCameraParameters();
            OnScreenSizeChangeFirst?.Invoke();
            OnScreenSizeChangeSecond?.Invoke();
        }

        void OnDestroy()
        {
            OnScreenSizeChangeFirst = null;
            OnScreenSizeChangeSecond = null;
        }

        /// <summary>
        /// sets global parameters
        /// </summary>
        private void SetCameraParameters()
        {
            m_ScreenHeight = Screen.height;
            m_ScreenWidth = Screen.width;

            Vector3 topRightCorner = m_MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
                    -m_MainCamera.transform.position.z));
            Vector3 bottomLeftCorner = m_MainCamera.ScreenToWorldPoint(new Vector3(0, 0,
                    -m_MainCamera.transform.position.z));

            WorldMaxX = topRightCorner.x;
            WorldMinX = bottomLeftCorner.x;
            WorldMaxY = topRightCorner.y;
            WorldMinY = bottomLeftCorner.y;
            WorldCenter = new Vector3(Mathf.Lerp(WorldMaxX, WorldMinX, 0.5f), Mathf.Lerp(WorldMaxY, WorldMinY, 0.5f), 0);
        }

        #endregion

        // check if screen size changed and if so updates listeners
        void Update()
        {
            if (m_ScreenWidth != Screen.width || m_ScreenHeight != Screen.height)
            {
                SetCameraParameters();
                OnScreenSizeChangeFirst?.Invoke();
                OnScreenSizeChangeSecond?.Invoke();
            }
        }

        #region Helper Methods

        /// <summary>
        /// returns world point correlating to screen point by percentages
        /// </summary>
        /// <param name="xPercent"> desired presentation of world point by width percentage </param>
        /// <param name="yPercent"> desired presentation of world point by height percentage </param>
        /// <returns></returns>
        public static Vector3 GetWorldPositionByScreenPercentage(float xPercent, float yPercent)
        {
            Vector3 output = Vector3.zero;

            output.x = Mathf.Lerp(WorldMinX, WorldMaxX, xPercent);
            output.y = Mathf.Lerp(WorldMinY, WorldMaxY, yPercent);

            return output;
        }

        /// <summary>
        /// sets the position of a ui element on camera in front of desired world point
        /// </summary>
        /// <param name="uiElement"> ui elemnt to place </param>
        /// <param name="worldPosition"> desired world point to place ui elemnt in front of </param>
        public static void SetPositonByWorldPos(RectTransform uiElement, Vector3 worldPosition)
        {
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);

            // Set the local position of the UI element
            uiElement.position = screenPosition;
        }

        #endregion
    }
}
