using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Assets.Scripts.Structs;
using Assets.Scripts.Classes;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class RuleSetSmash : RuleSet
    {
        [SerializeField]
        [Range(10, 1000)]
        private int m_stackSize = 50;
        [SerializeField]
        private bool m_debug = false;
        [SerializeField]
        private GameObject m_levelHudUI;
        [SerializeField]
        private GameObject m_combo4ToolTip;
        [SerializeField]
        private GameObject m_combo5ToolTip;

        private int m_turnsLeft;
        private int m_combos;
        private int m_points;
        private int m_extraTurns;
        private PositionStack m_positionStack;

        private int m_turnsMax;
        private int m_pointsMax;

        private const int c_specialTokenId = 100;

        public int TurnsLeft
        {
            get
            {
                return m_turnsLeft;
            }

            set
            {
                m_turnsLeft = value;
                if (m_levelHudUI != null)
                {
                    ExecuteEvents.Execute<IHudEventTarget>(m_levelHudUI, null, (x, y) => x.OnUpdateTurns(m_turnsLeft));
                }
            }
        }

        public int Combos
        {
            get
            {
                return m_combos;
            }

            set
            {
                m_combos = value;
                if (m_levelHudUI != null)
                {
                    ExecuteEvents.Execute<IHudEventTarget>(m_levelHudUI, null, (x, y) => x.OnUpdateCombos(m_combos));
                }
            }
        }

        public int Points
        {
            get
            {
                return m_points;
            }

            set
            {
                m_points = value;
                if (m_levelHudUI != null)
                {
                    ExecuteEvents.Execute<IHudEventTarget>(m_levelHudUI, null, (x, y) => x.OnUpdateScore(m_points));
                }
            }
        }

        public int TurnsMax
        {
            get
            {
                return m_turnsMax;
            }

            set
            {
                m_turnsMax = value;
                if (m_levelHudUI != null)
                {
                    ExecuteEvents.Execute<IHudEventTarget>(m_levelHudUI, null, (x, y) => x.OnUpdateTurnsMax(m_turnsMax));
                }
            }
        }

        public int PointsMax
        {
            get
            {
                return m_pointsMax;
            }

            set
            {
                m_pointsMax = value;
                if (m_levelHudUI != null)
                {
                    ExecuteEvents.Execute<IHudEventTarget>(m_levelHudUI, null, (x, y) => x.OnUpdateScoreMax(m_pointsMax));
                }
            }
        }

        void Start()
        {
            m_positionStack = new PositionStack(m_stackSize, m_debug);
            TurnsMax = Preferences.Current.MoveCount;
            PointsMax = Preferences.Current.TargetCount;
            Restart();
        }

        public override void ProcessCombo(List<Combination> combinationList)
        {
            foreach (Combination combination in combinationList)
            {
                if (combination.Id == c_specialTokenId)
                {
                    Points = Math.Min(Points + combination.Positions.Count, PointsMax);
                }
                if (combination.Positions.Count == 4)
                {
                    SpawnLabelForCombination(m_combo4ToolTip, combination);
                    m_extraTurns++;
                }
                else if(combination.Positions.Count > 4)
                {
                    SpawnLabelForCombination(m_combo5ToolTip, combination);
                    m_extraTurns += 2;
                }
            }
            Combos++;
        }

        public override void TurnStart()
        {
            Combos = 0;
            m_extraTurns = 0;
        }

        public override void TurnEnd()
        {
            TurnsLeft = Math.Min(TurnsLeft + m_extraTurns - 1, TurnsMax);
            Combos = 0;
        }

        public override void Restart()
        {
            TurnsLeft = TurnsMax;
            Points = 0;
            Combos = 0;
            m_positionStack.Restart();
        }

        public override bool GameLost()
        {
            if (TurnsLeft == 0)
            {
                if (m_levelHudUI != null)
                {
                    ExecuteEvents.Execute<IHudEventTarget>(m_levelHudUI, null, (x, y) => x.OnGameEnded(false));
                }
                return true;
            }
            return false;
        }

        public override bool GameWon()
        {
            if (Points >= PointsMax)
            {
                if (m_levelHudUI != null)
                {
                    ExecuteEvents.Execute<IHudEventTarget>(m_levelHudUI, null, (x, y) => x.OnGameEnded(true));
                }
                return true;
            }
            return false;
        }

        public override void PlayFieldModifier()
        {
            //trigger every 4th combo
            if ((Combos % 4) == 0)
            {
                Dictionary<DataFieldPosition, int> tokenDict = new Dictionary<DataFieldPosition, int>();
                for (int i = 0; i < 8; i++)
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
    }
}
