using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PlayfieldDecorator : MonoBehaviour
    {
        private ParticleSystem m_particles;
        private Bounds m_bounds;

        private const float c_density = 0.03f;//0.15f;
        private const float c_shapeScale = 2.0f;

        void Awake()
        {
            m_particles = GetComponent<ParticleSystem>();
        }

        void Start()
        {
            m_bounds = PlayField.GetDimension();
            SetShapeSize();
        }

        private void SetShapeSize()
        {
            //take care of background size
            ParticleSystem.ShapeModule shape = m_particles.shape;
            shape.box = new Vector3(m_bounds.extents.x, m_bounds.extents.y, 0.0f) * c_shapeScale;

            //adjust emission rate
            ParticleSystem.EmissionModule emission = m_particles.emission;
            ParticleSystem.MinMaxCurve startRate = emission.rateOverTime;
            emission.rateOverTime = (m_bounds.extents.x * m_bounds.extents.y) * c_density * startRate.constant;
        }
    }
}
