using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Static
{
    class Preferences
    {
        private static int m_columnCount = 8;
        private static int m_rowCount = 8;
        private static int m_tokenCount = 6;
        private static int m_moveCount = 20;
        private static int m_targetCount = 30;

        public static int ColumnCount
        {
            get
            {
                return m_columnCount;
            }

            set
            {
                m_columnCount = value;
            }
        }

        public static int RowCount
        {
            get
            {
                return m_rowCount;
            }

            set
            {
                m_rowCount = value;
            }
        }

        public static int TokenCount
        {
            get
            {
                return m_tokenCount;
            }

            set
            {
                m_tokenCount = value;
            }
        }

        public static int MoveCount
        {
            get
            {
                return m_moveCount;
            }

            set
            {
                m_moveCount = value;
            }
        }

        public static int TargetCount
        {
            get
            {
                return m_targetCount;
            }

            set
            {
                m_targetCount = value;
            }
        }
    }
}
