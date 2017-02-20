using UnityEngine;

namespace Game.Logic.Objects
{
    public class TokenMorphVisualizer : MonoBehaviour
    {
        private static ParticleSystem.Particle[] s_particles;

        private float m_timer;
        private ParticleSystem[] m_particleSystems;
        private Transform m_transform;

        private const float c_animationSpeed = 3.5f;
        private const float c_lifespan = 0.8f;
        private const float c_scaleSpeed = 1.7f;

        void Awake()
        {
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            m_transform = transform;
            //use single buffer for all token visualizer particle systems (memory reduction)
            if ((s_particles == null) && (m_particleSystems.Length > 0))
            {
                s_particles = new ParticleSystem.Particle[m_particleSystems[0].main.maxParticles];
            }
            Vector3 pos = m_transform.position;
            pos.z = -1;
            m_transform.position = pos;
            m_transform.localScale = Vector3.zero;
            m_timer = 0.0f;
        }

        void Start()
        {
            Destroy(gameObject, c_lifespan);
        }

        void Update()
        {
            m_timer = Mathf.Min(m_timer + (Time.deltaTime * c_scaleSpeed), 1.0f);
            m_transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, m_timer);
            Rotate();
        }

        private void Rotate()
        {
            Vector3 angle = m_transform.localRotation.eulerAngles;
            //undo parent rotation
            angle.y = -m_transform.parent.localRotation.eulerAngles.y;
            angle.z = ((Time.time * c_animationSpeed * 360.0f) % 360);
            Quaternion rot = m_transform.localRotation;
            rot.eulerAngles = angle;
            m_transform.localRotation = rot;
        }

    }
}
