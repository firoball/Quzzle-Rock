namespace Assets.Scripts.Structs
{

    public struct GameStatistics
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
    }
}
