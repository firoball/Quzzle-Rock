using UnityEngine;
using Game.Logic;

namespace Game.UI
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
