using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Text))]
    public class SliderVisualizer2DUI : MonoBehaviour
    {
        [SerializeField]
        private Slider m_sliderHorizontal;
        [SerializeField]
        private Slider m_sliderVertical;

        private Text m_valueText;
        float m_valueHorizontal;
        float m_valueVertical;

        void Start()
        {
            m_valueText = GetComponent<Text>();
            if ((m_sliderHorizontal != null) && (m_sliderVertical != null))
            {
                m_sliderHorizontal.onValueChanged.AddListener((x) => OnChangeHorizontal(x));
                m_sliderVertical.onValueChanged.AddListener((x) => OnChangeVertical(x));
                m_valueHorizontal = m_sliderHorizontal.value;
                m_valueVertical = m_sliderVertical.value;
                UpdateValue();
            }
        }

        public void OnChangeHorizontal(float value)
        {
            m_valueHorizontal = value;
            UpdateValue();
        }

        public void OnChangeVertical(float value)
        {
            m_valueVertical = value;
            UpdateValue();
        }

        private void UpdateValue()
        {
            m_valueText.text = m_valueHorizontal.ToString() + "x" + m_valueVertical.ToString();
        }
    }
}
