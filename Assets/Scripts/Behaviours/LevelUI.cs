using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class LevelUI : DefaultUI
    {
        [SerializeField]
        private GameObject m_lockerUI;

        private void LockPlayField()
        {
            //make sure Playfield is locked with one big invisible panel
            if (m_lockerUI != null)
            {
                ExecuteEvents.Execute<IMenuEventTarget>(m_lockerUI, null, (x, y) => x.OnShow(true));
            }
        }

        private void UnlockPlayField()
        {
            //make sure Playfield is unlocked with one big invisible panel
            if (m_lockerUI != null)
            {
                ExecuteEvents.Execute<IMenuEventTarget>(m_lockerUI, null, (x, y) => x.OnHide(false));
            }
        }

        public override void OnShow(bool immediately)
        {
            UnlockPlayField();
            base.OnShow(immediately);
        }

        public override void OnHide(bool immediately)
        {
            LockPlayField();
            base.OnHide(immediately);
        }
    }
}
