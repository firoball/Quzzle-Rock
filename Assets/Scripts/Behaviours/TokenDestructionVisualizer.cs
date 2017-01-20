using UnityEngine;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(ParticleSystem))]
    public class TokenDestructionVisualizer : MonoBehaviour, ITokenDestructionEventTarget
    {
        private ParticleSystem m_particleSystem;

        void Awake()
        {
            m_particleSystem = GetComponent<ParticleSystem>();
        }

        public void OnDestruction()
        {
            Vector3 pos = transform.position;
            pos.z -= 1.0f; //make sure particle effect layers everything. Cheap hack to avoid child GO
            transform.position = pos;
            m_particleSystem.Play();
        }
    }
}
