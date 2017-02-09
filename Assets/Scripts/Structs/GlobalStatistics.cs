using UnityEngine;

namespace Assets.Scripts.Structs
{

    public struct GlobalStatistics
    {
        private int m_gamesPlayed;
        private int m_gamesWon;
        private int m_gamesLost;
        private int m_gamesAborted;
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

        public void Save()
        {
            PlayerPrefs.SetInt("global.gamesPlayed", m_gamesPlayed);
            PlayerPrefs.SetInt("global.gamesWon", m_gamesWon);
            PlayerPrefs.SetInt("global.gamesLost", m_gamesLost);
            PlayerPrefs.SetInt("global.gamesAborted", m_gamesAborted);
            PlayerPrefs.SetFloat("global.playTime", m_playTime);
        }

        public void Load()
        {
            m_gamesPlayed = PlayerPrefs.GetInt("global.gamesPlayed", 0);
            m_gamesWon = PlayerPrefs.GetInt("global.gamesWon", 0);
            m_gamesLost = PlayerPrefs.GetInt("global.gamesLost", 0);
            m_gamesAborted = PlayerPrefs.GetInt("global.gamesAborted", 0);
            m_playTime = PlayerPrefs.GetFloat("global.playTime", 0.0f);
        }

        #endregion

    }
}
