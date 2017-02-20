using UnityEngine;

namespace Assets.Scripts.Behaviours
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
