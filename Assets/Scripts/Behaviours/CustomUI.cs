using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts.Classes;

namespace Assets.Scripts.Behaviours
{
    public class CustomUI : DefaultUI
    {

        [SerializeField]
        private Slider m_columnSlider;
        [SerializeField]
        private Slider m_rowSlider;
        [SerializeField]
        private Slider m_turnSlider;
        [SerializeField]
        private Slider m_scoreSlider;
        [SerializeField]
        private Slider m_tokenSlider;

        void Start()
        {
            //adjust token slider range to configured token count
            if (m_tokenSlider != null)
            {
                m_tokenSlider.maxValue = Convert.ToSingle(TokenConfig.StandardToken.Length);
            }
        }

        public void Ok()
        {
            if (
                (m_columnSlider == null) || (m_rowSlider == null)
                || (m_turnSlider == null) || (m_scoreSlider == null) || (m_tokenSlider == null)
                )
            {
                Debug.LogWarning("CustomUI: Slider references not set.");
                return;
            }

            Preferences preferences = new Preferences();
            preferences.ColumnCount = Convert.ToInt32(m_columnSlider.value);
            preferences.RowCount = Convert.ToInt32(m_rowSlider.value);
            preferences.MoveCount = Convert.ToInt32(m_turnSlider.value);
            preferences.TargetCount = Convert.ToInt32(m_scoreSlider.value);
            preferences.TokenCount = Convert.ToInt32(m_tokenSlider.value);

            Preferences.Current = preferences;
            LoadLevel(1);
        }

    }
}
