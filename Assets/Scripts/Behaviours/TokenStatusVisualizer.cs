using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class TokenStatusVisualizer : MonoBehaviour, ITokenStatusEventTarget
    {
        private static ParticleSystem.Particle[] s_particles;

        private ParticleSystem[] m_particleSystems;
        private Transform m_transform;
        private bool m_enabled;
        private Vector3 m_parentPosition;

        private const float c_animationSpeed = 360.0f;

        void Awake()
        {
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            m_transform = transform;
            //use single buffer for all token visualizer particle systems (memory reduction)
            if ((s_particles == null) && (m_particleSystems.Length > 0))
            {
                s_particles = new ParticleSystem.Particle[m_particleSystems[0].main.maxParticles];
            }
            m_parentPosition = m_transform.parent.position;
            Disable();
        }

        void Update()
        {
            if (m_enabled)
            {
                Rotate();

                //if parent is moving, move all particles with it (for token swap)
                if (m_transform.parent.position != m_parentPosition)
                {
                    MoveWithParent();
                }
            }
            m_parentPosition = m_transform.parent.position;
        }

        public void OnSelect()
        {
            Enable(Color.green);
        }

        public void OnUnSelect()
        {
            Disable();
        }

        public void OnHover()
        {
            Enable(Color.white);
        }

        public void OnDrag()
        {
            Enable(Color.white);
        }

        public void OnSwap()
        {
            Enable(Color.yellow);
        }

        private void Enable(Color color)
        {
            m_enabled = true;
            foreach (ParticleSystem particle in m_particleSystems)
            {
                ParticleSystem.MainModule main = particle.main;
                main.startColor = color;
                //ParticleSystem.EmissionModule emission = particle.emission;
                //emission.enabled = true;
                particle.Play();
            }
        }

        private void Disable()
        {
            m_enabled = false;
            foreach (ParticleSystem particle in m_particleSystems)
            {
                particle.Clear();
                particle.Stop();
            }
        }

        private void MoveWithParent()
        {
            Vector3 diffPos = m_transform.parent.position - m_parentPosition;
            foreach (ParticleSystem particleSystem in m_particleSystems)
            {
                int particleCount = particleSystem.GetParticles(s_particles);
                for (int i = 0; i < particleCount; i++)
                {
                    s_particles[i].position += diffPos;
                }
                particleSystem.SetParticles(s_particles, particleCount);
            }
        }

        private void Rotate()
        {
            Vector3 angle = m_transform.localRotation.eulerAngles;
            //undo parent rotation
            angle.y = -m_transform.parent.localRotation.eulerAngles.y;
            angle.z = ((Time.time * c_animationSpeed) % 360);
            Quaternion rot = m_transform.localRotation;
            rot.eulerAngles = angle;
            m_transform.localRotation = rot;
        }
    }
}
