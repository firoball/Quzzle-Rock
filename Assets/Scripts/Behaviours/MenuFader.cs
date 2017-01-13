using UnityEngine;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(CanvasGroup))]
    class MenuFader : MonoBehaviour, IMenuEventTarget
    {
        [SerializeField]
        private float m_fadeSpeed = 4.0f;
        [SerializeField]
        private bool m_showOnStart = false;

        private CanvasGroup m_canvasGroup;
        private bool m_fadeOut = false;
        private bool m_fadeIn = false;


        void Awake()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
            if (m_showOnStart)
            {
                Enable();
            }
            else
            {
                Disable();
            }
        }

        void Update()
        {
            if (m_fadeOut)
            {
                m_canvasGroup.alpha = Mathf.Max(m_canvasGroup.alpha - (m_fadeSpeed * Time.deltaTime), 0.0f);
                if (m_canvasGroup.alpha <= 0.0f)
                {
                    m_fadeOut = false;
                    Disable();
                }

            }

            if (m_fadeIn)
            {
                m_canvasGroup.alpha = Mathf.Min(m_canvasGroup.alpha + (m_fadeSpeed * Time.deltaTime), 1.0f);
                if (m_canvasGroup.alpha >= 1.0f)
                {
                    m_fadeIn = false;
                    Enable();
                }

            }
        }

        private void Enable()
        {
            m_canvasGroup.alpha = 1.0f;
            m_canvasGroup.blocksRaycasts = true;
            m_canvasGroup.interactable = true;
        }

        private void Disable()
        {
            m_canvasGroup.alpha = 0.0f;
            m_canvasGroup.blocksRaycasts = false;
            m_canvasGroup.interactable = false;
        }

        public void Show(bool immediately)
        {
            m_canvasGroup.blocksRaycasts = false;
            m_canvasGroup.interactable = false;
            m_fadeOut = false;
            if (immediately)
            {
                Enable();
            }
            else
            {
                m_fadeIn = true;
            }

        }

        public void Hide(bool immediately)
        {
            m_canvasGroup.blocksRaycasts = false;
            m_canvasGroup.interactable = false;
            m_fadeIn = false;
            if (immediately)
            {
                Disable();
            }
            else
            {
                m_fadeOut = true;
            }
        }

    }
}
