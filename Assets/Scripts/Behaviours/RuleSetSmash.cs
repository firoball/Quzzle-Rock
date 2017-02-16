using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Assets.Scripts.Structs;
using Assets.Scripts.Classes;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(AudioSource))]
    public class RuleSetSmash : RuleSet
    {
        [SerializeField]
        [Range(10, 1000)]
        private int m_stackSize = 50;
        [SerializeField]
        private bool m_debug = false;
        [SerializeField]
        private GameObject m_levelHudUI = null;
        [SerializeField]
        private GameObject m_resultUI = null;
        [SerializeField]
        private GameObject m_combo4ToolTip = null;
        [SerializeField]
        private GameObject m_combo5ToolTip = null;

        private int m_turnsLeft;
        private int m_combos;
        private int m_points;
        private int m_turnsMax;
        private int m_pointsMax;
        private PositionStack m_positionStack;
        private AudioSource m_audio;
        private float m_audioPitch;
        private float m_shakeIntensity;
        private GameObject m_cameraObj;
        private StatisticsManager m_statistics;

        private const int c_specialTokenId = 100;
        private const float c_pitchIncrement = /*0.07f;*/0.1f;
        private const float c_pitchMax = /*2.0f;*/2.5f;
        private const float c_defaultShakeIntensity = 0.5f;
        private const float c_shakeIntensityIncrement = 0.2f;
        private const float c_shakeIntensityMaximum = 1.7f;
        private const int c_alertTurnsLeft = 5;
        private const int c_modifierBonusAmount = 8;

        private int TurnsLeft
        {
            get
            {
                return m_turnsLeft;
            }

            set
            {
                m_turnsLeft = value;
                ExecuteEvents.Execute<IHudEventTarget>(m_levelHudUI, null, (x, y) => x.OnUpdateTurns(m_turnsLeft));
            }
        }

        private int Combos
        {
            get
            {
                return m_combos;
            }

            set
            {
                m_combos = value;
                ExecuteEvents.Execute<IHudEventTarget>(m_levelHudUI, null, (x, y) => x.OnUpdateCombos(m_combos));
            }
        }

        private int Points
        {
            get
            {
                return m_points;
            }

            set
            {
                m_points = value;
                ExecuteEvents.Execute<IHudEventTarget>(m_levelHudUI, null, (x, y) => x.OnUpdateScore(m_points));
            }
        }

        private int TurnsMax
        {
            get
            {
                return m_turnsMax;
            }

            set
            {
                m_turnsMax = value;
                ExecuteEvents.Execute<IHudEventTarget>(m_levelHudUI, null, (x, y) => x.OnUpdateTurnsMax(m_turnsMax));
            }
        }

        private int PointsMax
        {
            get
            {
                return m_pointsMax;
            }

            set
            {
                m_pointsMax = value;
                ExecuteEvents.Execute<IHudEventTarget>(m_levelHudUI, null, (x, y) => x.OnUpdateScoreMax(m_pointsMax));
            }
        }

        void Awake()
        {
            m_audio = GetComponent<AudioSource>();
            m_statistics = new StatisticsManager(m_resultUI, Preferences.Current.Name);
            m_positionStack = new PositionStack(m_stackSize, m_debug);
            m_audioPitch = 1.0f;
            m_shakeIntensity = c_defaultShakeIntensity;
        }

        void Start()
        {
            if (Camera.main != null)
            {
                m_cameraObj = Camera.main.gameObject;
            }
            else
            {
                m_cameraObj = null;
            }
            //Events are not available yet during Awake()...
            TurnsMax = Preferences.Current.MoveCount;
            PointsMax = Preferences.Current.TargetCount;
        }

        public override void ProcessCombo(List<Combination> combinationList)
        {
            bool scored = false;
            bool extraTurn = false;
            foreach (Combination combination in combinationList)
            {
                if (combination.Id == c_specialTokenId)
                {
                    Points = Math.Min(Points + combination.Positions.Count, PointsMax);
                    scored = true;
                }
                if (combination.Positions.Count == 4)
                {
                    SpawnLabelForCombination(m_combo4ToolTip, combination);
                    m_statistics.ExtraTurns++;
                    TurnsLeft = Math.Min(TurnsLeft + 1, TurnsMax);
                    extraTurn = true;
                }
                else if (combination.Positions.Count > 4)
                {
                    SpawnLabelForCombination(m_combo5ToolTip, combination);
                    m_statistics.ExtraTurns += 2;
                    TurnsLeft = Math.Min(TurnsLeft + 2, TurnsMax);
                    extraTurn = true;
                }
            }
            Combos++;

            //play sound
            if (combinationList.Count > 0)
            {
                m_audio.pitch = m_audioPitch;
                m_audio.Play();
                m_audioPitch = Mathf.Min(m_audioPitch + c_pitchIncrement, c_pitchMax);
            }

            if (scored)
            {
                AudioManager.Play("score");
                ExecuteEvents.Execute<ICameraShakeTarget>(m_cameraObj, null, (x, y) => x.OnShake(m_shakeIntensity));
                m_shakeIntensity = Math.Min(m_shakeIntensity + c_shakeIntensityIncrement, c_shakeIntensityMaximum);
            }
            else
            {
                m_shakeIntensity = c_defaultShakeIntensity;
            }

            if (extraTurn)
            {
                AudioManager.Play("extra turn");
            }

        }

        public override void PlayerDone()
        {
            TurnsLeft--;
        }

        public override void TurnStart()
        {
            Combos = 0;
            m_audioPitch = 1.0f;
            m_shakeIntensity = c_defaultShakeIntensity;
        }

        public override void TurnEnd()
        {
            m_statistics.Turns++;
            TriggerAlert();
            TriggerResults();
            Combos = 0;
        }

        public override void Restart()
        {
            ExecuteEvents.Execute<IHudEventTarget>(m_levelHudUI, null, (x, y) => x.OnRestart());
            TurnsLeft = TurnsMax;
            Points = 0;
            m_positionStack.Restart();
            TriggerAlert();
            m_statistics.Begin();
        }

        public override bool IsGameLost()
        {
            if (TurnsLeft == 0)
            {
                return true;
            }
            return false;
        }

        public override bool IsGameWon()
        {
            if (Points >= PointsMax)
            {
                return true;
            }
            return false;
        }

        public override void Abort()
        {
            m_statistics.End(StatisticsManager.Finish.ABORTED, Points);
        }

        public override void PlayFieldModifier()
        {
            //trigger every 4th combo
            if ((Combos % 4) == 0)
            {
                Dictionary<DataFieldPosition, int> tokenDict = new Dictionary<DataFieldPosition, int>();
                for (int i = 0; i < c_modifierBonusAmount; i++)
                {
                    DataFieldPosition pos;
                    //filter doubles
                    do
                    {
                        pos = m_positionStack.GetNextPosition();
                    } while (tokenDict.ContainsKey(pos));
                    tokenDict.Add(pos, c_specialTokenId);
                }
                PlayField.CreateTokensFromDictionary(tokenDict);
                tokenDict.Clear();

                AudioManager.Play("bonus");

            }
        }

        private void SpawnLabelForCombination(GameObject obj, Combination combination)
        {
            if (obj != null)
            {
                Vector3 center = PlayField.GetCombinationCenter(combination);
                Instantiate(obj, center, Quaternion.identity);
            }
        }

        private void TriggerAlert()
        {
            float intensity;
            if (!IsGameLost() && !IsGameWon())
            {
                intensity = Convert.ToSingle(1 + c_alertTurnsLeft - TurnsLeft) / Convert.ToSingle(c_alertTurnsLeft);
                intensity = Mathf.Clamp01(intensity);
            }
            else
            {
                intensity = 0.0f; //stop alert
            }
            ExecuteEvents.Execute<ICameraAlertTarget>(m_cameraObj, null, (x, y) => x.OnAlert(intensity));
        }

        private void TriggerResults()
        {
            if (Combos > 1)
            {
                m_statistics.Combos++;
            }
            m_statistics.BiggestCombo = Math.Max(m_statistics.BiggestCombo, Combos);

            //meh - need own won/lost hooks
            if (IsGameWon())
            {
                m_statistics.End(StatisticsManager.Finish.WON, Points);
                m_statistics.Report();
            }
            if (IsGameLost())
            {
                m_statistics.End(StatisticsManager.Finish.LOST, Points);
            }
        }
    }
}
