using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField]
    private bool m_show = true;

    private float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        if (m_show)
        {
            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(10, 100, 100, 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = 16;
            style.normal.textColor = Color.white;
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }
    }
}