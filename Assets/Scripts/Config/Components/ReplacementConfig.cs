using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Scripts.Structs;

namespace Assets.Scripts.Behaviours
{
    public class ReplacementConfig : MonoBehaviour
    {

        private static ReplacementConfig s_singleton = null;

        [SerializeField]
        private List<Replacement> m_replacements;

        public static List<Replacement> Replacements
        {
            get
            {
                if (s_singleton != null)
                {
                    return s_singleton.m_replacements;
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

            foreach (Replacement replacement in s_singleton.m_replacements)
            {
                /* try to find best match. If combination size is bigger than size of any
                 * replacement then pick at least best match
                 */
                if ((replacement.OriginalId == id) && (replacement.Size() <= size))
                {
                    found = true;
                    result = replacement.ReplacementIds;
                    //exact size match - it won't get any better. Abort search
                    if ((replacement.Size() == size))
                    {
                        break;
                    }
                }
            }

            return found;
        }

        void Awake()
        {
            if (s_singleton == null)
            {
                s_singleton = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.Log("ReplacementConfig: Multiple instances detected. Destroying...");
                Destroy(this);
            }

        }
    }
}
