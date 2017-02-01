using UnityEngine;
using UnityEngine.UI;
using System;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Text))]
    public class SliderVisualizerUI : MonoBehaviour
    {
        [SerializeField]
        private Slider m_slider;
        [SerializeField]
        private float m_valueMultiplier = 1.0f;
        [SerializeField]
        private bool m_round = true;

        private Text m_valueText;

        public Slider Slider
        {
            get
            {
                return m_slider;
            }
        }

        public float GetValue()
        {
            if (m_slider != null)
            {
                return m_slider.value * m_valueMultiplier;
            }
            else
            {
                return 0.0f;
            }
        }

        public void SetValue(float value)
        {
            if (m_slider != null)
            {
                m_slider.value = value / m_valueMultiplier;
                if (m_round)
                {
                    m_slider.value = Mathf.Round(m_slider.value);
                }
            }
        }

        public int GetValueInt()
        {
            return Convert.ToInt32(GetValue());
        }

        public void SetValueInt(int value)
        {
            SetValue(Convert.ToSingle(value));
        }

        void Start()
        {
            m_valueText = GetComponent<Text>();
            m_slider.onValueChanged.AddListener((x) => OnChange(x));
            OnChange(m_slider.value);
        }

        public void OnChange(float value)
        {
            if (m_valueText != null)
            {
                value *= m_valueMultiplier;
                m_valueText.text = value.ToString();
            }
        }
    }
}
