using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Scripts.Structs;

namespace Assets.Scripts.Behaviours
{
    public class ReplacementConfig : MonoBehaviour
    {

        private static ReplacementConfig singleton = null;

        [SerializeField]
        private List<Replacement> m_replacements;

        public static List<Replacement> Replacements
        {
            get
            {
                if (singleton != null)
                {
                    return singleton.m_replacements;
                }
                else
                {
                    return null;
                }
            }
        }

        public static bool Find(int id, int size, out int[] result)
        {
            bool found = false;
            result = null;

            foreach (Replacement replacement in singleton.m_replacements)
            {
                if ((replacement.OriginalId == id) && (replacement.Size() == size))
                {
                    found = true;
                    result = replacement.ReplacementIds;
                    break;
                }
            }

            return found;
        }

        void Awake()
        {
            if (singleton == null)
            {
                singleton = this;
            }
            else
            {
                Debug.LogWarning("ReplacementConfig: Multiple instanced detected. Destroying...");
                Destroy(this);
            }

        }
    }
}
