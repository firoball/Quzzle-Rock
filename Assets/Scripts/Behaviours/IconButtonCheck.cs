using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(Text))]
    public class IconButtonCheck : MonoBehaviour
    {

        void Start()
        {
            char icon = '\u2717'; //check
            Text text = GetComponent<Text>();
            text.text = icon.ToString();
        }
    }
}
