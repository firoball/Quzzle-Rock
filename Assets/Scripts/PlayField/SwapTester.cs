using System;
using Assets.Scripts.Structs;

namespace Assets.Scripts.Classes
{
    public class SwapTester
    {

        private DataField<int> m_idField;

        public SwapTester(DataField<int> idField)
        {
            m_idField = idField;
        }

        public bool IsValidSwap(DataFieldPosition position1, DataFieldPosition position2, int minLength)
        {
            bool valid = Validate(position1, position2, minLength) ||
                Validate(position2, position1, minLength);
            return valid;
        }

        private bool Validate(DataFieldPosition origin, DataFieldPosition partner, int minLength)
        {
            bool found = false;

            if (m_idField != null)
            {
                int from;
                int to;
                int length;

                //row
                from = Math.Max(0, origin.Column - (minLength - 1));
                to = Math.Min(origin.Column + (minLength - 1), m_idField.MaxColumn);
                length = 0;
                for (int column = from; (column <= to) && (length < minLength); column++)
                {
                    DataFieldPosition position = new DataFieldPosition(column, origin.Row);
                    length = FindMatch(origin, partner, position, length);
                    //Debug.Log("ori" + origin.Column + "from" +from+" "+"to"+to+"len"+length);
                }

                //                Debug.Log("isValidSwap: row match " + length + " row " + origin.Row);
                //no row match was found, try again with column...
                if (length < minLength)
                {
                    //column
                    from = Math.Max(0, origin.Row - (minLength - 1));
                    to = Math.Min(origin.Row + (minLength - 1), m_idField.MaxRow);
                    length = 0;
                    for (int row = from; (row <= to) && (length < minLength); row++)
                    {
                        DataFieldPosition position = new DataFieldPosition(origin.Column, row);
                        length = FindMatch(origin, partner, position, length);
                        //Debug.Log("ori"+ origin.Row+"from" + from + " " + "to" + to + "len" + length);
                    }

                    //Debug.Log("isValidSwap: col match " + length + " col " + origin.Column);
                }

                //minimum length was reached? We have a match
                if (length >= minLength)
                {
                    found = true;
                }
            }

            return found;
        }

        private int FindMatch(DataFieldPosition origin, DataFieldPosition partner, DataFieldPosition position, int length)
        {
            //ids are twisted since we need the playfield state AFTER swap action for validation
            int originId = m_idField.GetFromPosition(partner);
            int partnerId = m_idField.GetFromPosition(origin);
            int id;

            if ((originId == -1) || (partnerId == -1))
            {
                return 0;
            }

            /* check if current position is same as origin (swapped token) or
             * partner (swapped token partner) position.
             * as the swap was not yet performed, the playfield still has old data
             * at this position. 
             * The cheap trick is to handle those position separately and "patch" their new ids in.
             */
            if (position == origin)
            {
                id = originId;
            }
            else if (position == partner)
            {
                id = partnerId;
            }
            else
            {
                id = m_idField.GetFromPosition(position);
            }

            /* when id's don't match the length counter will be reset, otherwise incremented.
             * length can then be checked against minimum length in caller function and match
             * procedure can be stopped in case minimum length was reached.
             */
            if (id != originId)
            {
                length = 0;
            }
            else
            {
                length++;
            }
            return length;
        }

    }
}
