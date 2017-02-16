using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.Structs;
using Assets.Scripts.Classes;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Behaviours
{
    public class PreferencesUI : DefaultUI
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
        private GameObject m_helpMenu;
        [SerializeField]
        private GameObject m_statisticsMenu;
        [SerializeField]
        private GameObject m_optionsMenu;
        [SerializeField]
        private GameObject m_customMenu;

        private Preferences[] m_preferences;

        void Start()
        {
            OnShow(false);
            Screen.sleepTimeout = SleepTimeout.SystemSetting;

            if (
                (m_textButtonPrefab != null) && (m_iconButtonPrefab != null)
                && m_textButtonPrefab.GetComponent<RectTransform>()
                && m_iconButtonPrefab.GetComponent<RectTransform>()
                )
            {
                RectTransform rectTransformText = m_textButtonPrefab.GetComponent<RectTransform>();
                RectTransform rectTransformIcon = m_iconButtonPrefab.GetComponent<RectTransform>();
                float xOffset = (rectTransformText.rect.width * 0.5f) + (rectTransformIcon.rect.width * 0.5f) + m_buttonSpacing;
                float yOffset = rectTransformText.rect.height + m_buttonSpacing;

                m_preferences = PreferencesConfig.Preferences;
                Vector3 pos = new Vector3(0.0f, m_buttonTop, 0.0f);
                Button button;
                foreach (Preferences preferences in m_preferences)
                {
                    button = CreateTextButton(pos, preferences.Name);
                    button.onClick.AddListener(() => SetPreferencesAndStart());
                    pos.y -= yOffset;
                }
                button = CreateTextButton(pos, "Custom");
                button.onClick.AddListener(() => OpenMenu(m_customMenu));

                pos.x -= xOffset;
                //exit button does not make sense for web application
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    button = CreateIconButton(pos, "x");
                    button.onClick.AddListener(() => OpenMenu(m_exitMenu));
                }

                pos = new Vector3(xOffset, m_buttonTop, 0.0f);
                button = CreateIconButton(pos, "book_alt2");
                button.onClick.AddListener(() => OpenMenu(m_helpMenu));
                pos.y -= yOffset;
                button = CreateIconButton(pos, "list");
                button.onClick.AddListener(() => OpenMenu(m_statisticsMenu));
                pos.y -= yOffset;
                button = CreateIconButton(pos, "cog");
                button.onClick.AddListener(() => OpenMenu(m_optionsMenu));
            }
            else
            {
                Debug.LogWarning("PreferencesUI: button prefabs are not set or are not uGui buttons.");
            }
        }

        /*void OnGUI()
        {

            string dbg = "";
            dbg += "\nisEditor: " + Application.isEditor;
            dbg += "\nisMobilePlatform: " + Application.isMobilePlatform;
            dbg += "\nisWebPlayer: " + Application.isWebPlayer;
            dbg += "\nisConsolePlatform: " + Application.isConsolePlatform;
            dbg += "\nplatform: " + Application.platform.ToString();
            GUI.Label(new Rect(10, 10, 300, 250), dbg);
        }*/

        private Button CreateTextButton(Vector3 position, string name)
        {
            return CreateGenericButton(position, name, m_textButtonPrefab);
        }

        private Button CreateIconButton(Vector3 position, string identifier)
        {
            string icon = IconButton.GetIconForIdentifier(identifier);
            return CreateGenericButton(position, icon, m_iconButtonPrefab);
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
                    Preferences preferences;
                    if (PreferencesConfig.Find(text.text, out preferences))
                    {
                        Preferences.Current = preferences;
                    }
                    LoadLevel(1);
                }
            }
        }

    }
}
