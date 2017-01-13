using UnityEngine;
using System;
using System.Collections;
using Assets.Scripts.Classes;

namespace Assets.Scripts.Structs
{
    [Serializable]
    public struct PreferencesSet
    {
        [SerializeField]
        private string m_name;
        [SerializeField]
        private Preferences m_preferences;

        public string Name
        {
            get
            {
                return m_name;
            }

            set
            {
                m_name = value;
            }
        }

        public Preferences Preferences
        {
            get
            {
                return m_preferences;
            }

            set
            {
                m_preferences = value;
            }
        }
    }
}
