namespace Assets.Scripts.Structs
{

    public struct DataFieldSize
    {
        private int m_columns;
        private int m_rows;

        public DataFieldSize(int columns, int rows)
        {
            m_columns = columns;
            m_rows = rows;
        }

        public int Columns
        {
            get
            {
                return m_columns;
            }

            set
            {
                m_columns = value;
            }
        }

        public int Rows
        {
            get
            {
                return m_rows;
            }

            set
            {
                m_rows = value;
            }
        }

    }
}
