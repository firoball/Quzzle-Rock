﻿using UnityEngine;
using System.Collections;

namespace Game.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    class UIFader : MonoBehaviour
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
            m_canvasGroup.interactable = true;
            m_canvasGroup.blocksRaycasts = true;
        }

        private void Disable()
        {
            m_canvasGroup.alpha = 0.0f;
            //m_canvasGroup.blocksRaycasts = false;
            //m_canvasGroup.interactable = false;
            StartCoroutine(DelayedDisable());
        }

        public void Show(bool immediately)
        {
            //m_canvasGroup.blocksRaycasts = false;
            //m_canvasGroup.interactable = false;
            m_fadeOut = false;
            if (immediately)
            {
                Enable();
            }
            else
            {
                m_fadeIn = true;
                StartCoroutine(DelayedDisable());
            }

        }

        public void Hide(bool immediately)
        {
            //m_canvasGroup.blocksRaycasts = false;
            //m_canvasGroup.interactable = false;
            m_fadeIn = false;
            if (immediately)
            {
                Disable();
            }
            else
            {
                m_fadeOut = true;
                StartCoroutine(DelayedDisable());
            }
        }

        //Work around button highlighting bug
        private IEnumerator DelayedDisable()
        {
            m_canvasGroup.blocksRaycasts = false;
            yield return new WaitForSeconds(0.01f);
            m_canvasGroup.interactable = false;
        }
    }
}
