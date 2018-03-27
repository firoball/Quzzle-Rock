using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Controls
{
    [RequireComponent(typeof(RectTransform))]
    public class ResultVisualizerUI : MonoBehaviour, IResultSetTarget
    {
        private Text m_key;
        private Text m_value;

        void Awake()
        {
            Transform child;

            child = transform.Find("Key");
            m_key = child.GetComponent<Text>();

            child = transform.Find("Value");
            m_value = child.GetComponent<Text>();
        }

        public void OnSetResult(string key, string value)
        {
            if (m_key != null)
            {
                m_key.text = key;
            }
            if (m_value != null)
            {
                m_value.text = value;
            }
        }
    }
}
