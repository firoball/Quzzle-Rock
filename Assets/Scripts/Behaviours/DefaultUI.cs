using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class DefaultUI : MonoBehaviour
    {
        public virtual void OpenMenu(GameObject newMenu)
        {
            if (newMenu != null)
            {
                StartCoroutine(OpenMenuDelayed(newMenu));
            }
        }

        public virtual void LoadLevel(int sceneIndex)
        {
            StartCoroutine(LoadLevelDelayed(sceneIndex));
        }

        public virtual void ExitGame()
        {
            StartCoroutine(ExitGameDelayed());
        }

        /*public void Show()
        {
            ExecuteEvents.Execute<IMenuEventTarget>(gameObject, null, (x, y) => x.Show(false));
        }

        public void Hide()
        {
            ExecuteEvents.Execute<IMenuEventTarget>(gameObject, null, (x, y) => x.Hide(false));
        }*/

        private IEnumerator OpenMenuDelayed(GameObject newMenu)
        {
            ExecuteEvents.Execute<IMenuEventTarget>(gameObject, null, (x, y) => x.Hide(false));
            yield return new WaitForSeconds(0.2f);
            ExecuteEvents.Execute<IMenuEventTarget>(newMenu, null, (x, y) => x.Show(false));
        }

        private IEnumerator LoadLevelDelayed(int sceneIndex)
        {
            ExecuteEvents.Execute<IMenuEventTarget>(gameObject, null, (x, y) => x.Hide(false));
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadScene(sceneIndex);
        }

        private IEnumerator ExitGameDelayed()
        {
            ExecuteEvents.Execute<IMenuEventTarget>(gameObject, null, (x, y) => x.Hide(false));
            yield return new WaitForSeconds(0.2f);
            Application.Quit();
            Debug.Log("Application.Quit()");
        }

    }
}
