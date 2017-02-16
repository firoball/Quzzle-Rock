using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Classes;

namespace Assets.Scripts.Behaviours
{
    public class ResultsUI : DefaultUI, IResultEventTarget
    {
        [SerializeField]
        private GameObject m_scrollView;
        [SerializeField]
        private GameObject m_viewportContent;
        [SerializeField]
        private GameObject m_endedUI;

        public override void OnShow(bool immediately)
        {
            //skip results menu when no results were reported (viewport content does not have any children)
            if ((m_viewportContent != null) 
                && (m_viewportContent.transform.childCount > 0)
                && Options.ShowResults
                )
            {
                base.OnShow(immediately);
            }
            else
            {
                //forward trigger to follow-up menu
                ExecuteEvents.Execute<IMenuEventTarget>(m_endedUI, null, (x, y) => x.OnShow(false));
            }
        }

        public void OnAddResults(Dictionary<string, string> results)
        {
            //forward to scrollView
            ExecuteEvents.Execute<IResultEventTarget>(m_scrollView, null, (x, y) => x.OnAddResults(results));
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
            //forward to scrollView
            ExecuteEvents.Execute<IResultEventTarget>(m_scrollView, null, (x, y) => x.OnReset());
        }

    }
}
