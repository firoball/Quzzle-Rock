using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Scripts.Static;

namespace Assets.Scripts.Classes
{
    public class TokenStack
    {
        private List<int>[] m_columns;

        private int m_columnCount;
        private int m_tokenCount;
        private int[] m_columnIndex;
        private System.Random m_rand = new System.Random();

        private int m_stackSize;
        private bool m_debug;

        public TokenStack(int stackSize) : this (stackSize, false)
        {

        }

        public TokenStack() : this(100, false)
        {

        }

        public TokenStack(int stackSize, bool debug)
        {
            m_stackSize = stackSize;
            m_debug = debug;

            m_columnCount = Preferences.ColumnCount;
            m_tokenCount = Preferences.TokenCount;

            m_columnIndex = new int[m_columnCount];
            Restart();

            m_columns = new List<int>[m_columnCount];
            for (int i = 0; i < m_columns.Length; i++)
            {
                m_columns[i] = new List<int>(m_stackSize);
                GenerateTokenIds(m_columns[i]);
            }

            if (m_debug)
            {
                Log();
            }
        }

        public int GetNextTokenId(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex > m_columns.Length)
            {
                return -1;
            }

            List<int> columnStack = m_columns[columnIndex];
            //current index
            int index = m_columnIndex[columnIndex];
            //next index (cycling)
            m_columnIndex[columnIndex] = (index + 1) % columnStack.Count;
            return columnStack[index];
        }

        public void Restart()
        {
            for (int i = 0; i < m_columnIndex.Length; i++)
            {
                m_columnIndex[i] = 0;
            }
        }

        private void Log()
        {
            string log = "TokenStack (" + Convert.ToString(m_stackSize) + "):\n";
            for (int i = m_stackSize - 1; i >= 0; i--)
            {
                string row = Convert.ToString(i);
                row.PadLeft(2, ' ');
                log += row + "\t";
                foreach (List<int> column in m_columns)
                {
                    string token = Convert.ToString(column[i]);
                    log += token;
                }
                log += "\n";
            }
            Debug.Log(log);
        }

        private void GenerateTokenIds(List<int> column)
        {
            for (int i = 0; i < column.Capacity; i++)
            {
                int token = m_rand.Next(0, m_tokenCount);
                column.Add(token);
            }

        }

    }
}
