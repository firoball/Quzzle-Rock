using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Classes;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Toggle))]
    public class ResultToggleUI : MonoBehaviour, IOptionEventTarget
    {
        private Toggle m_toggle;
        private bool m_lastValue;
        private bool m_showResults;

        void Start()
        {
            m_toggle = GetComponent<Toggle>();
            m_toggle.onValueChanged.AddListener((x) => OnChange(x));
            Load();
            m_toggle.isOn = m_showResults;
            m_lastValue = m_showResults;
        }

        public void OnChange(bool toggle)
        {
            m_showResults = toggle;
        }

        public void OnConfirm()
        {
            Options.ShowResults = m_showResults;
            m_lastValue = m_showResults;
            Save();
        }

        public void OnAbort()
        {
            StartCoroutine(OnAbortDelayed());
        }

        private IEnumerator OnAbortDelayed()
        {
            yield return new WaitForSeconds(0.3f);
            m_toggle.isOn = m_lastValue;
        }

        private void Save()
        {
            int showResults;
            if (m_showResults)
            {
                showResults = 1;
            }
            else
            {
                showResults = 0;
            }
            PlayerPrefs.SetInt("options.showResults", showResults);
        }

        private void Load()
        {
            int defVal;
            if (Options.ShowResults)
            {
                defVal = 1;
            }
            else
            {
                defVal = 0;
            }
            if (PlayerPrefs.GetInt("options.showResults", defVal) == 1)
            {
                m_showResults = true;
            }
            else
            {
                m_showResults = false;
            }
        }
    }
}
