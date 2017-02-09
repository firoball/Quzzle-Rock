using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class GaugeVisualizerUI : MonoBehaviour, IGaugeEventTarget
    {
        [SerializeField]
        private RectTransform m_gauge;
        [SerializeField]
        private RectTransform m_gaugeBackground;
        [SerializeField]
        private Text m_text;
        [SerializeField]
        private bool m_visualizeIncrement = false;
        [SerializeField]
        private bool m_visualizeDecrement = false;
        [SerializeField]
        private Vector3 m_visualizationScale = Vector3.one;
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float m_animationInhibition = 0.1f;

        private float m_width;
        private float m_timer;
        private float m_increment;
        private int m_lastValue;
        private Vector3 m_originalScale;
        private bool m_visualize;
        private bool m_enabled;

        private const float c_scaleSpeed = 0.2f;
        private const float c_scaleSpeedReduction = 0.12f;

        void Awake()
        {
            if ((m_gauge != null) && (m_gaugeBackground != null))
            {
                m_width = m_gauge.sizeDelta.x;
                m_originalScale = m_gauge.localScale;
            }
            else
            {
                m_width = 0.0f;
                m_originalScale = Vector3.one;
            }

            OnReset();
        }

        void Update()
        {
            if (m_visualize)
            {
                float factor = Mathf.Sin(m_timer);
                if (factor < 0.0f)
                {
                    m_visualize = false;
                }
                factor = Mathf.Clamp01(factor);
                m_gauge.localScale = Vector3.Lerp(m_originalScale, m_visualizationScale, factor);
                m_gaugeBackground.localScale = m_gauge.localScale;

                m_increment = Mathf.Max(m_increment - (Time.deltaTime * c_scaleSpeedReduction), 0.0f);
                m_timer += m_increment;
            }
        }

        public void OnUpdate(int value, int maxValue)
        {
            value = Math.Min(value, maxValue);
            if ((m_gauge != null) && (m_text != null))
            {
                float width = m_width * (Convert.ToSingle(value) / Convert.ToSingle(maxValue));
                m_gauge.sizeDelta = new Vector2(width, m_gauge.sizeDelta.y);
                m_text.text = value + "/" + maxValue;
            }

            if (
                ((m_lastValue < value) && m_visualizeIncrement)
                || ((m_lastValue > value) && m_visualizeDecrement)
                )
            {
                TriggerAnimation();
            }
            m_lastValue = value;
        }

        public void OnReset()
        {
            m_visualize = false;
            m_timer = 0.0f;
            m_enabled = false;

            StartCoroutine(StartupDelay());
        }

        private void TriggerAnimation()
        {
            if ((m_gauge != null) && (m_gaugeBackground != null) && m_enabled)
            {
                m_visualize = true;
                m_timer = 0.0f;
                m_increment = c_scaleSpeed;
            }
        }

        private IEnumerator StartupDelay()
        {
            yield return new WaitForSeconds(m_animationInhibition);
            m_enabled = true;
        }
    }
}
