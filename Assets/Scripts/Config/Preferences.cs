using UnityEngine;
using System;
namespace Game.Config
{
    [Serializable]
    public class Preferences
    {
        private static Preferences s_current = new Preferences();

        [SerializeField]
        private string m_name;
        [SerializeField][Range(1, 20)]
        private int m_columnCount = 8;
        [SerializeField][Range(1, 20)]
        private int m_rowCount = 8;
        [SerializeField][Range(3, 10)]
        private int m_tokenCount = 4;//6;
        [SerializeField]
        private int m_moveCount = 7;//20;
        [SerializeField]
        private int m_targetCount = 3;//60;

        public string Name
        {
            get
            {
                return m_name;
            }

            set
            {
                m_name = value;
            }
        }

        public int ColumnCount
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

        public int RowCount
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

        public int TokenCount
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

        public int MoveCount
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

        public int TargetCount
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

        public static Preferences Current
        {
            get
            {
                return s_current;
            }

            set
            {
                s_current = value;
            }
        }

        public Preferences()
        {
            m_name = ToString();
        }

        public override string ToString()
        {
            string str = "";
            str += "f" + m_columnCount + "x" + m_rowCount + "x" + m_tokenCount;
            str += "m" + m_moveCount + "t" + m_targetCount;
            return str;
        }
    }
}
