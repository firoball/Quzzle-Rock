using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Camera))]
    public class PlayFieldWatcher : MonoBehaviour, ICameraShakeTarget
    {
        private Camera m_camera;
        private int m_width;
        private int m_height;
        private float m_aspect;
        private float m_shakeTimer;
        private float m_shakeIntensity;
        private Vector3 m_originalPosition;

        private const float c_borderFactor = 1.2f;
        private const float c_shakeSpeed = 1.3f;
        private const float c_shakeFactor = 0.3f;

        void Start()
        {
            m_camera = GetComponent<Camera>();
            m_width = m_camera.pixelWidth;
            m_height = m_camera.pixelHeight;
            m_aspect = m_camera.aspect;
            m_shakeTimer = 0.0f;
            m_originalPosition = transform.position;
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

            if (m_shakeTimer > 0.0f)
            {
                ShakeUpdate();
            }
            else
            {
                transform.position = m_originalPosition;
            }
        }

        public void OnShake(float intensity)
        {
            m_shakeTimer = 1.0f;
            m_shakeIntensity = intensity;
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

        private void ShakeUpdate()
        {
            float intensity = m_shakeIntensity * m_shakeTimer;
            float x = intensity * c_shakeFactor * Mathf.Sin(m_shakeTimer * 30.0f);
            float y = intensity * c_shakeFactor * Mathf.Sin(m_shakeTimer * 30.0f + 12345.0f);

            transform.position = new Vector3(m_originalPosition.x + x, m_originalPosition.y + y, m_originalPosition.z);
            m_shakeTimer = Mathf.Max(m_shakeTimer - Time.deltaTime * c_shakeSpeed, 0.0f);
        }
    }
}
