using UnityEngine;
using System;

namespace Assets.Scripts.Structs
{
    [Serializable]
    public struct NamedAudioClip
    {
        [SerializeField]
        private string m_identifier;
        [SerializeField]
        private AudioClip m_clip;

        public string Identifier
        {
            get
            {
                return m_identifier;
            }

            set
            {
                m_identifier = value;
            }
        }

        public AudioClip Clip
        {
            get
            {
                return m_clip;
            }

            set
            {
                m_clip = value;
            }
        }

        public NamedAudioClip(string identifier, AudioClip clip)
        {
            m_identifier = identifier;
            m_clip = clip;
        }
    }
}
