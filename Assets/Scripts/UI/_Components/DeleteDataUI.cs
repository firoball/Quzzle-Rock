using UnityEngine;
using Game.Ambient;

namespace Game.UI
{
    public class DeleteDataUI : DefaultUI
    {
        public void Delete(GameObject menu)
        {
            AudioManager.Play("delete");
            PlayerPrefs.DeleteAll();
            OpenMenu(menu);
        }

    }
}
