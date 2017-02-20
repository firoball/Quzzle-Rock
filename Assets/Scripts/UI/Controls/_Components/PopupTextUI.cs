using UnityEngine;
using System.Collections;
using Game.Logic;

namespace Game.UI.Controls
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class PopupTextUI: MonoBehaviour
    {
        private CanvasGroup m_group;
        private RectTransform m_transform;
        private bool m_enableFade;
        private float m_scaleFactor;
        private Vector3 m_originalScale;

        [SerializeField]
        private float m_fadeDelay = 0.7f;
        [SerializeField]
        private float m_fadeSpeed = 1.2f;
        [SerializeField]
        private float m_moveSpeed = 0.3f;
        [SerializeField]
        private bool m_enableScale = false;
        [SerializeField]
        private float m_scaleSpeed = 0.3f;
        [SerializeField]
        private Vector3 m_targetScale = Vector3.one;
        [SerializeField]
        private bool m_scaleWithPlayField = true;

        void Awake()
        {
            m_group = GetComponent<CanvasGroup>();
            m_transform = GetComponent<RectTransform>();
        }

        void Start()
        {
            m_enableFade = false;
            m_scaleFactor = 0.0f;
            if (!m_scaleWithPlayField)
            {
                Bounds size = PlayField.GetDimension();
                transform.localScale = Vector3.Scale(transform.localScale, size.extents / 5.1f);
            }
            m_originalScale = transform.localScale;
            StartCoroutine(WaitForFade());
        }

        void Update()
        {
            if (m_enableFade)
            {
                if (m_group.alpha <= 0.0f)
                {
                    Destroy(gameObject.transform.parent.gameObject);
                }
                m_group.alpha = Mathf.Max(m_group.alpha - (m_fadeSpeed * Time.deltaTime), 0.0f);
            }

            if (m_enableScale)
            {
                transform.localScale = Vector3.Lerp(m_originalScale, m_targetScale, m_scaleFactor);
                m_scaleFactor = Mathf.Min(m_scaleFactor + (m_scaleSpeed * Time.deltaTime), 1.0f);
            }

            Vector3 pos = m_transform.localPosition;
            pos.y += m_moveSpeed * Time.deltaTime;
            m_transform.localPosition = pos;
        }

        private IEnumerator WaitForFade()
        {
            yield return new WaitForSeconds(m_fadeDelay);
            m_enableFade = true;
        }
    }
}
