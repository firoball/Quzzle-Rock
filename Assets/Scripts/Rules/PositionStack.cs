using UnityEngine;
using System;
using Game.Config;
using Game.Logic;

namespace Game.Rules
{
    public class PositionStack
    {
        private int m_index;
        private int m_columnCount;
        private int m_rowCount;
        private DataFieldPosition[] m_positionStack;
        private System.Random m_rand = new System.Random();

        private int m_stackSize;
        private bool m_debug;

        public PositionStack(int stackSize) : this(stackSize, false)
        {

        }

        public PositionStack() : this(100, false)
        {

        }

        public PositionStack(int stackSize, bool debug)
        {
            m_stackSize = stackSize;
            m_debug = debug;
            m_columnCount = Preferences.Current.ColumnCount;
            m_rowCount = Preferences.Current.RowCount;
            m_positionStack = new DataFieldPosition[m_stackSize];

            Restart();
            GenerateTokenPositions();

            if (m_debug)
            {
                Log();
            }
        }

        public DataFieldPosition GetNextPosition()
        {
            //current index
            int index = m_index;
            //next index (cycling)
            m_index = (index + 1) % m_positionStack.Length;
            return m_positionStack[index];
        }

        public void Restart()
        {
            m_index = 0;
        }

        private void Log()
        {
            string log = "PositionStack (" + Convert.ToString(m_stackSize) + "):\n";
            for (int i = m_stackSize - 1; i >= 0; i--)
            {
                string row = Convert.ToString(i);
                row.PadLeft(2, ' ');
                log += row + "\t";
                string token = Convert.ToString(m_positionStack[i].Column) +"/" + 
                    Convert.ToString(m_positionStack[i].Row);
                log += token;
            }
            log += "\n";

            Debug.Log(log);
        }

        private void GenerateTokenPositions()
        {
            for (int i = 0; i < m_stackSize; i++)
            {
                int column = m_rand.Next(0, m_columnCount);
                int row = m_rand.Next(0, m_rowCount);
                m_positionStack[i] = new DataFieldPosition(column, row);
            }

        }

    }
}
