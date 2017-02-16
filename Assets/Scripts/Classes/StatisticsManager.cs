using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using Assets.Scripts.Classes;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class StatisticsManager
    {

        private GameStatistics m_game;
        private ModeStatistics m_mode;
        private GlobalStatistics m_global;
        private GameObject m_resultUI;
        private string m_modeIdentifier;
        private bool m_started;

        public int Combos
        {
            get
            {
                return m_game.Combos;
            }

            set
            {
                if (m_started)
                {
                    m_game.Combos = value;
                }
            }
        }

        public int BiggestCombo
        {
            get
            {
                return m_game.BiggestCombo;
            }

            set
            {
                if (m_started)
                {
                    m_game.BiggestCombo = value;
                }
            }
        }

        public int Turns
        {
            get
            {
                return m_game.Turns;
            }

            set
            {
                if (m_started)
                {
                    m_game.Turns = value;
                }
            }
        }

        public int ExtraTurns
        {
            get
            {
                return m_game.ExtraTurns;
            }

            set
            {
                if (m_started)
                {
                    m_game.ExtraTurns = value;
                }
            }
        }

        public StatisticsManager(GameObject resultUI, string name)
        {
            m_game = new GameStatistics();
            m_global = new GlobalStatistics();
            m_mode = new ModeStatistics();

            m_resultUI = resultUI;
            m_modeIdentifier = name.ToLower();
            m_global.Load();
            m_mode.Load(m_modeIdentifier);
            m_started = false;
        }

        public void Begin()
        {
            m_started = true;

            //only game statistics are initialized on start of new game round!
            m_game.Turns = 0;
            m_game.ExtraTurns = 0;
            m_game.Combos = 0;
            m_game.BiggestCombo = 0;
            m_game.PlayTime = Time.time;

            ExecuteEvents.Execute<IResultEventTarget>(m_resultUI, null, (x, y) => x.OnReset());
        }

        public void Report()
        {
            Dictionary<string, string> results;

            results = m_game.Report();
            ExecuteEvents.Execute<IResultEventTarget>(m_resultUI, null, (x, y) => x.OnAddResults(results));

            results = m_mode.Report();
            ExecuteEvents.Execute<IResultEventTarget>(m_resultUI, null, (x, y) => x.OnAddResults(results));

            results = m_global.Report();
            ExecuteEvents.Execute<IResultEventTarget>(m_resultUI, null, (x, y) => x.OnAddResults(results));
        }

        public void End(Finish finish, int score)
        {
            if (!m_started)
            {
                return;
            }
            m_started = false;

            //update game statistics (don't need saving)
            m_game.PlayTime = Time.time - m_game.PlayTime;

            //update global statistics
            if (finish == Finish.ABORTED)
            {
                m_global.GamesAborted++;
            }
            m_global.GamesPlayed++;
            m_global.PlayTime += m_game.PlayTime;
            //save
            m_global.Save();

            //update mode statistics
            if (finish == Finish.WON)
            {
                m_mode.GamesWon++;
                m_mode.GamesWonInRow++;
            }
            if (finish == Finish.LOST)
            {
                m_mode.GamesLost++;
                m_mode.GamesWonInRow = 0;
            }
            if (finish != Finish.ABORTED)
            {
                m_mode.PlayTime = Mathf.Min(m_mode.PlayTime, m_game.PlayTime);
                m_mode.BestTurns = Math.Min(m_mode.BestTurns, m_game.Turns);
                m_mode.WorstScore = Math.Min(m_mode.WorstScore, score);
                //m_global.GamesPlayed is not used since it contains aborted games
                float finished = Convert.ToSingle(m_mode.GamesLost + m_mode.GamesWon);
                m_mode.TurnsAverage = (m_mode.TurnsAverage * (finished - 1.0f) + Convert.ToSingle(m_game.Turns)) / finished;
                m_mode.ScoreAverage = (m_mode.ScoreAverage * (finished - 1.0f) + Convert.ToSingle(score)) / finished;
            }
            m_mode.GamesPlayed++;
            m_mode.BiggestCombo = Math.Max(m_mode.BiggestCombo, m_game.BiggestCombo);
            m_mode.Combos = Math.Max(m_mode.Combos, m_game.Combos);
            m_mode.BestExtraTurns = Math.Max(m_mode.BestExtraTurns, m_game.ExtraTurns);
            //save
            m_mode.Save(m_modeIdentifier);

            PlayerPrefs.Save();
        }

        public enum Finish : int
        {
            WON = 0,
            LOST = 1,
            ABORTED = 2
        }
    }
}
