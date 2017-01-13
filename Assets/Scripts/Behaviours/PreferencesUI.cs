using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Assets.Scripts.Structs;
using Assets.Scripts.Classes;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class PreferencesUI : MonoBehaviour
    {
        [SerializeField]
        [Range(-400.0f, 0.0f)]
        private float m_buttonTop = -50.0f;
        [SerializeField]
        [Range(0.0f, 400.0f)]
        private float m_buttonSpacing = 20.0f;
        [SerializeField]
        private GameObject m_textButtonPrefab;
        [SerializeField]
        private GameObject m_iconButtonPrefab;
        [SerializeField]
        private GameObject m_exitMenu;
        [SerializeField]
        private GameObject m_HelpMenu;
        [SerializeField]
        private GameObject m_StatisticsMenu;

        private PreferencesSet[] m_preferencesSets;

        void Start()
        {
            if (
                (m_textButtonPrefab != null) && (m_iconButtonPrefab != null)
                && m_textButtonPrefab.GetComponent<RectTransform>()
                && m_iconButtonPrefab.GetComponent<RectTransform>()
                )
            {
                RectTransform rectTransformText = m_textButtonPrefab.GetComponent<RectTransform>();
                RectTransform rectTransformIcon = m_iconButtonPrefab.GetComponent<RectTransform>();
                float xOffset = (rectTransformText.rect.width * 0.5f) + (rectTransformIcon.rect.width  * 0.5f)+ m_buttonSpacing;
                float yOffset = rectTransformText.rect.height + m_buttonSpacing;

                m_preferencesSets = PreferencesConfig.PreferencesSets;
                Vector3 pos = new Vector3(0.0f, m_buttonTop, 0.0f);
                Button button;
                foreach (PreferencesSet preferenceSet in m_preferencesSets)
                {
                    button = CreateTextButton(pos, preferenceSet.Name);
                    button.onClick.AddListener(() => SetPreferencesAndStart());
                    pos.y -= yOffset;
                }
                button = CreateTextButton(pos, "Custom");

                pos.x += xOffset;
                //exit button does not make sense for web application
                if (!Application.isWebPlayer)
                {
                    button = CreateIconButton(pos, '\u2713'); //x
                    button.onClick.AddListener(() => ShowMenu(m_exitMenu));
                }

                pos = new Vector3(xOffset, m_buttonTop, 0.0f);
                button = CreateIconButton(pos, '\ue06a'); //book_alt2
                button.onClick.AddListener(() => ShowMenu(m_HelpMenu));
                pos.y -= yOffset;
                button = CreateIconButton(pos, '\ue055'); //list
                button.onClick.AddListener(() => ShowMenu(m_StatisticsMenu));
                pos.y -= yOffset;
            }
            else
            {
                Debug.LogWarning("PreferencesUI: button prefabs are not set or are not uGui buttons.");
            }

        }

        private Button CreateTextButton(Vector3 position, string name)
        {
            return CreateGenericButton(position, name, m_textButtonPrefab);
        }

        private Button CreateIconButton(Vector3 position, char icon)
        {
            return CreateGenericButton(position, icon.ToString(), m_iconButtonPrefab);
        }

        private Button CreateGenericButton(Vector3 position, string name, GameObject prefab)
        {
            GameObject buttonObject = (GameObject)Instantiate(prefab, position, Quaternion.identity);
            buttonObject.transform.SetParent(transform, false);
            buttonObject.name = name;
            Button button = buttonObject.GetComponent<Button>();
            Text text = buttonObject.GetComponentInChildren<Text>();
            text.text = name;

            return button;
        }

        private void SetPreferencesAndStart()
        {
            GameObject obj = EventSystem.current.currentSelectedGameObject;
            if (obj != null)
            {
                Button button = obj.GetComponent<Button>();
                if (button != null)
                {
                    Text text = button.GetComponentInChildren<Text>();
                    PreferencesSet set;
                    if (PreferencesConfig.Find(text.text, out set))
                    {
                        Preferences.Current = set.Preferences;
                    }
                    StartCoroutine(StartGame());
                }
            }
        }

        private void ExitGame()
        {
            ExecuteEvents.Execute<IMenuEventTarget>(gameObject, null, (x, y) => x.Hide(false));
        }

        private void ShowMenu(GameObject newMenu)
        {
            if (newMenu != null)
            {
                StartCoroutine(ToggleMenu(newMenu));
            }
            else
            {
                Debug.LogWarning("PreferencesUi: Menu reference not set.");
            }
        }

        private IEnumerator ToggleMenu(GameObject newMenu)
        {
            ExecuteEvents.Execute<IMenuEventTarget>(gameObject, null, (x, y) => x.Hide(false));
            yield return new WaitForSeconds(0.2f);
            ExecuteEvents.Execute<IMenuEventTarget>(newMenu, null, (x, y) => x.Show(false));
        }

        private IEnumerator StartGame()
        {
            ExecuteEvents.Execute<IMenuEventTarget>(gameObject, null, (x, y) => x.Hide(false));
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadScene("level");
        }

    }
}
