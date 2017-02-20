using System;
using System.Collections.Generic;

namespace Game.Statistics
{

    public class GameStatistics
    {
        private int m_turns;
        private int m_extraTurns;
        private int m_combos;
        private int m_biggestCombo;
        private float m_playTime;

        public int Turns
        {
            get
            {
                return m_turns;
            }

            set
            {
                m_turns = value;
            }
        }

        public int ExtraTurns
        {
            get
            {
                return m_extraTurns;
            }

            set
            {
                m_extraTurns = value;
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

        public Dictionary<string, string> Report()
        {
            //these strings should not be hardcoded, but let's get this done
            Dictionary<string, string> results = new Dictionary<string, string>();
            string key;
            string value;

            key = "Play Time";
            TimeSpan time = TimeSpan.FromSeconds(Convert.ToDouble(m_playTime));
            value = string.Format("{0}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
            results.Add(key, value);

            key = "Total Turns";
            value = m_turns.ToString();
            results.Add(key, value);

            key = "Extra Turns";
            value = m_extraTurns.ToString();
            results.Add(key, value);

            key = "Combos";
            value = m_combos.ToString();
            results.Add(key, value);

            key = "Longest Combo";
            value = m_biggestCombo.ToString();
            results.Add(key, value);

            return results;
        }

    }
}
