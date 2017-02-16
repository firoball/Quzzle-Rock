using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Classes;

namespace Assets.Scripts.Behaviours
{
    public class ResultsUI : DefaultUI, IResultEventTarget
    {
        [SerializeField]
        private GameObject m_viewportContent;
        [SerializeField]
        private Scrollbar m_viewportScrollbar;
        [SerializeField]
        private GameObject m_endedUI;
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

        public override void OnShow(bool immediately)
        {
            //skip results menu when no results were reported (viewport content does not have any children)
            if ((m_viewportContent != null) 
                && (m_viewportContent.transform.childCount > 0)
                && Options.ShowResults
                )
            {
                SetHeight();
                base.OnShow(immediately);
            }
            else
            {
                //forward trigger to follow-up menu
                ExecuteEvents.Execute<IMenuEventTarget>(m_endedUI, null, (x, y) => x.OnShow(false));
            }
        }

        public override void OnHide(bool immediately)
        {
            base.OnHide(immediately);
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
            }

        }

        public void Ok()
        {
            /* use this function instead of button configuration in editor directly
             * in order to ensure consistency with results skip feature
             */
            OpenMenu(m_endedUI);
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
