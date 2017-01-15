using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Camera))]
    public class PlayFieldWatcher : MonoBehaviour
    {
        private int m_width;
        private int m_height;
        private float m_aspect;
        private Camera m_camera;

        private const float c_borderFactor = 1.2f;

        void Start()
        {
            m_camera = GetComponent<Camera>();
            m_width = m_camera.pixelWidth;
            m_height = m_camera.pixelHeight;
            m_aspect = m_camera.aspect;
            SetView();
        }

        void Update()
        {
            if ((m_camera.pixelWidth != m_width) || (m_camera.pixelHeight != m_height) || (m_camera.aspect != m_aspect))
            {
                m_width = m_camera.pixelWidth;
                m_height = m_camera.pixelHeight;
                m_aspect = m_camera.aspect;
                SetView();
            }
        }

        private void SetView()
        {
            Bounds playfieldBounds = PlayField.GetDimension();
            float playfieldAspect = playfieldBounds.extents.x / playfieldBounds.extents.y;
            float size;
            if (playfieldAspect <= m_camera.aspect)
            {
                size = playfieldBounds.extents.y * c_borderFactor;
            }
            else
            {
                size = (playfieldBounds.extents.x / m_camera.aspect) * c_borderFactor;
            }

            //position and rotate camera
            transform.position = new Vector3(playfieldBounds.center.x, playfieldBounds.center.y, -10.0f);
            transform.rotation = Quaternion.identity;
            m_camera.orthographicSize = size;
        }

    }
}
