using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Structs;

namespace Assets.Scripts.Classes
{
    public class CombinationFinder
    {
        private List<Combination> m_combinations;
        private DataField<int> m_idField;
        private bool m_debug;

        public CombinationFinder(DataField<int> idField) : this(idField, false)
        {
        }

        public CombinationFinder(DataField<int> idField, bool debug)
        {
            m_combinations = new List<Combination>();
            m_idField = idField;
            m_debug = debug;
        }

        public List<Combination> Combinations
        {
            get
            {
                return m_combinations;
            }
        }

        public bool Find(int minLength)
        {
            bool found = false;

            if (m_idField != null)
            {
                DataFieldSize size = m_idField.Size();
                //process columns
                for (int column = 0; column < size.Columns; column++)
                {
                    int lastId = -1;
                    Combination combination = new Combination(lastId); //please compiler
                    for (int row = 0; row < size.Rows; row++)
                    {
                        lastId = Store(lastId, column, row, ref combination);
                    }
                }

                //process rows
                for (int row = 0; row < size.Rows; row++)
                {
                    int lastId = -1;
                    Combination combination = new Combination(lastId); //please compiler
                    for (int column = 0; column < size.Columns; column++)
                    {
                        lastId = Store(lastId, column, row, ref combination);
                    }
                }
                if (Filter(minLength) > 0)
                {
                    found = true;
                }

                if (m_debug)
                {
                    Log();
                }
            }

            return found;
        }

        public void Clear()
        {
            m_combinations.Clear();
        }

        private int Filter(int minLength)
        {
            for (int i = m_combinations.Count - 1; i >= 0; i--)
            {
                //remove all combinations which are too short or are empty (id = -1)
                if ((m_combinations[i].Positions.Count < minLength) || (m_combinations[i].Id == -1))
                {
                    m_combinations.RemoveAt(i);
                }
            }
            return m_combinations.Count;
        }

        private int Store(int lastId, int column, int row, ref Combination combination)
        {
            DataFieldPosition position = new DataFieldPosition(column, row);
            int id = m_idField.GetFromPosition(position);
            if (id != lastId)
            {
                //start new combination
                combination = new Combination(id);
                combination.Positions.Add(position);
                m_combinations.Add(combination);
                lastId = id;
            }
            else
            {
                //append to last combination
                combination.Positions.Add(position);
            }
            return lastId;
        }

        private void Log()
        {
            string str = "CombinationFinder";
            foreach (Combination combination in m_combinations)
            {
                str += "\n" + combination.Positions[0].Column + "/" + combination.Positions[0].Row
                    + " id: " + combination.Id + " size: " + combination.Positions.Count;
            }
            Debug.Log(str);
        }


    }
}
