using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Game.UI.Controls
{
    public class ScrollviewVisualizerUI : MonoBehaviour, IResultEventTarget
    {

        [SerializeField]
        private GameObject m_viewportContent;
        [SerializeField]
        private Scrollbar m_viewportScrollbar;
        [SerializeField]
        private GameObject m_resultSetPrefab;
        [SerializeField]
        private float m_resultEntryStartPos = -10.0f;
        [SerializeField]
        private float m_resultGroupSpacing = 20.0f;

        private float m_positionTracker;
        private float m_resultSetHeight;

        void Start()
        {
            m_resultSetHeight = m_resultGroupSpacing;
            //get height of result set from its RectTransform
            if (m_resultSetPrefab != null)
            {
                RectTransform rect = m_resultSetPrefab.GetComponent<RectTransform>();
                if (rect != null)
                {
                    m_resultSetHeight = rect.rect.height;
                }
            }
        }

        public void OnAddResults(Dictionary<string, string> results)
        {
            if (m_viewportContent != null)
            {
                if (m_viewportContent.transform.childCount > 0)
                {
                    m_positionTracker -= m_resultGroupSpacing;
                }
                else
                {
                    m_positionTracker = m_resultEntryStartPos;
                }
                foreach (KeyValuePair<string, string> kvp in results)
                {
                    Vector3 position = new Vector3(0.0f, m_positionTracker, 0.0f);
                    GameObject resultSet = Instantiate(m_resultSetPrefab, position, Quaternion.identity);
                    resultSet.transform.SetParent(m_viewportContent.transform, false);
                    ExecuteEvents.Execute<IResultSetTarget>(resultSet, null, (x, y) => x.OnSetResult(kvp.Key, kvp.Value));
                    m_positionTracker -= m_resultSetHeight;
                }
                SetHeight();
            }

        }

        public void OnReset()
        {
            //scroll back to top
            if (m_viewportScrollbar != null)
            {
                m_viewportScrollbar.value = 1.0f;
            }

            //remove all viewport content
            if (m_viewportContent != null)
            {
                for (int i = m_viewportContent.transform.childCount - 1; i >= 0; i--)
                {
                    Transform child = m_viewportContent.transform.GetChild(i);
                    child.SetParent(null);
                    Destroy(child.gameObject);
                }
            }
        }

        private void SetHeight()
        {
            RectTransform rect = m_viewportContent.GetComponent<RectTransform>();
            if (rect != null)
            {
                float height = -1.0f * (m_positionTracker - m_resultEntryStartPos);
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, height);
            }
        }

    }
}
