using UnityEngine;
using UnityEngine.EventSystems;
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
            PlayerPrefs.Save();
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
