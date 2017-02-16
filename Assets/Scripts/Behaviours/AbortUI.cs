using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class AbortUI : DefaultUI
    {
        public void Ok(GameObject newMenu)
        {
            OpenMenu(newMenu);
            PlayTurn.Abort();
        }
    }
}
