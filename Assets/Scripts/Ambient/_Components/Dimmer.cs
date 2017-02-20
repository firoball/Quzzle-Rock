using UnityEngine;

namespace Game.Ambient
{
    [RequireComponent(typeof(Light))]
    public class Dimmer : MonoBehaviour, IDimmerEventTarget
    {
        private Color m_originalColor;
        private Light m_light;
        private bool m_dimEnabled;
        private float m_dimfactor;

        [SerializeField]
        private Color m_dimmedColor;

        private const float c_dimSpeed = 2.5f;

        void Start()
        {
            m_light = GetComponent<Light>();
            m_originalColor = m_light.color;
            m_dimEnabled = false;
            m_dimfactor = 0.0f;
        }

        void Update()
        {
            if (m_dimEnabled)
            {
                if (m_dimfactor < 1.0f)
                {
                    m_dimfactor = Mathf.Clamp01(m_dimfactor + c_dimSpeed * Time.deltaTime);
                    m_light.color = Color.Lerp(m_originalColor, m_dimmedColor, m_dimfactor);
                }
            }
            else
            {
                if (m_dimfactor > 0.0f)
                {
                    m_dimfactor = Mathf.Clamp01(m_dimfactor - c_dimSpeed * Time.deltaTime);
                    m_light.color = Color.Lerp(m_originalColor, m_dimmedColor, m_dimfactor);
                }
            }
        }

        public void OnDimmer(bool enableDim)
        {
            m_dimEnabled = enableDim;
        }

    }
}
