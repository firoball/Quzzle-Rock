using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Renderer))]
    public class TokenStatusVisualizer : MonoBehaviour, ITokenStatusEventTarget
    {
        private Renderer m_renderer;

        void Awake()
        {
            m_renderer = GetComponent<Renderer>();
        }

        public void OnSelect()
        {
            m_renderer.enabled = true;
            m_renderer.material.color = Color.green;
        }

        public void OnUnSelect()
        {
            m_renderer.enabled = false;
        }

        public void OnHover()
        {
            m_renderer.enabled = true;
            m_renderer.material.color = Color.white;
        }

        public void OnDrag()
        {
            m_renderer.enabled = true;
            m_renderer.material.color = Color.white;
        }

        public void OnSwap()
        {
            m_renderer.enabled = true;
            m_renderer.material.color = Color.yellow;
        }
    }
}
