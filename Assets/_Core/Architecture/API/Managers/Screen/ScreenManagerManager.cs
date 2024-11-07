using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Architecture.API.Managers
{
    public class ScreenManager : Manager
    {
        public static event Action OnScreenSizeChange;
        public static float WorldMaxX;
        public static float WorldMinX;
        public static float WorldMaxY;
        public static float WorldMinY;
        public static Vector3 WorldCenter;

        private Camera m_MainCamera;
        private int m_ScreenWidth;
        private int m_ScreenHeight;

        private void OnEnable()
        {
            m_MainCamera = Camera.main;
            OnScreenSizeChange = null;
            SetCameraParameters();
            OnScreenSizeChange?.Invoke();
        }

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

        // Update is called once per frame
        void Update()
        {
            if (m_ScreenWidth != Screen.width || m_ScreenHeight != Screen.height)
            {
                SetCameraParameters();
                OnScreenSizeChange?.Invoke();
            }
        }

        public static Vector3 GetWorldPositionByScreenPercentage(float xPercent, float yPercent)
        {
            Vector3 output = Vector3.zero;

            output.x = Mathf.Lerp(WorldMinX, WorldMaxX, xPercent);
            output.y = Mathf.Lerp(WorldMinY, WorldMaxY, yPercent);

            return output;
        }

        public static void SetPositonByWorldPos(RectTransform uiElement, Vector3 worldPosition)
        {
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);

            // Set the local position of the UI element
            uiElement.position = screenPosition;
        }

        void OnDestroy()
        {
            OnScreenSizeChange = null;
        }
    }
}
