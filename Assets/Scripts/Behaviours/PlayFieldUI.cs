using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class PlayFieldUI : DefaultUI
    {
        public override void OnShow(bool immediately)
        {
            PlayField.Unlock();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            base.OnShow(immediately);
        }

        public override void OnHide(bool immediately)
        {
            PlayField.Lock();
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            base.OnHide(immediately);
        }
    }
}
