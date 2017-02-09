using UnityEngine;

namespace Assets.Scripts.Structs
{

    public struct ModeStatistics
    {
        private int m_gamesPlayed;
        private int m_gamesWon;
        private int m_gamesWonInRow;
        private int m_gamesLost;
        private int m_gamesAborted;
        private int m_combos;
        private int m_biggestCombo;
        private int m_worstScore;
        private int m_bestScore;
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

        public int GamesAborted
        {
            get
            {
                return m_gamesAborted;
            }

            set
            {
                m_gamesAborted = value;
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

        public int BestScore
        {
            get
            {
                return m_bestScore;
            }

            set
            {
                m_bestScore = value;
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
            PlayerPrefs.SetInt(mode + ".gamesPlayed", m_gamesPlayed);
            PlayerPrefs.SetInt(mode + ".gamesWon", m_gamesWon);
            PlayerPrefs.SetInt(mode + ".gamesWonInRow", m_gamesWonInRow);
            PlayerPrefs.SetInt(mode + ".gamesLost", m_gamesLost);
            PlayerPrefs.SetInt(mode + ".gamesAborted", m_gamesAborted);
            PlayerPrefs.SetInt(mode + ".combos", m_combos);
            PlayerPrefs.SetInt(mode + ".biggestCombo", m_biggestCombo);
            PlayerPrefs.SetInt(mode + ".worstScore", m_worstScore);
            PlayerPrefs.SetInt(mode + ".bestScore", m_bestScore);
            PlayerPrefs.SetInt(mode + ".bestTurns", m_bestTurns);
            PlayerPrefs.SetInt(mode + ".bestExtraTurns", m_bestExtraTurns);
            PlayerPrefs.SetFloat(mode + ".scoreAverage", m_scoreAverage);
            PlayerPrefs.SetFloat(mode + ".turnsAverage", m_turnsAverage);
            PlayerPrefs.SetFloat(mode + ".playTime", m_playTime);
        }

        public void Load(string mode)
        {
            m_gamesPlayed = PlayerPrefs.GetInt(mode + ".gamesPlayed", 0);
            m_gamesWon = PlayerPrefs.GetInt(mode + ".gamesWon", 0);
            m_gamesWonInRow = PlayerPrefs.GetInt(mode + ".gamesWonInRow", 0);
            m_gamesLost = PlayerPrefs.GetInt(mode + ".gamesLost", 0);
            m_gamesAborted = PlayerPrefs.GetInt(mode + ".gamesAborted", 0);
            m_combos = PlayerPrefs.GetInt(mode + ".combos", 0);
            m_biggestCombo = PlayerPrefs.GetInt(mode + ".biggestCombo", 0);
            m_worstScore = PlayerPrefs.GetInt(mode + ".worstScore", int.MaxValue);
            m_bestScore = PlayerPrefs.GetInt(mode + ".bestScore", 0);
            m_bestTurns = PlayerPrefs.GetInt(mode + ".bestTurns", int.MaxValue);
            m_bestExtraTurns = PlayerPrefs.GetInt(mode + ".bestExtraTurns", 0);
            m_scoreAverage = PlayerPrefs.GetFloat(mode + ".scoreAverage", 0.0f);
            m_turnsAverage = PlayerPrefs.GetFloat(mode + ".turnsAverage", 0.0f);
            m_playTime = PlayerPrefs.GetFloat(mode + ".playTime", 0.0f);
        }

        #endregion
    }
}
