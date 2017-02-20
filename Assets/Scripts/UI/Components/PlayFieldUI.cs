using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class PlayFieldUI : DefaultUI, IDimmerEventTarget
    {
        [SerializeField]
        private GameObject m_lightSource;

        public override void OnShow(bool immediately)
        {
            PlayField.Unlock();
            OnDimmer(false);
            base.OnShow(immediately);
        }

        public override void OnHide(bool immediately)
        {
            PlayField.Lock();
            OnDimmer(true);
            base.OnHide(immediately);
        }

        public override void OpenMenu(GameObject newMenu)
        {
            base.OpenMenu(newMenu);
        }

        public void OnDimmer(bool enableDim)
        {
            if(enableDim)
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }
            //dimmer event is forwarded here so a light source reference is not required in multiple places
            ExecuteEvents.Execute<IDimmerEventTarget>(m_lightSource, null, (x, y) => x.OnDimmer(enableDim));
        }
    }
}
