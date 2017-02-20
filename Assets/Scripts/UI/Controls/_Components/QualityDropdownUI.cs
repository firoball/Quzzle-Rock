using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace Game.UI.Controls
{
    [RequireComponent(typeof(Dropdown))]
    public class QualityDropdownUI : MonoBehaviour, IOptionEventTarget
    {
        private Dropdown m_dropdown;
        private int m_qualityLevel;
        private int m_lastQualitylevel;

        void Start()
        {
            m_dropdown = GetComponent<Dropdown>();
            m_dropdown.ClearOptions();

            string[] names = QualitySettings.names;
            //m_lastQualitylevel = QualitySettings.GetQualityLevel();
            Load();
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            for (int i = 0; i < names.Length; i++)
            {
                Dropdown.OptionData data = new Dropdown.OptionData(names[i]);
                options.Add(data);
            }
            m_dropdown.AddOptions(options);
            m_dropdown.onValueChanged.AddListener((x) => OnChange(x));
            m_dropdown.value = m_lastQualitylevel;
        }

        public void OnChange(int value)
        {
            m_qualityLevel = value;
        }

        public void OnConfirm()
        {
            QualitySettings.SetQualityLevel(m_qualityLevel);
            m_lastQualitylevel = m_qualityLevel;
            Save();
        }

        public void OnAbort()
        {
            StartCoroutine(OnAbortDelayed());
        }

        private IEnumerator OnAbortDelayed()
        {
            yield return new WaitForSeconds(0.3f);
            m_dropdown.value = m_lastQualitylevel;
        }

        private void Save()
        {
            PlayerPrefs.SetInt("options.qualityLevel", m_lastQualitylevel);
        }

        private void Load()
        {
            m_lastQualitylevel = PlayerPrefs.GetInt("options.qualityLevel", QualitySettings.GetQualityLevel());
        }
    }
}
