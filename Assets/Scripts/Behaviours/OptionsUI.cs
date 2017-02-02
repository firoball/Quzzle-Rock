using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class OptionsUI : DefaultUI
    {

        public void Confirm(GameObject newMenu)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject target = transform.GetChild(i).gameObject;
                ExecuteEvents.Execute<IOptionEventTarget>(target, null, (x, y) => x.OnConfirm());
            }
            OpenMenu(newMenu);
        }

        public void Abort(GameObject newMenu)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject target = transform.GetChild(i).gameObject;
                ExecuteEvents.Execute<IOptionEventTarget>(target, null, (x, y) => x.OnAbort());
            }
            OpenMenu(newMenu);
        }
    }
}
