using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class LevelUI : DefaultUI
    {

        void Update()
        {
            if (PlayTurn.HasEnded)
            {
                ExecuteEvents.Execute<IMenuEventTarget>(gameObject, null, (x, y) => x.Hide(false));
            }
        }

        /*public override void OpenMenu(GameObject newMenu)
        {
            OpenMenu(newMenu);
        }*/

    }
}
