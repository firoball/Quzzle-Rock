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
            m_preferences.Name = m_preferences.ToString();

            Preferences.Current = m_preferences;
            Save();
            PlayerPrefs.Save();
            LoadLevel(1);
        }

        private void Load()
        {
            m_preferences = new Preferences();
            m_preferences.ColumnCount = PlayerPrefs.GetInt("custom.columnCount", m_fieldSliderUI.GetValueHorInt());
            m_preferences.RowCount = PlayerPrefs.GetInt("custom.rowCount", m_fieldSliderUI.GetValueVertInt());
            m_preferences.MoveCount = PlayerPrefs.GetInt("custom.moveCount", m_turnSliderUI.GetValueInt());
            m_preferences.TargetCount = PlayerPrefs.GetInt("custom.targetCount", m_scoreSliderUI.GetValueInt());
            m_preferences.TokenCount = PlayerPrefs.GetInt("custom.tokenCount", m_tokenSliderUI.GetValueInt());
        }

        private void Save()
        {
            PlayerPrefs.SetInt("custom.columnCount", m_preferences.ColumnCount);
            PlayerPrefs.SetInt("custom.rowCount", m_preferences.RowCount);
            PlayerPrefs.SetInt("custom.moveCount", m_preferences.MoveCount);
            PlayerPrefs.SetInt("custom.targetCount", m_preferences.TargetCount);
            PlayerPrefs.SetInt("custom.tokenCount", m_preferences.TokenCount);
        }
    }
}
