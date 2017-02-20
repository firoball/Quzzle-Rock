using System.Collections.Generic;

namespace Game.Logic
{

    public struct Combination
    {
        private int m_id;
        private List<DataFieldPosition> m_positions;

        public Combination(int id)
        {
            m_id = id;
            m_positions = new List<DataFieldPosition>();
        }

        public List<DataFieldPosition> Positions
        {
            get
            {
                return m_positions;
            }
        }

        public int Id
        {
            get
            {
                return m_id;
            }

            set
            {
                m_id = value;
                if (m_positions == null)
                {
                    m_positions = new List<DataFieldPosition>();
                }
            }
        }

    }
}
