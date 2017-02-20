using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections.Generic;

namespace Game.UI.Controls
{
    [RequireComponent(typeof(Text))]
    public class IconButton : MonoBehaviour
    {
        private static bool s_regexDone = false;
        private static Dictionary<string, string> s_iconDict = new Dictionary<string, string>();

        [SerializeField]
        private string m_icon;
        [SerializeField]
        private TextAsset m_textAsset;
        [SerializeField]
        private string m_matchPattern = @".([A-Za-z0-9_\-]+):before[\s]*{[\s]*content:[\s]*\'\\([A-Fa-f0-9\-]+)\'";
        [SerializeField]
        private bool m_debug = false;

        public string Icon
        {
            get
            {
                return m_icon;
            }

            set
            {
                m_icon = value;
                UpdateIcon();
            }
        }

        void Awake()
        {
            //do parsing only for first icon. Lazy and somewhat unclean solution
            if (!s_regexDone)
            {
                ParseStylesheet();
                s_regexDone = true;
            }
            UpdateIcon();
        }

        private void ParseStylesheet()
        {
            if (m_textAsset != null)
            {
                foreach (Match match in Regex.Matches(m_textAsset.ToString(), m_matchPattern, RegexOptions.IgnoreCase))
                {
                    string key = match.Groups[1].Value;
                    char value = (char)int.Parse(match.Groups[2].Value, NumberStyles.HexNumber);
                    s_iconDict.Add(key, value.ToString());
                    if (m_debug)
                    {
                        Debug.Log("Icon added: " + key + " - \\u" + match.Groups[2].Value);
                    }
                }
            }
        }

        private void UpdateIcon()
        {
            string icon = GetIconForIdentifier(m_icon);
            Text text = GetComponent<Text>();
            text.text = icon;
        }

        public static string GetIconForIdentifier(string identifier)
        {
            string icon;
            if (!s_iconDict.TryGetValue(identifier, out icon))
            {
                icon = " ";
            }
            return icon;
        }
    }
}
