using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class LevelHudUI : DefaultUI, IHudEventTarget
    {

        [SerializeField]
        private RectTransform m_turnsGauge;
        [SerializeField]
        private RectTransform m_scoreGauge;
        [SerializeField]
        private Text m_turnsText;
        [SerializeField]
        private Text m_scoreText;
        [SerializeField]
        private Text m_comboText;
        [SerializeField]
        private Text m_endText;


        private float m_turnsWidth;
        private float m_scoreWidth;
        private float m_comboFadeTimer;
        private Color m_comboOriginalColor;
        private Color m_comboNoAlphaColor;
        private float m_endFadeTimer;
        private Color m_endOriginalColor;
        private Color m_endNoAlphaColor;

        private int m_combos;
        private int m_turns;
        private int m_score;
        private int m_turnsMax;
        private int m_scoreMax;

        private const float c_combofadeSpeed = 1.5f;
        private const float c_endfadeSpeed = 0.5f;

        protected override void Awake()
        {
            base.Awake();
            if (m_turnsGauge != null)
            {
                m_turnsWidth = m_turnsGauge.sizeDelta.x;
            }
            if (m_scoreGauge != null)
            {
                m_scoreWidth = m_scoreGauge.sizeDelta.x;
            }
            if (m_comboText != null)
            {
                m_comboOriginalColor = m_comboText.color;
                m_comboNoAlphaColor = new Vector4 (m_comboOriginalColor.r, m_comboOriginalColor.g, m_comboOriginalColor.b, 0.0f);
                m_comboText.color = m_comboNoAlphaColor;
            }
            if (m_endText != null)
            {
                m_endText.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            }
        }

        void Update()
        {
            if (m_comboFadeTimer > 0.0f)
            {
                m_comboFadeTimer = Mathf.Max(m_comboFadeTimer - c_combofadeSpeed * Time.deltaTime, 0.0f);
                m_comboText.color = Color.Lerp(m_comboNoAlphaColor, m_comboOriginalColor, m_comboFadeTimer);
            }
            if (m_endFadeTimer > 0.0f)
            {
                m_endFadeTimer = Mathf.Max(m_endFadeTimer - c_endfadeSpeed * Time.deltaTime, 0.0f);
                m_endText.color = Color.Lerp(m_endNoAlphaColor, m_endOriginalColor, m_endFadeTimer);
            }
        }

        private void UpdateTurns()
        {
            if ((m_turnsGauge != null) && (m_turnsText != null))
            {
                float width = m_turnsWidth * (Convert.ToSingle(m_turns) / Convert.ToSingle(m_turnsMax));
                m_turnsGauge.sizeDelta = new Vector2(width, m_turnsGauge.sizeDelta.y);
                m_turnsText.text = m_turns + "/" + m_turnsMax;
            }
        }

        private void UpdateScore()
        {
            if ((m_scoreGauge != null) && (m_scoreText != null))
            {
                float width = m_scoreWidth * (Convert.ToSingle(m_score) / Convert.ToSingle(m_scoreMax));
                m_scoreGauge.sizeDelta = new Vector2(width, m_scoreGauge.sizeDelta.y);
                m_scoreText.text = m_score + "/" + m_scoreMax;
            }
        }

        private void UpdateCombos()
        {
            if (m_combos > 1)
            {
                m_comboText.color = m_comboOriginalColor;
                m_comboFadeTimer = 1.0f;
                m_comboText.text = m_combos.ToString();
            }
            else
            {
                m_comboText.text = "";
            }
        }

        public void OnUpdateScore(int score)
        {
            m_score = score;
            UpdateScore();
        }

        public void OnUpdateScoreMax(int scoreMax)
        {
            m_scoreMax = scoreMax;
            UpdateScore();
        }

        public void OnUpdateTurns(int turns)
        {
            m_turns = turns;
            UpdateTurns();
        }

        public void OnUpdateTurnsMax(int turnsMax)
        {
            m_turnsMax = turnsMax;
            UpdateTurns();
        }

        public void OnUpdateCombos(int combos)
        {
            m_combos = combos;
            UpdateCombos();
        }

        public void OnGameEnded(bool success)
        {
            //since this - unlike the rest of this gui - is generic, it should be handled separately
            if (m_endText != null)
            {
                if (success)
                {
                    m_endOriginalColor = Color.green;
                    m_endText.text = "You won";
                }
                else
                {
                    m_endOriginalColor = Color.red;
                    m_endText.text = "You lost";
                }
                m_endNoAlphaColor = new Vector4(m_endOriginalColor.r, m_endOriginalColor.g, m_endOriginalColor.b, 0.0f);
                m_endText.color = m_endNoAlphaColor;
                m_endFadeTimer = 1.0f;
            }
        }
    }
}
