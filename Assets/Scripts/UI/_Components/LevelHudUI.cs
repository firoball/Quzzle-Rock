using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Game.UI.Controls;

namespace Game.UI
{
    public class LevelHudUI : DefaultUI, IHudEventTarget
    {

        [SerializeField]
        private Text m_comboText;
        [SerializeField]
        private GameObject m_turnsGauge;
        [SerializeField]
        private GameObject m_scoreGauge;


        private float m_comboFadeTimer;
        private Color m_comboOriginalColor;
        private Color m_comboNoAlphaColor;

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
            if (m_comboText != null)
            {
                m_comboOriginalColor = m_comboText.color;
                m_comboNoAlphaColor = new Vector4 (m_comboOriginalColor.r, m_comboOriginalColor.g, m_comboOriginalColor.b, 0.0f);
                m_comboText.color = m_comboNoAlphaColor;
            }
        }

        void Update()
        {
            if (m_comboFadeTimer > 0.0f)
            {
                m_comboFadeTimer = Mathf.Max(m_comboFadeTimer - c_combofadeSpeed * Time.deltaTime, 0.0f);
                m_comboText.color = Color.Lerp(m_comboNoAlphaColor, m_comboOriginalColor, m_comboFadeTimer);
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
            ExecuteEvents.Execute<IGaugeEventTarget>(m_scoreGauge, null, (x, y) => x.OnUpdate(m_score, m_scoreMax));
        }

        public void OnUpdateScoreMax(int scoreMax)
        {
            m_scoreMax = scoreMax;
            ExecuteEvents.Execute<IGaugeEventTarget>(m_scoreGauge, null, (x, y) => x.OnUpdate(m_score, m_scoreMax));
        }

        public void OnUpdateTurns(int turns)
        {
            m_turns = turns;
            ExecuteEvents.Execute<IGaugeEventTarget>(m_turnsGauge, null, (x, y) => x.OnUpdate(m_turns, m_turnsMax));
        }

        public void OnUpdateTurnsMax(int turnsMax)
        {
            m_turnsMax = turnsMax;
            ExecuteEvents.Execute<IGaugeEventTarget>(m_turnsGauge, null, (x, y) => x.OnUpdate(m_turns, m_turnsMax));
        }

        public void OnUpdateCombos(int combos)
        {
            m_combos = combos;
            UpdateCombos();
        }

        public void OnRestart()
        {
            ExecuteEvents.Execute<IGaugeEventTarget>(m_scoreGauge, null, (x, y) => x.OnReset());
            ExecuteEvents.Execute<IGaugeEventTarget>(m_turnsGauge, null, (x, y) => x.OnReset());
        }

    }
}
