using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class StatisticsUI : DefaultUI
    {
        [SerializeField]
        private GameObject m_dropDown;

        public override void OnShow(bool immediately)
        {
            ExecuteEvents.Execute<IStatisticsEventTarget>(m_dropDown, null, (x, y) => x.OnShow());
            base.OnShow(immediately);
        }

    }
}
