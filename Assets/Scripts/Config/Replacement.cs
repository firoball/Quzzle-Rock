using UnityEngine;
using System;

namespace Assets.Scripts.Structs
{
    [Serializable]
    public struct Replacement
    {
        [SerializeField]
        private int m_originalId;
        [SerializeField]
        private int[] m_replacementIds;

        public int OriginalId
        {
            get
            {
                return m_originalId;
            }
        }

        public int[] ReplacementIds
        {
            get
            {
                return m_replacementIds;
            }
        }

        public int Size()
        {
            return m_replacementIds.Length;
        }

    }
}
