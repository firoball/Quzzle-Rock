using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Behaviours
{
    public class DelayedAudio : MonoBehaviour
    {
        [SerializeField]
        private string m_audioClip = "";
        [SerializeField]
        private float m_delay = 0.0f;

        void Start()
        {
            StartCoroutine(SoundDelay());
        }

        private IEnumerator SoundDelay()
        {
            yield return new WaitForSeconds(m_delay);
            AudioManager.Play(m_audioClip);
        }
    }
}
