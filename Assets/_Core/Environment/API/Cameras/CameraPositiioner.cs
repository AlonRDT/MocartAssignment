using System.Collections.Generic;
using System;
using UnityEngine;
using Architecture.API.Managers;

namespace Environment.API.Cameras
{
    public class CameraPositiioner : MonoBehaviour
    {
        private Camera m_Camera;
        [SerializeField] private List<Transform> m_AlignmentPoints = new List<Transform>();

        // Start is called before the first frame update
        void Start()
        {
            m_Camera = GetComponent<Camera>();
            AdjustCamera();
            ScreenManager.OnScreenSizeChange += AdjustCamera;
        }

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
    }
}