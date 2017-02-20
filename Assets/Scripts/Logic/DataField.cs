using System.Collections.Generic;

namespace Game.Logic
{
    public class DataField<T>
    {
        private T[][] m_field;

        public DataField(DataFieldSize size)
        {
            int columns = size.Columns;
            int rows = size.Rows;
            if (columns < 1)
            {
                columns = 1;
            }
            if (rows < 1)
            {
                rows = 1;
            }
            m_field = new T[columns][];
            for (int i = 0; i < m_field.Length; i++)
            {
                m_field[i] = new T[rows];
            }
        }

        public T[][] Field
        {
            get
            {
                return m_field;
            }

            set
            {
                m_field = value;
            }
        }

        public int MinColumn
        {
            get { return 0; }
        }

        public int MaxColumn
        {
            get { return m_field.Length - 1; }
        }

        public int MinRow
        {
            get { return 0; }
        }

        public int MaxRow
        {
            get { return m_field[0].Length - 1; }
        }

        public T GetFromPosition(DataFieldPosition position)
        {
            if (Contains(position))
            {
                return m_field[position.Column][position.Row];
            }
            else
            {
                return default(T);
            }
        }

        public void SetAtPosition(DataFieldPosition position, T content)
        {
            if (Contains(position))
            {
                m_field[position.Column][position.Row] = content;
            }
        }

        public bool Find(T content, out DataFieldPosition position)
        {
            position = new DataFieldPosition(int.MaxValue, int.MaxValue);
            bool found = false;

            if (content != null)
            {
                for (int column = 0; column < m_field.Length && !found; column++)
                {
                    for (int row = 0; row < m_field[column].Length && !found; row++)
                    {
                        if (EqualityComparer<T>.Default.Equals(m_field[column][row], content))
                        //if (m_field[column][row] == content)
                        {
                            found = true;
                            position.Column = column;
                            position.Row = row;
                        }
                    }
                }
            }

            return found;
        }

        public DataFieldSize Size()
        {
            return new DataFieldSize(m_field.Length, m_field[0].Length);
        }

        public bool Contains(DataFieldPosition position)
        {
            if (
                (position.Column >= 0) && (position.Column < m_field.Length) &&
                (position.Row >= 0) && (position.Row < m_field[0].Length)
                )
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void Clear()
        {
            for (int column = 0; column < m_field.Length; column++)
            {
                for (int row = 0; row < m_field[column].Length; row++)
                {
                    m_field[column][row] = default(T);
                }
            }
        }

        public DataField<T> Copy()
        {
            //this is somewhat rancid and in no way fast...
            //will only do a shallow copy
            DataField<T> copy = new DataField<T>(Size());

            for (int column = 0; column < m_field.Length; column++)
            {
                for (int row = 0; row < m_field[column].Length; row++)
                {
                    DataFieldPosition position = new DataFieldPosition(column, row);
                    T content = GetFromPosition(position);
                    copy.SetAtPosition(position, content);
                }
            }
            return copy;
        }
    }
}
