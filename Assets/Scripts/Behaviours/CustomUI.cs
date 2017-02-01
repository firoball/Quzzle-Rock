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
            if (m_tokenSliderUI != null)
            {
                Slider tokenSlider = m_tokenSliderUI.Slider;
                tokenSlider.maxValue = Convert.ToSingle(TokenConfig.StandardToken.Length);
            }

            if (Preferences.Custom != null)
            {
                Preferences preferences = Preferences.Custom;
                m_fieldSliderUI.SetValueHorInt(preferences.ColumnCount);
                m_fieldSliderUI.SetValueVertInt(preferences.RowCount);
                m_turnSliderUI.SetValueInt(preferences.MoveCount);
                m_scoreSliderUI.SetValueInt(preferences.TargetCount);
                m_tokenSliderUI.SetValueInt(preferences.TokenCount);
            }
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

            Preferences preferences = new Preferences();
            preferences.ColumnCount = m_fieldSliderUI.GetValueHorInt();
            preferences.RowCount = m_fieldSliderUI.GetValueVertInt();
            preferences.MoveCount = m_turnSliderUI.GetValueInt();
            preferences.TargetCount = m_scoreSliderUI.GetValueInt();
            preferences.TokenCount = m_tokenSliderUI.GetValueInt();

            Preferences.Current = preferences;
            Preferences.Custom = preferences;
            LoadLevel(1);
        }

    }
}
