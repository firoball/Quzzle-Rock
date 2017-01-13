using UnityEngine;
using System;
using Assets.Scripts.Classes;

namespace Assets.Scripts.Behaviours
{
    public class TokenConfig : MonoBehaviour
    {
        private static TokenConfig s_singleton = null;

        [SerializeField]
        private GameObject[] m_standardTokenList;
        [SerializeField]
        private GameObject[] m_specialTokenList;

        public static GameObject[] StandardToken
        {
            get
            {
                if (s_singleton != null)
                {
                    return s_singleton.m_standardTokenList;
                }
                else
                {
                    return null;
                }
            }
        }

        public static GameObject[] SpecialToken
        {
            get
            {
                if (s_singleton != null)
                {
                    return s_singleton.m_specialTokenList;
                }
                else
                {
                    return null;
                }
            }
        }

        public void Awake()
        {
            if (s_singleton == null)
            {
                s_singleton = this;

                if (m_standardTokenList.Length < Preferences.Current.TokenCount)
                {
                    Debug.LogWarning("TokenConfig: Insufficient tokens defined! Configured " + m_standardTokenList.Length +
                        ", required " + Preferences.Current.TokenCount + ". Adjusting token count.");
                    //fallback. should be avoided.
                    Preferences.Current.TokenCount = m_standardTokenList.Length;
                }
            }
            else
            {
                Debug.LogWarning("TokenConfig: Multiple instanced detected. Destroying...");
                Destroy(this);
            }
        }
    }
}
