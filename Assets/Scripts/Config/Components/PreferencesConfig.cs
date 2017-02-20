using UnityEngine;
using System.Collections;
using Assets.Scripts.Structs;
using Assets.Scripts.Classes;

namespace Assets.Scripts.Behaviours
{

    public class PreferencesConfig : MonoBehaviour
    {
        private static PreferencesConfig s_singleton;

        [SerializeField]
        private Preferences[] m_preferences;

        public static Preferences[] Preferences
        {
            get
            {
                return s_singleton.m_preferences;
            }
        }

        public static bool Find(string name, out Preferences preferences)
        {
            preferences = new Preferences();
            foreach (Preferences pref in s_singleton.m_preferences)
            {
                if (pref.Name == name)
                {
                    preferences = pref;
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
