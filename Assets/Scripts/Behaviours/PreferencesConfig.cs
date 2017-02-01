using UnityEngine;
using System.Collections;
using Assets.Scripts.Structs;

namespace Assets.Scripts.Behaviours
{

    public class PreferencesConfig : MonoBehaviour
    {
        private static PreferencesConfig s_singleton;

        [SerializeField]
        private PreferencesSet[] m_preferencesSets;

        public static PreferencesSet[] PreferencesSets
        {
            get
            {
                return s_singleton.m_preferencesSets;
            }
        }

        public static bool Find(string name, out PreferencesSet preferencesSet)
        {
            preferencesSet = new PreferencesSet();
            foreach (PreferencesSet set in s_singleton.m_preferencesSets)
            {
                if (set.Name == name)
                {
                    preferencesSet = set;
                    return true;
                }
            }

            return false;
        }

        void Awake()
        {
            if (s_singleton == null)
            {
                s_singleton = this;
            }
            else
            {
                Debug.Log("PreferencesConfig: Multiple instances detected. Destroying...");
                Destroy(this);
            }
        }

    }
}
