using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Structs;
using Assets.Scripts.Classes;

namespace Assets.Scripts.Behaviours
{
    public class RuleSetSmash : RuleSet
    {
        [SerializeField]
        [Range(10, 1000)]
        private int m_stackSize = 50;
        [SerializeField]
        private bool m_debug = false;

        private int m_turnsLeft;
        private int m_combos;
        private int m_points;
        private int m_extraTurns;
        private PositionStack m_positionStack;

        private int m_turnsMax;
        private int m_pointsMax;

        private const int c_specialTokenId = 100;

        void Awake()
        {
            m_positionStack = new PositionStack(m_stackSize, m_debug);
            m_turnsMax = 1;// Preferences.Current.MoveCount;
            m_pointsMax = Preferences.Current.TargetCount;
            Restart();
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 100, 20), "Turns left: " + m_turnsLeft);
            GUI.Label(new Rect(10, 30, 100, 20), "Points: " + m_points + " / " + m_pointsMax);
            if (m_combos > 0)
            {
                GUI.Label(new Rect(10, 50, 100, 20), "Combos: " + m_combos);
            }
        }

        public override void ProcessCombo(List<Combination> combinationList)
        {
            foreach (Combination combination in combinationList)
            {
                if (combination.Id == c_specialTokenId)
                {
                    m_points = Math.Min(m_points + combination.Positions.Count, m_pointsMax);
                }
                if (combination.Positions.Count == 4)
                {
                    m_extraTurns++;
                }
                else if(combination.Positions.Count > 4)
                {
                    m_extraTurns += 2;
                }
            }
            m_combos++;
        }

        public override void TurnStart()
        {
            m_combos = 0;
            m_extraTurns = 0;
        }

        public override void TurnEnd()
        {
            m_turnsLeft = Math.Min(m_turnsLeft + m_extraTurns - 1, m_turnsMax);
            m_combos = 0;
        }

        public override void Restart()
        {
            m_turnsLeft = m_turnsMax;
            m_points = 0;
            m_positionStack.Restart();
        }

        public override bool GameLost()
        {
            if (m_turnsLeft == 0)
            {
                return true;
            }
            return false;
        }

        public override bool GameWon()
        {
            if (m_points >= m_pointsMax)
            {
                return true;
            }
            return false;
        }

        public override void PlayFieldModifier()
        {
            //trigger every 4th combo
            if ((m_combos % 4) == 0)
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

    }
}
