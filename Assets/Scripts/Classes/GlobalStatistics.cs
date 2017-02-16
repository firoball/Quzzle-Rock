using UnityEngine;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Classes
{

    public class GlobalStatistics
    {
        private int m_gamesPlayed;
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
            PlayerPrefs.SetInt("global.gamesAborted", m_gamesAborted);
            PlayerPrefs.SetFloat("global.playTime", m_playTime);
        }

        public void Load()
        {
            m_gamesPlayed = PlayerPrefs.GetInt("global.gamesPlayed", 0);
            m_gamesAborted = PlayerPrefs.GetInt("global.gamesAborted", 0);
            m_playTime = PlayerPrefs.GetFloat("global.playTime", 0.0f);
        }

        public Dictionary<string, string> Report()
        {
            //these strings should not be hardcoded, but let's get this done
            Dictionary<string, string> results = new Dictionary<string, string>();
            string key;
            string value;

            /* avoid NaN  due to div/0 when no game was played at all.
             * Make sure that at least game was fully finished once, before a full report is generated.
             */
            if (m_gamesPlayed > 0)
            {
                key = "Total Time";
                TimeSpan time = TimeSpan.FromSeconds(Convert.ToDouble(m_playTime));
                value = string.Format("{0}d {1}:{2:D2}", time.Days, time.Hours, time.Minutes);
                results.Add(key, value);

                key = "Games Played";
                value = m_gamesPlayed.ToString();
                results.Add(key, value);

                key = "Abort Ratio";
                float percent = 100.0f * (Convert.ToSingle(m_gamesAborted) / Convert.ToSingle(m_gamesPlayed));
                value = percent.ToString("F1") + "%";
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
