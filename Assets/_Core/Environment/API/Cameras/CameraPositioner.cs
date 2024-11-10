using System.Collections.Generic;
using System;
using UnityEngine;
using Architecture.API.Managers;

namespace Environment.API.Cameras
{
    public class CameraPositioner : MonoBehaviour
    {
        #region Variables

        private Camera m_Camera;
        [SerializeField] private List<Transform> m_AlignmentPoints = new List<Transform>(); // points that camera should always keep in view

        #endregion

        #region Ctor/Dtor

        void Awake()
        {
            m_Camera = GetComponent<Camera>();
            AdjustCamera();
            ScreenManager.OnScreenSizeChangeFirst += AdjustCamera;
        }

        private void OnDestroy()
        {
            ScreenManager.OnScreenSizeChangeFirst -= AdjustCamera;
        }

        /// <summary>
        /// camera should remain on same plane and move on z axis to keep alignment point so if screen size changes than view of scenes remain as much the same as
        /// possible
        /// </summary>
        public void AdjustCamera()
        {
            float Length = Mathf.Abs(m_AlignmentPoints[0].position.x - m_AlignmentPoints[1].position.x) / 2;
            float height = Mathf.Abs(m_AlignmentPoints[0].position.y - m_AlignmentPoints[1].position.y) / 2;
            //float angleInRad = angle * ;
            float angleY = (m_Camera.fieldOfView / 2) * Mathf.Deg2Rad;

            float aspectRatio = Screen.width / (float)Screen.height;

            // Convert vertical FOV from degrees to radians
            double verticalFovRadians = m_Camera.fieldOfView * Math.PI / 180.0;

            // Calculate the horizontal FOV using the formula
            double horizontalFovRadians = 2 * Math.Atan(Math.Tan(verticalFovRadians / 2) * aspectRatio);

            float angleX = (float)(horizontalFovRadians / 2);

            float distanceY = height / Mathf.Tan(angleY);
            float distanceX = Length / Mathf.Tan(angleX);
            float distance = Mathf.Max(distanceX, distanceY);

            transform.position = new Vector3(0, 0, m_AlignmentPoints[0].position.z - distance);
        }

        #endregion
    }
}