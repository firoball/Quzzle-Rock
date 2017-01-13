using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class HelpUI : MonoBehaviour
    {
        [SerializeField]
        GameObject m_MainMenu;

        void Start()
        {
            ExecuteEvents.Execute<IMenuEventTarget>(gameObject, null, (x, y) => x.Hide(true));
        }

        public void Close()
        {
            if (m_MainMenu != null)
            {
                StartCoroutine(ToggleMenu(m_MainMenu));
            }
            else
            {
                Debug.LogWarning("HelpUI: Menu reference not set.");
            }
        }

        private IEnumerator ToggleMenu(GameObject newMenu)
        {
            ExecuteEvents.Execute<IMenuEventTarget>(gameObject, null, (x, y) => x.Hide(false));
            yield return new WaitForSeconds(0.2f);
            ExecuteEvents.Execute<IMenuEventTarget>(newMenu, null, (x, y) => x.Show(false));
        }

    }
}
