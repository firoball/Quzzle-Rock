using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Text))]
    public class SliderVisualizerUI : MonoBehaviour
    {
        [SerializeField]
        private Slider m_slider;

        private Text m_valueText;

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
                m_valueText.text = value.ToString();
            }
        }
    }
}
