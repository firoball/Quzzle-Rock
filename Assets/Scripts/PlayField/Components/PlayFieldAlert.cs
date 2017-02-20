using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Camera))]
    public class PlayFieldAlert : MonoBehaviour, ICameraAlertTarget
    {
        [SerializeField]
        private Color m_alertColor = Color.red;

        private float m_alertTimer;
        private bool m_active;
        private Color m_originalColor;
        private Camera m_camera;

        private const float c_alertPause = 5.0f;
        private const float c_alertMinPause = 1.0f;
        private const float c_alertSpeed = 4.0f;

        void Awake()
        {
            m_camera = GetComponent<Camera>();
            m_active = false;
            m_alertTimer = 0.0f;
        }

        void Start()
        {
            m_originalColor = m_camera.backgroundColor;
        }

        void Update()
        {
            if (m_active)
            {
                AlertUpdate();
            }
        }

        public void OnAlert(float intensity)
        {
            StopAllCoroutines();

            intensity = Mathf.Clamp01(intensity);
            if (intensity > 0.0f)
            {
                float pause = c_alertPause * ((1.0f - intensity));
                StartCoroutine(AlertTrigger(pause));
            }
        }

        private void AlertUpdate()
        {
            float alertFac = Mathf.Sin(m_alertTimer);
            if (alertFac < 0.0f)
            {
                m_active = false;
            }
            alertFac = Mathf.Clamp01(alertFac);
            m_camera.backgroundColor = Color.Lerp(m_originalColor, m_alertColor, alertFac);
            m_alertTimer += Time.deltaTime * c_alertSpeed;
        }

        private IEnumerator AlertTrigger(float pause)
        {
            while (true)
            {
                yield return new WaitForSeconds(c_alertMinPause);
                AudioManager.Play("alert");
                m_alertTimer = 0.0f;
                m_active = true;
                yield return new WaitForSeconds(pause);
            }
        }
    }
}
