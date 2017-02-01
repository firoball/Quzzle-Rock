using UnityEngine;
using UnityEngine.UI;
using System;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Text))]
    public class SliderVisualizer2DUI : MonoBehaviour
    {
        [SerializeField]
        private Slider m_sliderHorizontal;
        [SerializeField]
        private Slider m_sliderVertical;
        [SerializeField]
        private float m_valueMultiplier = 1.0f;
        [SerializeField]
        private bool m_round = true;

        private Text m_valueText;
        float m_valueHorizontal;
        float m_valueVertical;

        public Slider SliderHor
        {
            get
            {
                return m_sliderHorizontal;
            }
        }

        public Slider SliderVert
        {
            get
            {
                return m_sliderVertical;
            }
        }

        public float GetValueHor()
        {
            if (m_sliderHorizontal != null)
            {
                return m_sliderHorizontal.value * m_valueMultiplier;
            }
            else
            {
                return 0.0f;
            }
        }

        public void SetValueHor(float value)
        {
            if (m_sliderHorizontal != null)
            {
                m_sliderHorizontal.value = value / m_valueMultiplier;
                if (m_round)
                {
                    m_sliderHorizontal.value = Mathf.Round(m_sliderHorizontal.value);
                }
            }
        }

        public float GetValueVert()
        {
            if (m_sliderVertical != null)
            {
                return m_sliderVertical.value * m_valueMultiplier;
            }
            else
            {
                return 0.0f;
            }
        }

        public void SetValueVert(float value)
        {
            if (m_sliderVertical != null)
            {
                m_sliderVertical.value = value / m_valueMultiplier;
                if (m_round)
                {
                    m_sliderVertical.value = Mathf.Round(m_sliderVertical.value);
                }
            }
        }

        public int GetValueHorInt()
        {
            return Convert.ToInt32(GetValueHor());
        }

        public void SetValueHorInt(int value)
        {
            SetValueHor(Convert.ToSingle(value));
        }

        public int GetValueVertInt()
        {
            return Convert.ToInt32(GetValueVert());
        }

        public void SetValueVertInt(int value)
        {
            SetValueVert(Convert.ToSingle(value));
        }

        void Start()
        {
            m_valueText = GetComponent<Text>();
            if ((m_sliderHorizontal != null) && (m_sliderVertical != null))
            {
                m_sliderHorizontal.onValueChanged.AddListener((x) => OnChangeHorizontal(x));
                m_sliderVertical.onValueChanged.AddListener((x) => OnChangeVertical(x));
                m_valueHorizontal = m_sliderHorizontal.value * m_valueMultiplier;
                m_valueVertical = m_sliderVertical.value * m_valueMultiplier;
                UpdateValue();
            }
        }

        public void OnChangeHorizontal(float value)
        {
            m_valueHorizontal = value * m_valueMultiplier;
            UpdateValue();
        }

        public void OnChangeVertical(float value)
        {
            m_valueVertical = value * m_valueMultiplier;
            UpdateValue();
        }

        private void UpdateValue()
        {
            m_valueText.text = m_valueHorizontal.ToString() + "x" + m_valueVertical.ToString();
        }
    }
}
