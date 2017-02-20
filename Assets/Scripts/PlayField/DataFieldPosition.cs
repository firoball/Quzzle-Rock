using UnityEngine;
using System;
using System.Collections;

namespace Assets.Scripts.Structs
{

    public struct DataFieldPosition
    {
        private int m_column;
        private int m_row;

        public DataFieldPosition(int column, int row)
        {
            m_column = column;
            m_row = row;
        }

        public bool IsNeighbourOf (DataFieldPosition position)
        {
            //no over- / underflow protection done.
            if (
                ((Math.Abs(Column - position.Column) <= 1) && (Row == position.Row)) ||
                ((Math.Abs(Row - position.Row) <= 1) && (Column == position.Column))
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(DataFieldPosition position1, DataFieldPosition position2)
        {
            if(
                (position1.Column != position2.Column) ||
                (position1.Row != position2.Row)
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(DataFieldPosition position1, DataFieldPosition position2)
        {
            if (position1 != position2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DataFieldPosition))
            {
                return false;
            }

            DataFieldPosition pos = (DataFieldPosition)obj;

            if (pos == this)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + m_column.GetHashCode();
            hash = (hash * 7) + m_row.GetHashCode();

            return hash;
        }

        public int Column
        {
            get
            {
                return m_column;
            }

            set
            {
                m_column = value;
            }
        }

        public int Row
        {
            get
            {
                return m_row;
            }

            set
            {
                m_row = value;
            }
        }

    }
}
