using UnityEngine;
using System.Collections;

namespace Game.Ambient
{
    public class TitleScreenDecorator : MonoBehaviour
    {

        private int m_width;
        private int m_height;
        private float m_aspect;
        private Bounds bounds;

        [SerializeField]
        private Camera m_camera;
        [SerializeField]
        private bool m_alignLeft = false;
        [SerializeField]
        private bool m_alignRight = false;
        [SerializeField]
        private float m_distanceFromCenter = 2.0f;

        void Start()
        {
            if (m_camera == null)
            {
                Destroy(gameObject);
            }
            else
            {
                m_width = m_camera.pixelWidth;
                m_height = m_camera.pixelHeight;
                m_aspect = m_camera.aspect;
                UpdatePosition();
            }
        }

        void Update()
        {
            if ((m_camera.pixelWidth != m_width) || (m_camera.pixelHeight != m_height) || (m_camera.aspect != m_aspect))
            {
                m_width = m_camera.pixelWidth;
                m_height = m_camera.pixelHeight;
                m_aspect = m_camera.aspect;
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            float width = GetWidth();
            Vector3 pixelPos;
            if (m_alignLeft)
            {
                pixelPos = new Vector3(0.0f, 0.0f, m_camera.nearClipPlane);
            }
            else if (m_alignRight)
            {
                pixelPos = new Vector3(m_camera.pixelWidth, 0.0f, m_camera.nearClipPlane);
            }
            else
            {
                pixelPos = new Vector3(m_camera.pixelWidth * 0.5f, 0.0f, m_camera.nearClipPlane);
            }
            Vector3 worldPos = m_camera.ScreenToWorldPoint(pixelPos);
            worldPos.z = 0;
            if (m_alignLeft)
            {
                worldPos.x = Mathf.Min(worldPos.x,  -(m_distanceFromCenter + width));
            }
            if (m_alignRight)
            {
                worldPos.x = Mathf.Max(worldPos.x, m_distanceFromCenter + width);
            }
            transform.position = worldPos;
        }

        private float GetWidth()
        {
            bounds = new Bounds(transform.position, Vector3.zero);
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                Encapsulate(child, ref bounds);
            }

            return bounds.extents.x * 2.0f;
        }

        private void Encapsulate(GameObject obj, ref Bounds bounds)
        {
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                bounds.Encapsulate(renderer.bounds);
            }
        }

    }
}
