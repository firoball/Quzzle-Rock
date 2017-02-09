using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Text))]
    public class VolumeSliderVisualizerUI : MonoBehaviour, IOptionEventTarget
    {
        [SerializeField]
        private Slider m_slider;
        [SerializeField]
        private float m_valueMultiplier = 1.0f;
        [SerializeField]
        private float m_valueNormalizer = 1.0f;
        [SerializeField]
        private bool m_round = true;

        private Text m_valueText;
        private float m_volume;
        private float m_lastValue;
        private bool m_enableSound;

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
                return m_slider.value * m_valueNormalizer;
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
                m_slider.value = value / m_valueNormalizer;
                if (m_round)
                {
                    m_slider.value = Mathf.Round(m_slider.value);
                }
            }
        }

        void Start()
        {
            m_enableSound = false;
            m_valueText = GetComponent<Text>();
            m_slider.onValueChanged.AddListener((x) => OnChange(x));
            Load();
            SetValue(m_volume);
            OnChange(m_slider.value); //text display would not always update without - for whatever reason
            m_lastValue = m_slider.value;
            m_enableSound = true;
        }

        public void OnChange(float value)
        {
            m_volume = value * m_valueNormalizer;
            if (m_valueText != null)
            {
                value *= m_valueMultiplier;
                m_valueText.text = value.ToString();
            }
            StartCoroutine(PlayVolumeSample(m_enableSound));
        }

        public void OnConfirm()
        {
            AudioListener.volume = m_volume;
            m_lastValue = m_slider.value;
            Save();
        }

        public void OnAbort()
        {
            StartCoroutine(OnAbortDelayed());
        }

        private IEnumerator PlayVolumeSample(bool enabled)
        {
            //only play if slider hasn't changed position for some time
            float volume = m_volume;
            yield return new WaitForSeconds(0.3f);
            if (volume == m_volume)
            {
                AudioListener.volume = m_volume;
                if (enabled)
                {
                    AudioManager.Play("volume sample");
                }
            }
        }

        private IEnumerator OnAbortDelayed()
        {
            yield return new WaitForSeconds(0.3f);
            m_enableSound = false;
            m_slider.value = m_lastValue;
            m_enableSound = true;
        }

        private void Save()
        {
            PlayerPrefs.SetFloat("Audio Volume", m_volume);
        }

        private void Load()
        {
            m_volume = PlayerPrefs.GetFloat("Audio Volume", AudioListener.volume);
        }
    }
}
