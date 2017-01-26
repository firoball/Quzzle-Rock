using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Collider))]
    public class TokenAnimator : MonoBehaviour, ITokenEventTarget,
        IPointerEnterHandler, IPointerExitHandler
    {
        private static GameObject s_hoverToken = null;

        private Vector3 m_originalRotation;
        private Vector3 m_currentRotation;
        private Vector3 m_originalPosition;
        private Vector3 m_targetPosition;
        private float m_hoverTimer;
        private float m_moveTimer;
        private float m_moveTimerReverse;
        private float m_removeTimer;
        private float m_refTime;
        private ParticleSystem m_particleSystem;

        private const float c_animationSpeed = 5.0f;
        private const float c_slowDownSpeed = 2.5f;
        private const float c_animationAngle = 20.0f;
        private const float c_movementSpeed = 5.0f;
        private const float c_scalingSpeed = 5.0f;
        private const float c_removeSpeed = 500.0f;

        void Awake()
        {
            m_originalRotation = transform.eulerAngles;
            m_hoverTimer = 0.0f;
            m_moveTimer = 0.0f;
            m_moveTimerReverse = 0.0f;
            m_removeTimer = 0.0f;
            m_refTime = Time.time;
            m_particleSystem = GetComponent<ParticleSystem>();
        }

        void Update() //TODO: could be moved to Coroutines...
        {
            //smoothly fade back to original position
            if (m_hoverTimer > 0.0f)
            {
                m_hoverTimer = Mathf.Max(0.0f, m_hoverTimer - c_slowDownSpeed * Time.deltaTime);
                transform.eulerAngles = Vector3.Slerp(m_originalRotation, m_currentRotation, m_hoverTimer);
            }

            //move to new position
            if (m_moveTimer > 0.0f)
            {
                m_moveTimer = Mathf.Max(0.0f, m_moveTimer - c_movementSpeed * Time.deltaTime);
                transform.position = Vector3.Lerp(m_targetPosition, m_originalPosition, m_moveTimer);
            }

            //move back to old position
            if ((m_moveTimer <= 0.0f) && (m_moveTimerReverse > 0.0f))
            {
                m_moveTimerReverse = Mathf.Max(0.0f, m_moveTimerReverse - c_movementSpeed * Time.deltaTime);
                transform.position = Vector3.Lerp(m_originalPosition, m_targetPosition, m_moveTimerReverse);
            }

            //token is being removed
            if (m_removeTimer > 0.0f)
            {
                m_removeTimer = Mathf.Max(0.0f, m_removeTimer - c_scalingSpeed * Time.deltaTime);
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, m_removeTimer);
                transform.Rotate(0.0f, 0.0f, Time.deltaTime * c_removeSpeed);
            }

            //token is being hovered
            if (s_hoverToken == gameObject)
            {
                float angle = c_animationAngle * Mathf.Sin(c_animationSpeed * (Time.time - m_refTime));
                m_currentRotation = new Vector3(m_originalRotation.x, angle, m_originalRotation.z);
                transform.eulerAngles = m_currentRotation;
            }
        }

        public void AnimateDestruction()
        {
            Vector3 pos = transform.position;
            pos.z -= 1.0f; //make sure particle effect layers everything. Cheap hack to avoid child GO
            transform.position = pos;
            m_particleSystem.Play();
        }

        #region events
        public void OnMoveTo(Vector3 newPosition)
        {
            m_originalPosition = transform.position;
            m_targetPosition = newPosition;
            m_moveTimer = 1.0f;
            m_moveTimerReverse = 0.0f;
        }

        public void OnFakeMoveTo(Vector3 newPosition)
        {
            OnMoveTo(newPosition);
            m_moveTimerReverse = 1.0f;
        }

        public void OnRemove()
        {
            StartCoroutine(WaitRemove(0.0f));
        }

        public void OnRemove(float delay)
        {
            delay = Mathf.Max(delay, 0.0f);
            StartCoroutine(WaitRemove(delay));
        }

        public void OnPointerEnter(PointerEventData data)
        {
            s_hoverToken = gameObject;

            m_hoverTimer = 0.0f;
            m_refTime = Time.time;
            transform.eulerAngles = m_originalRotation;
        }

        public void OnPointerExit(PointerEventData data)
        {
            s_hoverToken = null;

            m_hoverTimer = 1.0f;
        }
        #endregion events

        private IEnumerator WaitRemove(float delay)
        {
            yield return new WaitForSeconds(delay);
            m_removeTimer = 1.0f;
            Destroy(gameObject, 0.6f);
            AnimateDestruction();
        }

    }
}
