using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class BonusTextUI: MonoBehaviour
    {
        private CanvasGroup m_group;
        private RectTransform m_transform;
        private bool m_enableFade;

        private const float c_fadeDelay = 0.7f;
        private const float c_fadeSpeed = 1.2f;
        private const float c_moveSpeed = 0.3f;

        void Awake()
        {
            m_group = GetComponent<CanvasGroup>();
            m_transform = GetComponent<RectTransform>();
            m_enableFade = false;
        }

        void Start()
        {
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
                m_group.alpha = Mathf.Max(m_group.alpha - (c_fadeSpeed * Time.deltaTime), 0.0f);
            }
            Vector3 pos = m_transform.localPosition;
            pos.y += c_moveSpeed * Time.deltaTime;
            m_transform.localPosition = pos;
        }

        private IEnumerator WaitForFade()
        {
            yield return new WaitForSeconds(c_fadeDelay);
            m_enableFade = true;
        }
    }
}
