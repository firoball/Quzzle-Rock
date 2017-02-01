using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Behaviours
{
    public class DummyTokenDropper : MonoBehaviour
    {
        private Vector3 m_targetPosition;
        private Vector3 m_startPosition;
        private float m_factor;
        private bool m_enabled;

        [SerializeField]
        private float m_fallSpeed = 1.0f;
        [SerializeField]
        private float m_delay = 0.0f;

        private const float c_startPositionOffset = 20.0f;

        void Start()
        {
            m_targetPosition = transform.localPosition;
            m_startPosition = transform.localPosition;
            m_startPosition.y += c_startPositionOffset;
            transform.localPosition = m_startPosition;
            m_factor = 0.0f;
            m_enabled = false;
            StartCoroutine(StartupDelay());
        }

        void Update()
        {
            if (m_enabled)
            {
                m_factor = Mathf.Clamp01(m_factor + m_fallSpeed * Time.deltaTime);
                transform.localPosition = Vector3.Lerp(m_startPosition, m_targetPosition, m_factor);
                if (m_factor >= 1.0f)
                {
                    m_enabled = false;
                }
            }
        }

        private IEnumerator StartupDelay()
        {
            yield return new WaitForSeconds(m_delay);
            m_enabled = true;
        }
    }
}
