using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts.Classes;

namespace Assets.Scripts.Behaviours
{
    public class CustomUI : DefaultUI
    {

        [SerializeField]
        private SliderVisualizer2DUI m_fieldSliderUI;
        [SerializeField]
        private SliderVisualizerUI m_turnSliderUI;
        [SerializeField]
        private SliderVisualizerUI m_scoreSliderUI;
        [SerializeField]
        private SliderVisualizerUI m_tokenSliderUI;

        private Preferences m_preferences;

        void Start()
        {
            if (
                (m_fieldSliderUI == null)
                || (m_turnSliderUI == null) || (m_scoreSliderUI == null) || (m_tokenSliderUI == null)
                )
            {
                Debug.LogWarning("CustomUI: Slider references not set.");
                return;
            }

            //adjust token slider range to configured token count
            Slider tokenSlider = m_tokenSliderUI.Slider;
            tokenSlider.maxValue = Convert.ToSingle(TokenConfig.StandardToken.Length);

            Load();
            m_fieldSliderUI.SetValueHorInt(m_preferences.ColumnCount);
            m_fieldSliderUI.SetValueVertInt(m_preferences.RowCount);
            m_turnSliderUI.SetValueInt(m_preferences.MoveCount);
            m_scoreSliderUI.SetValueInt(m_preferences.TargetCount);
            m_tokenSliderUI.SetValueInt(m_preferences.TokenCount);
        }

        public void Ok()
        {
            if (
                (m_fieldSliderUI == null)
                || (m_turnSliderUI == null) || (m_scoreSliderUI == null) || (m_tokenSliderUI == null)
                )
            {
                Debug.LogWarning("CustomUI: Slider references not set.");
                return;
            }

            m_preferences.ColumnCount = m_fieldSliderUI.GetValueHorInt();
            m_preferences.RowCount = m_fieldSliderUI.GetValueVertInt();
            m_preferences.MoveCount = m_turnSliderUI.GetValueInt();
            m_preferences.TargetCount = m_scoreSliderUI.GetValueInt();
            m_preferences.TokenCount = m_tokenSliderUI.GetValueInt();

            Preferences.Current = m_preferences;
            Save();
            LoadLevel(1);
        }

        private void Load()
        {
            m_preferences = new Preferences();
            m_preferences.ColumnCount = PlayerPrefs.GetInt("Custom ColumnCount", m_fieldSliderUI.GetValueHorInt());
            m_preferences.RowCount = PlayerPrefs.GetInt("Custom RowCount", m_fieldSliderUI.GetValueVertInt());
            m_preferences.MoveCount = PlayerPrefs.GetInt("Custom MoveCount", m_turnSliderUI.GetValueInt());
            m_preferences.TargetCount = PlayerPrefs.GetInt("Custom TargetCount", m_scoreSliderUI.GetValueInt());
            m_preferences.TokenCount = PlayerPrefs.GetInt("Custom TokenCount", m_tokenSliderUI.GetValueInt());
        }

        private void Save()
        {
            PlayerPrefs.SetInt("Custom ColumnCount", m_preferences.ColumnCount);
            PlayerPrefs.SetInt("Custom RowCount", m_preferences.RowCount);
            PlayerPrefs.SetInt("Custom MoveCount", m_preferences.MoveCount);
            PlayerPrefs.SetInt("Custom TargetCount", m_preferences.TargetCount);
            PlayerPrefs.SetInt("Custom TokenCount", m_preferences.TokenCount);
        }
    }
}
