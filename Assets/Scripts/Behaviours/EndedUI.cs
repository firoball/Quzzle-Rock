using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Assets.Scripts.Classes;

namespace Assets.Scripts.Behaviours
{
    public class EndedUI : DefaultUI
    {

        void OnGUI()
        {
            GUI.Label(new Rect(10, 70, 100, 20), EventSystem.current.IsPointerOverGameObject().ToString());

        }
        public void Retry(GameObject newMenu)
        {
            OpenMenu(newMenu);
            PlayTurn.Retry();
        }

        public void New(GameObject newMenu)
        {
            OpenMenu(newMenu);
            PlayTurn.New();
        }

    }
}
