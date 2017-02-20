using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class EndedUI : DefaultUI
    {
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
