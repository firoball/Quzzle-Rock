using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.Statistics
{
    [Serializable]
    public class ModeStatistics
    {
        private int m_gamesPlayed;
        private int m_gamesWon;
        private int m_gamesWonInRow;
        private int m_gamesLost;
        private int m_combos;
        private int m_biggestCombo;
        private int m_worstScore;
        private int m_bestTurns;
        private int m_bestExtraTurns;
        private float m_scoreAverage;
        private float m_turnsAverage;
        private float m_playTime;

        #region properties

        public int GamesPlayed
        {
            get
            {
                return m_gamesPlayed;
            }

            set
            {
                m_gamesPlayed = value;
            }
        }

        public int GamesWon
        {
            get
            {
                return m_gamesWon;
            }

            set
            {
                m_gamesWon = value;
            }
        }

        public int GamesWonInRow
        {
            get
            {
                return m_gamesWonInRow;
            }

            set
            {
                m_gamesWonInRow = value;
            }
        }

        public int GamesLost
        {
            get
            {
                return m_gamesLost;
            }

            set
            {
                m_gamesLost = value;
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
            }
        }

        public int BiggestCombo
        {
            get
            {
                return m_biggestCombo;
            }

            set
            {
                m_biggestCombo = value;
            }
        }

        public int WorstScore
        {
            get
            {
                return m_worstScore;
            }

            set
            {
                m_worstScore = value;
            }
        }

        public int BestTurns
        {
            get
            {
                return m_bestTurns;
            }

            set
            {
                m_bestTurns = value;
            }
        }

        public int BestExtraTurns
        {
            get
            {
                return m_bestExtraTurns;
            }

            set
            {
                m_bestExtraTurns = value;
            }
        }

        public float ScoreAverage
        {
            get
            {
                return m_scoreAverage;
            }

            set
            {
                m_scoreAverage = value;
            }
        }

        public float TurnsAverage
        {
            get
            {
                return m_turnsAverage;
            }

            set
            {
                m_turnsAverage = value;
            }
        }

        public float PlayTime
        {
            get
            {
                return m_playTime;
            }

            set
            {
                m_playTime = value;
            }
        }

        #endregion

        #region public functions

        public void Save(string mode)
        {
            PlayerPrefs.SetInt("mode." + mode + ".gamesPlayed", m_gamesPlayed);
            PlayerPrefs.SetInt("mode." + mode + ".gamesWon", m_gamesWon);
            PlayerPrefs.SetInt("mode." + mode + ".gamesWonInRow", m_gamesWonInRow);
            PlayerPrefs.SetInt("mode." + mode + ".gamesLost", m_gamesLost);
            PlayerPrefs.SetInt("mode." + mode + ".combos", m_combos);
            PlayerPrefs.SetInt("mode." + mode + ".biggestCombo", m_biggestCombo);
            PlayerPrefs.SetInt("mode." + mode + ".worstScore", m_worstScore);
            PlayerPrefs.SetInt("mode." + mode + ".bestTurns", m_bestTurns);
            PlayerPrefs.SetInt("mode." + mode + ".bestExtraTurns", m_bestExtraTurns);
            PlayerPrefs.SetFloat("mode." + mode + ".scoreAverage", m_scoreAverage);
            PlayerPrefs.SetFloat("mode." + mode + ".turnsAverage", m_turnsAverage);
            PlayerPrefs.SetFloat("mode." + mode + ".playTime", m_playTime);
        }

        public void Load(string mode)
        {
            m_gamesPlayed = PlayerPrefs.GetInt("mode." + mode + ".gamesPlayed", 0);
            m_gamesWon = PlayerPrefs.GetInt("mode." + mode + ".gamesWon", 0);
            m_gamesWonInRow = PlayerPrefs.GetInt("mode." + mode + ".gamesWonInRow", 0);
            m_gamesLost = PlayerPrefs.GetInt("mode." + mode + ".gamesLost", 0);
            m_combos = PlayerPrefs.GetInt("mode." + mode + ".combos", 0);
            m_biggestCombo = PlayerPrefs.GetInt("mode." + mode + ".biggestCombo", 0);
            m_worstScore = PlayerPrefs.GetInt("mode." + mode + ".worstScore", int.MaxValue);
            m_bestTurns = PlayerPrefs.GetInt("mode." + mode + ".bestTurns", int.MaxValue);
            m_bestExtraTurns = PlayerPrefs.GetInt("mode." + mode + ".bestExtraTurns", 0);
            m_scoreAverage = PlayerPrefs.GetFloat("mode." + mode + ".scoreAverage", 0.0f);
            m_turnsAverage = PlayerPrefs.GetFloat("mode." + mode + ".turnsAverage", 0.0f);
            m_playTime = PlayerPrefs.GetFloat("mode." + mode + ".playTime", float.MaxValue);
        }

        public Dictionary<string, string> Report()
        {
            //these strings should not be hardcoded, but let's get this done
            Dictionary<string, string> results = new Dictionary<string, string>();
            string key;
            string value;
            float percent;
            int rounded;

            /* Not all statistic data is calculated if game was aborted.
             * This would result in default garbage values if first game ever was aborted.
             * Mainly the "best of/fewest" statistics are affected by this.
             * Make sure that at least game was fully finished once, before a full report is generated.
             */
            if ((m_gamesWon > 0) || (m_gamesLost > 0))
            {
                key = "Wins In Series";
                value = m_gamesWonInRow.ToString();
                results.Add(key, value);

                key = "Win Ratio";
                percent = 100.0f * (Convert.ToSingle(m_gamesWon) / Convert.ToSingle(m_gamesPlayed));
                value = percent.ToString("F1") + "%";
                results.Add(key, value);

                key = "Lose Ratio";
                percent = 100.0f * (Convert.ToSingle(m_gamesLost) / Convert.ToSingle(m_gamesPlayed));
                value = percent.ToString("F1") + "%";
                results.Add(key, value);

                key = "Average Turns";
                rounded = Convert.ToInt32(m_turnsAverage);
                value = rounded.ToString();
                results.Add(key, value);

                key = "Average Score";
                rounded = Convert.ToInt32(m_scoreAverage);
                value = rounded.ToString();
                results.Add(key, value);

                key = "Worst Score";
                value = m_worstScore.ToString();
                results.Add(key, value);

                key = "Fastest Time";
                TimeSpan time = TimeSpan.FromSeconds(Convert.ToDouble(m_playTime));
                value = string.Format("{0}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
                results.Add(key, value);

                key = "Least Turns";
                value = m_bestTurns.ToString();
                results.Add(key, value);

                key = "Max Extra Turns";
                value = m_bestExtraTurns.ToString();
                results.Add(key, value);

                key = "Max Combos";
                value = m_combos.ToString();
                results.Add(key, value);

                key = "Max Combo Size";
                value = m_biggestCombo.ToString();
                results.Add(key, value);
            }
            else
            {
                results.Add("No data.", "");
            }

            return results;
        }

        #endregion
    }
}
